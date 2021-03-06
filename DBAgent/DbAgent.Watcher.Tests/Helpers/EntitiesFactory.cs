﻿using DbAgent.Service.FireBird;
using DbAgent.Watcher.Models;
using DbAgent.Watcher.Scheme;
using DbAgent.Tests.Core;
using DbAgent.Watcher.Core;

namespace DbAgent.Watcher.Tests.Helpers
{
    internal class EntitiesFactory
    {
        public IFbWatcher<TModel> CreateWatcher<TModel>()
            where TModel : IModel, new()
        {
            var factory = new WatcherFactory<TModel>();
            var watcherOptions = new WatcherOptions(TestsContext.MainDbConnectionString,
                TestsContext.TempDbConnectionString);
            return factory.CreateWatcher(watcherOptions);

        }

        public FbSqlExecuter CreateMainDbSqlExecuter()
        {
            return new FbSqlExecuter(TestsContext.MainDbConnectionString);
        }

        public ISchemeFactory<TModel> CreateSchemeFactory<TModel>()
            where TModel: IModel, new()
        {
            return new SchemeFactory<TModel>(TestsContext.ExternalDbSource,
                TestsContext.ExternalUser, TestsContext.ExternalPassword);
        }
    }
}
