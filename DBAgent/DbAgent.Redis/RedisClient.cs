using System;
using System.Linq;
using System.Reflection;
using DbAgent.Watcher.Attributes;
using DbAgent.Watcher.Models;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace DbAgent.Redis
{
    internal class RedisClient<TModel>: IRedisClient<TModel> where TModel:IModel, new()
    {
        public RedisClient(string connectionString, string mainDbId)
        {
            ConnectionString = connectionString;
            MainDbId = mainDbId;
        }

        public string ConnectionString { get; }
        public string MainDbId { get; }

        private ConnectionMultiplexer OpenConnection()
        {
            var connection = new Lazy<ConnectionMultiplexer>(
                () => ConnectionMultiplexer.Connect(ConnectionString));

            return connection.Value;
        }

        public bool TrySendModel(TModel model, out Exception ex)
        {
            try
            {
                var mainTableName = typeof(TModel).GetCustomAttribute<DbTransferInfo>().MainTableName;
                var key = $"{mainTableName}:{MainDbId}";
                var value = JObject.FromObject(model).ToString();

                using (var connection = OpenConnection())
                {
                    var db = connection.GetDatabase();
                    db.ListRightPush(key, value);
                }

                //EnsureItemExists(key, value);
                ex = null;
                return true;
            }
            catch (Exception exc)
            {
                ex = exc;
                return false;
            }
        }

        private void EnsureItemExists(string key, string value)
        {
            using (var connection = OpenConnection())
            {
                var db = connection.GetDatabase();
                var values = db.ListRange(key, -1, -10).
                    Select(item=> item.ToString());

                if(values.Contains(value)) return;

                throw new Exception($"Can't ensure value is exists");
            }
        }
    }
}
