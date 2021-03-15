using DbAgent.Watcher.Models;

namespace DbAgent.Redis
{
    public interface IRedisFactory<in TModel> where TModel: IModel, new()
    {
        IRedisClient<TModel> CreateRedisClient();
    }
}
