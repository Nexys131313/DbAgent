using System;
using System.Linq;
using System.Threading.Tasks;
using DbAgent.Service.FireBird;
using DbAgent.Tests.Core;
using DbAgent.Watcher.Models;

namespace DbAgent.Service.Tests.Helpers
{
    public class DbAgentServiceTestEngine<TModel>: IDisposable where TModel : IModel, new()
    {
        private readonly ModelsCache<TModel> _modelsCache
           = new ModelsCache<TModel>();

        private readonly DbAgentService<TModel> _service;

        public DbAgentServiceTestEngine()
        {
            var factory = new DbAgentServiceFactory();
            _service = factory.CreateService<TModel>();

        }

        public async Task<TModel> CaptureModel(string insertCommand, Func<TModel, bool> findModelFunc)
        {
            _service.ModelsSentToRedis += OnModelsSent;
            _service.Start();

            var sqlExecuter = new FbSqlExecuter(TestsContext.MainDbConnectionString);
            sqlExecuter.ExecuteNonQuery(insertCommand);

            var model = await WaitForModel(TimeSpan.FromSeconds(10), findModelFunc);
            return model;
        }

        private void OnModelsSent(object sender, Events.Args.ModelsUpdatedEventArgs<TModel> args)
        {
            _modelsCache.SetModels(args.Models);
        }

        public void Dispose()
        {
            _service.Dispose();
        }

        private async Task<TModel> WaitForModel(TimeSpan waitForTime, Func<TModel, bool> findModelFunc)
        {
            var startTime = DateTime.Now;

            while (true)
            {
                var models = _modelsCache.Models;

                var local = models.FirstOrDefault(findModelFunc);
                if (local != null)
                    return local;

                if (DateTime.Now > startTime + waitForTime)
                    throw new Exception($"Time out");

                await Task.Delay(100);
            }
        }

    }
}
