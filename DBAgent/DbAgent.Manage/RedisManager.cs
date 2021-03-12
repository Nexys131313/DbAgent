using System;
using DbAgent.Manage.Events.Args;
using DbAgent.Manage.Events.Handlers;
using DbAgent.Watcher;
using DBAgent.Watcher;
using DbAgent.Watcher.Models;

namespace DbAgent.Manage
{
    public class RedisManager<TModel> where TModel: IModel, new()
    {
        public RedisManager(IWatcherFactory<TModel> watcherFactory)
        {
            Watcher = watcherFactory.CreateWatcher()
        }

        public IFbWatcher<TModel> Watcher { get; }

        public event ModelsUpdatedEventHandler<TModel> ModelsUpdated;

        public void Initialize()
        {

        }
    }
}
