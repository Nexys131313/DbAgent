using System;
using System.Collections.Generic;
using System.Text;
using DbAgent.Watcher.Models;

namespace DbAgent.Manage.Events.Args
{
    [Serializable]
    public class ModelsUpdatedEventArgs<TModel> where TModel: IModel, new()
    {
        public List<TModel> Models { get; internal set; } = new List<TModel>();
    }
}
