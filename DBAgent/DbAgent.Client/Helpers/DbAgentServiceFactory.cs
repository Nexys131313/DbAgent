using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using DbAgent.Client.Common;
using DbAgent.Redis;
using DbAgent.Service;
using DbAgent.Service.FireBird;
using DbAgent.Watcher;
using DBAgent.Watcher;
using DbAgent.Watcher.Attributes;
using DBAgent.Watcher.Enums;
using DbAgent.Watcher.Models;
using DbAgent.Watcher.Scheme;
using Microsoft.Extensions.Logging;

namespace DbAgent.Client.Helpers
{
    public class DbAgentServiceFactory
    {
        public  DbAgentService<TModel> CreateService<TModel>()
            where TModel:IModel, new()
        {

            var watcher = CreateWatcher<TModel>();
            var redis = CreateRedisClient<TModel>();
            var sqlExecuter = CreateSqlExecuter();
            var logger = LogSettings.LoggerFactory.CreateLogger<DbAgentService<TModel>>();

            var service = new DbAgentService<TModel>(watcher, redis, logger, sqlExecuter);
            return service;
        }

        private static IFbSqlExecuter CreateSqlExecuter()
        {
            return new FbSqlExecuter(GetTempDbConnectionString());
        }

        private static IRedisClient<TModel> CreateRedisClient<TModel>()
            where TModel:IModel, new()
        {
            var factory = new RedisClientFactory<TModel>(AppConfig.Main.RedisConnectionString,
                AppConfig.Main.MainDbId);

            return factory.CreateRedisClient();
        }

        private static IFbWatcher<TModel> CreateWatcher<TModel>()
            where TModel : IModel, new()
        {
            var tableName = typeof(TModel).GetCustomAttribute<DbTransferInfo>()?.MainTableName;
            if (string.IsNullOrWhiteSpace(tableName))
                throw new Exception($"Can't extract table name from: {typeof(TModel)}");

            var triggersFileName = $"{tableName}_TRIGGERS.json";
            var triggersFilePath = Path.Combine(AppSettings.DataDirectoryPath, triggersFileName);

            var options = new FbSqlWatcherOptions(GetMainDbConnectionString(),
                GetTempDbConnectionString(), triggersFilePath);

            var factory = CreateWatcherFactory<TModel>();
            var watcher = factory.CreateWatcher(options);

            var schemeFactory = new SchemeFactory<TModel>(AppConfig.Main.ExternalDbSource,
                AppConfig.Main.ExternalUser, AppConfig.Main.ExternalPassword);

            watcher.AddTriggers(schemeFactory.CreateSchemes(TriggerType.Insert,
                TriggerType.Delete, TriggerType.Update));

            return factory.CreateWatcher(options);
        }

        private static IWatcherFactory<TModel> CreateWatcherFactory<TModel>()
            where TModel : IModel, new()
        {
            var factory = new WatcherFactory<TModel>(LogSettings.LoggerFactory);
            return factory;
        }

        private static string GetMainDbConnectionString()
        {
            return Properties.Resources.maindb_connectionstring_sql;
        }

        private static string GetTempDbConnectionString()
        {
            return Properties.Resources.tempdb_connectionstring_sql;
        }
    }
}
