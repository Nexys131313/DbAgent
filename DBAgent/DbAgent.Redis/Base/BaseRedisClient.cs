using StackExchange.Redis;

namespace DbAgent.Redis.Base
{
    public class BaseRedisClient
    {
        public IDatabase GetDataBase()
        {
            return RedisConnectorHelper.Connection.GetDatabase();
        }

        public void SaveData(string key, string value)
        {
            var cache = RedisConnectorHelper.Connection.GetDatabase();
            cache.StringSet(key, value);
        }

        public string GetValue(string key)
        {
            var cache = RedisConnectorHelper.Connection.GetDatabase();
            return cache.StringGet(key);
        }

        public void Delete(string key)
        {
            var cache = RedisConnectorHelper.Connection.GetDatabase();
            cache.KeyDelete(key);
        }

        //public void Test()
        //{
        //    var cache = RedisConnectorHelper.Connection.GetDatabase();
        //    cache.valu
        //}
    }
}
