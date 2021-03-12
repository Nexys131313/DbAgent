using System;
using System.Linq;
using System.Threading.Tasks;
using DBAgent.Watcher;
using DBAgent.Watcher.Enums;
using DbAgent.Watcher.Models;
using DbAgent.Watcher.Scheme;

namespace DbAgent.Watcher.Tests.Helpers
{
    public class ModelTestEngine<TModel>: IDisposable where TModel: IModel, new()
    {
        private readonly ModelsCache<TModel> _modelsCache
            = new ModelsCache<TModel>();

        private readonly IFbWatcher<TModel> _watcher;
        private readonly ISchemeFactory<TModel> _schemeFactory;
        private readonly EntitiesFactory _entitiesFactory;

        public ModelTestEngine()
        {
            _entitiesFactory = new EntitiesFactory();
            _watcher = _entitiesFactory.CreateWatcher<TModel>();
            _watcher.EnsureAllTriggersRemoved();
            _schemeFactory = _entitiesFactory.CreateSchemeFactory<TModel>();

        }

        public async Task<TModel> CaptureModel(string insertCommand, Func<TModel, bool> findModelFunc)
        {
            var schemes = _schemeFactory.CreateSchemes(
                TriggerType.Insert, TriggerType.Delete, TriggerType.Update);

            _watcher.AddTriggers(schemes);
            _watcher.TableChanged += OnTableChanged;
            _watcher.StartListening();

            var sqlExecuter = _entitiesFactory.CreateMainDbSqlExecuter();
            sqlExecuter.ExecuteNonQuery(insertCommand);
            
            var model = await WaitForModel(TimeSpan.FromSeconds(10), findModelFunc);
            return model;
        }

        public void Dispose()
        {
            _watcher.EnsureAllTriggersRemoved();
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

        private void OnTableChanged(object sender, Events.Args.TableChangedEventArgs<TModel> args)
        {
            _modelsCache.SetModels(args.TotalChangedModels);
        }
    }
}
