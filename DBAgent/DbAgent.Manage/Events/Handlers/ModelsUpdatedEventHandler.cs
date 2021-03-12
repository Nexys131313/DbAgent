using DbAgent.Manage.Events.Args;
using DbAgent.Watcher.Models;

namespace DbAgent.Manage.Events.Handlers
{
    public delegate void ModelsUpdatedEventHandler<TModel>(object sender,
        ModelsUpdatedEventArgs<TModel> args) where TModel : IModel, new();
}
