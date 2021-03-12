using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
