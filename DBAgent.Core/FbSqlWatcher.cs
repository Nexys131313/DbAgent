using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DbAgent.Watcher;
using DbAgent.Watcher.Attributes;
using DbAgent.Watcher.Core;
using DBAgent.Watcher.Entities;
using DbAgent.Watcher.Events.Args;
using DbAgent.Watcher.Events.Handlers;
using DbAgent.Watcher.Models;
using DBAgent.Watcher.Models;
using DBAgent.Watcher.Readers;
using DbAgent.Watcher.Scheme;
using FirebirdSql.Data.FirebirdClient;
using Melnik.Logging;
using Microsoft.Extensions.Logging;

namespace DBAgent.Watcher
{
    internal class FbSqlWatcher<TModel> : IFbWatcher<TModel> where TModel : IModel, new()
    {
        private readonly List<TriggerMetaData> _triggers = new List<TriggerMetaData>();
        private WatcherOptions _options;
        private FbSqlReader _tempDbReader;
        private FbRemoteEvent _remoteEvent;

        public FbSqlWatcher(WatcherOptions options, ISqlBuilder sqlBuilder)
        {
            InitializeInternal(options);
            SqlBuilder = sqlBuilder;
        }

        public FbSqlWatcher(WatcherOptions options, ISqlBuilder sqlBuilder, ILogger logger)
        {
            InitializeInternal(options);
            SqlBuilder = sqlBuilder;
            Logger = logger;
        }

        public IEnumerable<TriggerMetaData> CurrentTriggers => _triggers.ToArray();
        public string ConnectionString => _options.MainDbConnectionString;

        protected ISqlBuilder SqlBuilder { get; private set; }
        protected ILogger Logger { get; private set; }

        public event TableChangedEventHandler<TModel> TableChanged;

        public TriggerMetaData AddTrigger(SqlTriggerScheme<TModel> scheme)
        {
            var sqlQuery = SqlBuilder.BuildSqlTrigger(scheme);
            ExecuteNonQuery(sqlQuery);

            var triggerMetaData = new TriggerMetaData
            {
                Name = scheme.TriggerName,
                CreationTime = DateTime.Now,
                EventName = scheme.EventName,
                Type = scheme.TriggerType
            };

            var local = _triggers.FirstOrDefault(item => item.Id == triggerMetaData.Id);
            if (local != null)
                throw new Exception($"Trigger with Id: {local.Id} is already exists");

            _triggers.Add(triggerMetaData);
            return triggerMetaData;
        }

        public IEnumerable<TriggerMetaData> AddTriggers(IEnumerable<SqlTriggerScheme<TModel>> schemes)
        {
            return schemes.Select(AddTrigger).ToList();
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
            }
        }

        public void StartListening()
        {
            using (Logger.BeginScope(LoggerHelper.GetCaller()))
            {
                _remoteEvent?.Dispose();

                _remoteEvent = new FbRemoteEvent(ConnectionString);

                var events = _triggers.Select(item => item.EventName).ToList();
                _remoteEvent.QueueEvents(events.ToArray());
                _remoteEvent.RemoteEventCounts += OnDbEvent;

                var eventsStr = events.Aggregate("", (current, item) => current + $"{item} ");
                Logger.LogDebug($"Listening events: {eventsStr}");
            }
        }

        private void InitializeInternal(WatcherOptions options)
        {
            _options = options;
            _tempDbReader = new FbSqlReader(options.TempDbConnectionString);
            Logger = new LoggerFactory().CreateLogger<FbSqlWatcher<ProcessEventsActionModel>>();
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
                TotalChangedModels = models
            };

            TableChanged?.Invoke(this, args);
        }

        private static TModel GetModelOrNull(string eventName)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<DbTransferInfo>();
                if (attribute == null) continue;

                var eventNameAttrs = type.GetCustomAttributes<DbEventAttribute>();
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
            EnsureAllTriggersRemoved();
        }
    }
}
