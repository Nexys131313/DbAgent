using System;
using System.Reflection;
using DbAgent.Redis;
using DbAgent.Service.FireBird;
using DbAgent.Tests.Core;
using DbAgent.Watcher;
using DBAgent.Watcher;
using DbAgent.Watcher.Attributes;
using DBAgent.Watcher.Enums;
using DbAgent.Watcher.Models;
using DbAgent.Watcher.Scheme;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DbAgent.Service.Tests.Helpers
{
    internal class DbAgentServiceFactory
    {
        public DbAgentService<TModel> CreateService<TModel>()
            where TModel : IModel, new()
        {

            var watcher = CreateWatcher<TModel>();
            var redis = CreateRedisClient<TModel>();
            var sqlExecuter = CreateSqlExecuter();
            var logger = new NullLoggerFactory().CreateLogger<DbAgentService<TModel>>();

            var service = new DbAgentService<TModel>(watcher, redis, logger, sqlExecuter);
            return service;
        }

        private static IFbSqlExecuter CreateSqlExecuter()
        {
            return new FbSqlExecuter(TestsContext.TempDbConnectionString);
        }

        private static IRedisClient<TModel> CreateRedisClient<TModel>()
            where TModel : IModel, new()
        {
            var redisConnectionString = TestsContext.RedisConnectionString;
            var mainDbId = TestsContext.MainDbId;

            var factory = new RedisClientFactory<TModel>(redisConnectionString, mainDbId);

            return factory.CreateRedisClient();
        }

        private static IFbWatcher<TModel> CreateWatcher<TModel>()
            where TModel : IModel, new()
        {
            var tableName = typeof(TModel).GetCustomAttribute<DbTransferInfo>()?.MainTableName;
            if (string.IsNullOrWhiteSpace(tableName))
                throw new Exception($"Can't extract table name from: {typeof(TModel)}");

            var options = new FbSqlWatcherOptions(TestsContext.MainDbConnectionString,
                TestsContext.TempDbConnectionString);

            var factory = CreateWatcherFactory<TModel>();
            var watcher = factory.CreateWatcher(options);

            var schemeFactory = new SchemeFactory<TModel>(TestsContext.ExternalDbSource,
                TestsContext.ExternalUser, TestsContext.ExternalPassword);

            watcher.AddTriggers(schemeFactory.CreateSchemes(TriggerType.Insert,
                TriggerType.Delete, TriggerType.Update));

            return watcher;
        }

        private static IWatcherFactory<TModel> CreateWatcherFactory<TModel>()
            where TModel : IModel, new()
        {
            var factory = new WatcherFactory<TModel>();
            return factory;
        }
        
    }
}
