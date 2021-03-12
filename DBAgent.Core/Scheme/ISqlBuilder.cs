using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbAgent.Watcher.Models;

namespace DbAgent.Watcher.Scheme
{
    public interface ISqlBuilder
    {
         string BuildSqlTrigger<TModel>(SqlTriggerScheme<TModel> scheme)
             where TModel : IModel, new();
    }
}
