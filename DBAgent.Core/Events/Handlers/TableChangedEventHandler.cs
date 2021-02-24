using DbAgent.Watcher.Events.Args;
using DbAgent.Watcher.Models;

namespace DbAgent.Watcher.Events.Handlers
{
    public delegate void TableChangedEventHandler<TModel>(object sender, TableChangedEventArgs<TModel> args)
        where TModel : IModel, new();
}
