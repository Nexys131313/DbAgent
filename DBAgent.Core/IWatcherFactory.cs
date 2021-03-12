using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBAgent.Watcher;
using DbAgent.Watcher.Models;

namespace DbAgent.Watcher
{
    public interface IWatcherFactory<TModel> where TModel : IModel, new()
    {
        IFbWatcher<TModel> CreateWatcher(FbSqlWatcherOptions options);
    }
}
