using DbAgent.Watcher.Models;

namespace DbAgent.Watcher.Scheme
{
    public interface ISqlBuilder
    {
         string BuildSqlTrigger<TModel>(SqlTriggerScheme<TModel> scheme)
             where TModel : IModel, new();
    }
}
