using System;
using System.Collections.Generic;
using System.Linq;
using DBAgent.Watcher.Entities;
using DBAgent.Watcher.Enums;
using DBAgent.Watcher.Events.Args;
using DBAgent.Watcher.Events.Handlers;
using DBAgent.Watcher.Extensions;
using DBAgent.Watcher.Helpers;
using DBAgent.Watcher.Models;
using DBAgent.Watcher.Readers;
using FirebirdSql.Data.FirebirdClient;
using Melnik.Logging;
using Microsoft.Extensions.Logging;

namespace DBAgent.Watcher
{
    public class FbSqlWatcher: IDisposable
    {
        private readonly FbSqlWatcherOptions _options;
        private readonly TriggersStorage _triggersStorage;
        private readonly FbSqlReader _tempDbReader;
        private FbRemoteEvent _remoteEvent;

        public FbSqlWatcher(FbSqlWatcherOptions options)
        {
            _options = options;
            _triggersStorage = new TriggersStorage(options.TriggersFilePath, true);
            _triggersStorage.AddTriggersFromFileSafe(_options.TriggersFilePath);
            _tempDbReader = new FbSqlReader(WatcherResourceManager.TempDbConnectionStringSql);
            Logger = new LoggerFactory().CreateLogger<FbSqlWatcher>();
        }

        public FbSqlWatcher(FbSqlWatcherOptions options, ILogger logger)
        {
            _options = options;
            _triggersStorage = new TriggersStorage(options.TriggersFilePath, true);
            _triggersStorage.AddTriggersFromFileSafe(_options.TriggersFilePath);
            _tempDbReader = new FbSqlReader(WatcherResourceManager.TempDbConnectionStringSql);
            Logger = logger;
        }

        public IEnumerable<TriggerMetaData> CurrentTriggers => _triggersStorage.Triggers;
        protected ILogger Logger { get; private set; }

        public event ProcessEventsChangedEventHandler ProcessEventsChanged;

        public void EnsureAllTriggersExists()
        {
            using (Logger.BeginScope(LoggerHelper.GetCaller()))
            {
                try
                {
                    foreach (var table in _options.Tables)
                    {
                        var triggers = EnsureTriggersExists(table);

                        foreach (var trigger in triggers.Where(trigger => _triggersStorage.IsContains(trigger) == false))
                            _triggersStorage.AddTrigger(trigger);

                        Logger.LogDebug($"Triggers for table: {table} created");
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.ToString());
                    throw;
                }
            }

        }

        public void EnsureAllTriggersRemoved()
        {
            foreach (var triggerMetaData in CurrentTriggers)
            {
                EnsureTriggerRemoved(triggerMetaData);
            }
        }

        public void EnsureTriggerRemoved(TriggerMetaData trigger)
        {
            using (Logger.BeginScope(LoggerHelper.GetCaller()))
            {
                var triggerName = trigger.Name;

                try
                {
                    using (var connection = new FbConnection(GetConnectionString()))
                    {
                        connection.Open();

                        var cmdQuery = $"DROP TRIGGER {triggerName}";

                        using (var command = new FbCommand(cmdQuery, connection))
                        {
                            command.ExecuteNonQuery();
                        }

                        connection.Close();
                        _triggersStorage.RemoveTrigger(trigger);
                        Logger.LogDebug($"Trigger removed: {trigger.Name}");
                    }
                }
                catch (FbException ex)
                {
                    // trigger already deleted
                    if (ex.ErrorCode == 335544351)
                    {
                        Logger.LogWarning($"Trigger already removed: {trigger.Name}");
                        _triggersStorage.RemoveTrigger(trigger);
                        return;
                    }
                    throw;
                }
            }
        }

        public void InsertRandomToProcessEvents()
        {
            using (Logger.BeginScope(LoggerHelper.GetCaller()))
            {
                using (var connection = new FbConnection(GetConnectionString()))
                {
                    connection.Open();
                    var rnd = new Random();
                    var id = rnd.Next(1, 1000000);

                    var cmdQuery = $"INSERT INTO PROCESS_EVENTS (ID) VALUES ({id});";

                    using (var command = new FbCommand(cmdQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                    Logger.LogDebug("PROCESS_EVENTS random insert executed");
                }
            }

        }

        public void InitializeListeners()
        {
            using (Logger.BeginScope(LoggerHelper.GetCaller()))
            {
                _remoteEvent?.Dispose();

                _remoteEvent = new FbRemoteEvent(GetConnectionString());

                var triggers = _triggersStorage.Triggers;
                var events = triggers.Select(item => item.EventName);
                _remoteEvent.QueueEvents(events.ToArray());
                _remoteEvent.RemoteEventCounts += OnDbEvent;
            }
        }

        private void OnDbEvent(object sender, FbRemoteEventCountsEventArgs e)
        {
            Logger.LogInformation($"New event: {e.Name}");
            var tableType = _triggersStorage.ToTable(e.Name);

            switch (tableType)
            {
                case TableType.ProcessEvents:
                    OnProcessEventsChanged(e.Name);
                    break;
                default:
                    throw new Exception($"Table: {tableType} not supported");
            }
        }

        private void OnProcessEventsChanged(string eventName)
        {
            var models = _tempDbReader.ReadModels<ProcessEventsActionModel>();
            var args = new ProcessEventsChangedEventArgs { Models = models };
            ProcessEventsChanged?.Invoke(this, args);
        }

        private IEnumerable<TriggerMetaData> EnsureTriggersExists(TableType table)
        {
            using (Logger.BeginScope(LoggerHelper.GetCaller()))
            {
                var result = new List<TriggerMetaData>();

                using (var connection = new FbConnection(GetConnectionString()))
                {
                    connection.Open();

                    var queryList = WatcherResourceManager.GetSqlTriggerQueries(table);

                    foreach (var query in queryList)
                    {
                        using (var command = new FbCommand(query, connection))
                        {
                            command.ExecuteNonQuery(OnAddTriggerException);
                        }

                        var triggerMetaData = new TriggerMetaData
                        {
                            Name = TriggerExtractor.ExtractTriggerName(query),
                            CreationTime = DateTime.Now,
                            EventName = TriggerExtractor.ExtractEventName(query),
                            Type = TriggerExtractor.ExtractTriggerType(query),
                            TableType = table,
                        };

                        Logger.LogDebug($"Trigger created: {triggerMetaData.Name}");
                        result.Add(triggerMetaData);
                    }

                    connection.Close();
                }

                return result;
            }
        }

        private static void OnAddTriggerException(Exception ex)
        {
            var fbEx = (FbException)ex;

            //// mean trigger already exists
            //if (fbEx.ErrorCode == 335544351)
            //    return;

            throw ex;
        }

        private string GetConnectionString()
        {
            var connectionString = WatcherResourceManager.MainDbConnectionStringSql;
            return connectionString;
        }

        public void Dispose()
        {
            _remoteEvent?.Dispose();
        }
    }
}
