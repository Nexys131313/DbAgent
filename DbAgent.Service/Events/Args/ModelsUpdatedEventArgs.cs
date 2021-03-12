using System;
using System.Collections.Generic;
using DbAgent.Watcher.Models;

namespace DbAgent.Service.Events.Args
{
    [Serializable]
    public class ModelsUpdatedEventArgs<TModel> where TModel: IModel, new()
    {
        public List<TModel> Models { get; internal set; } = new List<TModel>();
    }
}
