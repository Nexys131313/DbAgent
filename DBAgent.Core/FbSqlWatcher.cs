using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DbAgent.Watcher.Attributes;
using DBAgent.Watcher.Entities;
using DbAgent.Watcher.Events.Args;
using DbAgent.Watcher.Events.Handlers;
using DbAgent.Watcher.Helpers;
using DBAgent.Watcher.Helpers;
using DbAgent.Watcher.Models;
using DBAgent.Watcher.Models;
using DBAgent.Watcher.Readers;
using FirebirdSql.Data.FirebirdClient;
using Melnik.Logging;
using Microsoft.Extensions.Logging;

namespace DBAgent.Watcher
{
    public class FbSqlWatcher<TModel> : IDisposable where TModel : IModel, new()
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
            Logger = new LoggerFactory().CreateLogger<FbSqlWatcher<ProcessEventsActionModel>>();
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
        public string ConnectionString => WatcherResourceManager.MainDbConnectionStringSql;
        protected ILogger Logger { get; private set; }

        public event TableChangedEventHandler<TModel> TableChanged;

        public TriggerMetaData AddTrigger(SqlTriggerScheme<TModel> scheme)
        {
            var sqlQuery = SqlTriggerBuilder.BuildSqlTrigger(scheme);
            ExecuteNonQuery(sqlQuery);

            var triggerMetaData = new TriggerMetaData
            {
                Name = scheme.TriggerName,
                CreationTime = DateTime.Now,
                EventName = scheme.EventName,
                Type = scheme.TriggerType
            };

            _triggersStorage.AddTriggerIfNonExists(triggerMetaData);
            return triggerMetaData;
        }

        public void EnsureAllTriggersRemoved()
        {
            foreach (var triggerMetaData in CurrentTriggers)
                EnsureTriggerRemoved(triggerMetaData);
        }

        public void EnsureTriggerRemoved(TriggerMetaData trigger)
        {
            using (Logger.BeginScope(LoggerHelper.GetCaller()))
            {
                var triggerName = trigger.Name;
                var cmdQuery = $"DROP TRIGGER {triggerName}";

                ExecuteNonQuery(cmdQuery, (ex) =>
                {
                    var fbEx = (FbException)ex;
                    if (fbEx.ErrorCode == 335544351) // trigger already deleted
                        Logger.LogWarning($"Trigger already removed: {trigger.Name}");
                    else
                        throw ex;

                });

                _triggersStorage.RemoveTriggerIfExists(trigger);
            }
        }

        //todo remove, for test only
        public void InsertRandomToProcessEvents()
        {
            using (Logger.BeginScope(LoggerHelper.GetCaller()))
            {
                var rnd = new Random();
                var id = rnd.Next(1, 1000000);
                var cmdQuery = $"INSERT INTO PROCESS_EVENTS (ID) VALUES ({id});";
                ExecuteNonQuery(cmdQuery);
            }
        }

        public void InitializeListeners()
        {
            using (Logger.BeginScope(LoggerHelper.GetCaller()))
            {
                _remoteEvent?.Dispose();

                _remoteEvent = new FbRemoteEvent(ConnectionString);

                var triggers = _triggersStorage.Triggers;
                var events = triggers.Select(item => item.EventName);
                _remoteEvent.QueueEvents(events.ToArray());
                _remoteEvent.RemoteEventCounts += OnDbEvent;
            }
        }

        private void OnDbEvent(object sender, FbRemoteEventCountsEventArgs eventArgs)
        {
            Logger.LogInformation($"New event: {eventArgs.Name}");

            var model = GetModelOrNull(eventArgs.Name);
            if (model == null)
            {
                Logger.LogWarning($"Can't find model for event: {eventArgs.Name}");
                return;
            }

            var models = _tempDbReader.ReadModels<TModel>();
            var args = new TableChangedEventArgs<TModel>()
            {
                ChangedModels = models
            };

            TableChanged?.Invoke(this, args);
        }

        private static TModel GetModelOrNull(string eventName)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<TransferInfo>();
                if (attribute == null) continue;

                var eventNameAttrs = type.GetCustomAttributes<EventNameAttribute>();
                var eventNames = eventNameAttrs.Select(item => item.EventName);

                if (!eventNames.Contains(eventName)) continue;

                var instance = Activator.CreateInstance(type);
                return (TModel)instance;
            }

            return default;
        }

        private void ExecuteNonQuery(string cmdQuery)
        {
            using (var connection = new FbConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new FbCommand(cmdQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        private void ExecuteNonQuery(string cmdQuery, Action<Exception> onError)
        {
            try
            {
                ExecuteNonQuery(cmdQuery);
            }
            catch (Exception ex)
            {
                onError.Invoke(ex);
            }
        }

        public void Dispose()
        {
            _remoteEvent?.Dispose();
        }
    }
}
