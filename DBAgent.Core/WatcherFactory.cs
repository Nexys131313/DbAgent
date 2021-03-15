using DBAgent.Watcher;
using DbAgent.Watcher.Core;
using DbAgent.Watcher.Models;
using DbAgent.Watcher.Scheme;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DbAgent.Watcher
{
    public class WatcherFactory<TModel> : IWatcherFactory<TModel> where TModel : IModel, new()
    {
        private readonly ISqlBuilder _sqlBuilder;
        private readonly ILoggerFactory _loggerFactory;

        public WatcherFactory()
        {
            _sqlBuilder = new DefaultSqlBuilder();
            _loggerFactory = new NullLoggerFactory();
        }

        public WatcherFactory(ILoggerFactory loggerFactory)
        {
            _sqlBuilder = new DefaultSqlBuilder();
            _loggerFactory = loggerFactory;
        }

        public WatcherFactory(ISqlBuilder sqlBuilder, ILoggerFactory loggerFactory)
        {
            _sqlBuilder = sqlBuilder;
            _loggerFactory = loggerFactory;
        }

        public IFbWatcher<TModel> CreateWatcher(WatcherOptions options)
        {
            var logger = _loggerFactory.CreateLogger<FbSqlWatcher<TModel>>();
            return new FbSqlWatcher<TModel>(options, _sqlBuilder, logger);
        }
    }
}
