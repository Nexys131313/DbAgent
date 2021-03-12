using System;
using System.Collections.Generic;
using DbAgent.Watcher.Models;

namespace DbAgent.Watcher.Events.Args
{
    [Serializable]
    public class TableChangedEventArgs<TModel>
        where TModel: IModel, new()
    {
        public List<TModel> TotalChangedModels { get; set; } = new List<TModel>();
    }
}
