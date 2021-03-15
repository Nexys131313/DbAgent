using System.Collections.Generic;
using DBAgent.Watcher.Enums;
using DbAgent.Watcher.Models;

namespace DbAgent.Watcher.Scheme
{
    public interface ISchemeFactory<TModel> where TModel : IModel, new()
    {
        SqlTriggerScheme<TModel> CreateScheme(TriggerType triggerType);

        IEnumerable<SqlTriggerScheme<TModel>> CreateSchemes(params TriggerType[] triggerTypes);
    }
}
