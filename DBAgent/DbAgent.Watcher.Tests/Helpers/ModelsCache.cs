using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DbAgent.Watcher.Models;

namespace DbAgent.Watcher.Tests.Helpers
{
    public class ModelsCache<TModel> where TModel : IModel, new()
    {
        private readonly object _modelsAccessKey = new object();
        private List<TModel> _models = new List<TModel>();

        public IReadOnlyCollection<TModel> Models
        {
            get
            {
                lock (_modelsAccessKey)
                {
                    return _models.ToArray();
                }
            }
        }

        public void AddModel(TModel model)
        {
            lock (_modelsAccessKey)
            {
                _models.Add(model);
            }
        }

        public void SetModels(List<TModel> models)
        {
            lock (_modelsAccessKey)
            {
                _models = models;
            }
        }
    }
}
