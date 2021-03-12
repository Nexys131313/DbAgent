using DbAgent.Service.Events.Args;
using DbAgent.Watcher.Models;

namespace DbAgent.Service.Events.Handlers
{
    public delegate void ModelsUpdatedEventHandler<TModel>(object sender,
        ModelsUpdatedEventArgs<TModel> args) where TModel : IModel, new();
}
