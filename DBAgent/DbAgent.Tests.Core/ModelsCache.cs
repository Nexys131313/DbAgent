using System.Collections.Generic;
using DbAgent.Watcher.Models;

namespace DbAgent.Tests.Core
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
