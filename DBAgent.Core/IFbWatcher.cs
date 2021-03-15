using System;
using System.Collections.Generic;
using DBAgent.Watcher.Entities;
using DbAgent.Watcher.Events.Handlers;
using DbAgent.Watcher.Models;
using DbAgent.Watcher.Scheme;

namespace DbAgent.Watcher
{
    public interface IFbWatcher<TModel>:IDisposable where TModel : IModel, new()
    {
        event TableChangedEventHandler<TModel> TableChanged;

        TriggerMetaData AddTrigger(SqlTriggerScheme<TModel> scheme);
        IEnumerable<TriggerMetaData> AddTriggers(IEnumerable<SqlTriggerScheme<TModel>> schemes);

        void EnsureAllTriggersRemoved();

        void EnsureTriggerRemoved(TriggerMetaData trigger);

        void StartListening();
    }
}
