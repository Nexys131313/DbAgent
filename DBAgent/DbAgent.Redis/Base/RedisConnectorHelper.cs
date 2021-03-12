using System;
using StackExchange.Redis;

namespace DbAgent.Redis.Base
{
    internal class RedisConnectorHelper
    {
        static RedisConnectorHelper()
        {
            LazyConnection = new Lazy<ConnectionMultiplexer>(
                () => ConnectionMultiplexer.Connect("10.44.127.201:6379, password=PnRD0BiAFj8F%7P$09G9"));
        }

        private static readonly Lazy<ConnectionMultiplexer> LazyConnection;

        public static ConnectionMultiplexer Connection => LazyConnection.Value;
    }
}
