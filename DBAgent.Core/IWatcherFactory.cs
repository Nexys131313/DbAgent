using DBAgent.Watcher;
using DbAgent.Watcher.Core;
using DbAgent.Watcher.Models;

namespace DbAgent.Watcher
{
    public interface IWatcherFactory<TModel> where TModel : IModel, new()
    {
        IFbWatcher<TModel> CreateWatcher(WatcherOptions options);
    }
}
