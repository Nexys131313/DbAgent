using System;
using System.Collections.Generic;
using System.Text;
using DbAgent.Watcher.Models;

namespace DbAgent.Redis
{
    public interface IRedisFactory<in TModel> where TModel: IModel, new()
    {
        IRedisClient<TModel> CreateRedisClient();
    }
}
