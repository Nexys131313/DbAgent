using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbAgent.Watcher.Models;

namespace DbAgent.Watcher.Events.Args
{
    [Serializable]
    public class TableChangedEventArgs<TModel>
        where TModel: IModel, new()
    {
        public List<TModel> ChangedModels { get; set; } = new List<TModel>();
    }
}
