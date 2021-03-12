using System;
using System.Collections.Generic;
using System.Text;
using DbAgent.Watcher.Models;

namespace DbAgent.Redis
{
    public class RedisClientFactory<TModel> : IRedisFactory<TModel> where TModel : IModel, new()
    {
        public RedisClientFactory(string connectionString, string mainDbId)
        {
            ConnectionString = connectionString;
            MainDbId = mainDbId;
        }

        public string ConnectionString { get; }
        public string MainDbId { get;  }

        public IRedisClient<TModel> CreateRedisClient()
        {
            return new RedisClient<TModel>(ConnectionString, MainDbId);
        }
    }
}
