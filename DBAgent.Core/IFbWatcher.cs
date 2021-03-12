using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBAgent.Watcher.Entities;
using DbAgent.Watcher.Events.Handlers;
using DbAgent.Watcher.Models;
using DbAgent.Watcher.Scheme;
using FirebirdSql.Data.FirebirdClient;
using Melnik.Logging;
using Microsoft.Extensions.Logging;

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
