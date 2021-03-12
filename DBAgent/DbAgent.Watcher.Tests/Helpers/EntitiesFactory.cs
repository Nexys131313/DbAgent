using DbAgent.Service.FireBird;
using DBAgent.Watcher;
using DbAgent.Watcher.Models;
using DbAgent.Watcher.Scheme;

namespace DbAgent.Watcher.Tests.Helpers
{
    internal class EntitiesFactory
    {
        public IFbWatcher<TModel> CreateWatcher<TModel>(string triggersFilePath)
            where TModel : IModel, new()
        {
            var factory = new WatcherFactory<TModel>();
            var watcherOptions = new FbSqlWatcherOptions(AppContext.MainDbConnectionString,
                AppContext.TempDbConnectionString, triggersFilePath);
            return factory.CreateWatcher(watcherOptions);

        }

        public FbSqlExecuter CreateMainDbSqlExecuter()
        {
            return new FbSqlExecuter(AppContext.MainDbConnectionString);
        }

        public ISchemeFactory<TModel> CreateSchemeFactory<TModel>()
            where TModel: IModel, new()
        {
            return new SchemeFactory<TModel>(AppContext.ExternalDbSource,
                AppContext.ExternalUser, AppContext.ExternalPassword);
        }
    }
}
