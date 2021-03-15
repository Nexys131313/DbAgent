using System;
using DbAgent.Watcher.Models;

namespace DbAgent.Redis
{
    public interface IRedisClient<in TModel> where TModel: IModel, new()
    {
        bool TrySendModel(TModel model, out Exception ex);
    }
}
