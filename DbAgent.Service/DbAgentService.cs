using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DbAgent.Redis;
using DbAgent.Service.Events.Args;
using DbAgent.Service.Events.Handlers;
using DbAgent.Service.FireBird;
using DbAgent.Watcher;
using DBAgent.Watcher;
using DbAgent.Watcher.Models;
using Melnik.Logging;
using Microsoft.Extensions.Logging;

namespace DbAgent.Service
{
    public class DbAgentService<TModel>: IDisposable where TModel: IModel, new()
    {
        private readonly IFbWatcher<TModel> _watcher;
        private readonly IRedisClient<TModel> _redisClient;
        private readonly IFbSqlExecuter _fbSqlExecuter;

        public DbAgentService(IFbWatcher<TModel> watcher, IRedisClient<TModel> redisClient,
            ILogger logger, IFbSqlExecuter sqlExecuter)
        {
            _watcher = watcher;
            _redisClient = redisClient;
            _fbSqlExecuter = sqlExecuter;
            Logger = logger;
            _watcher.TableChanged += OnTableChanged;
        }

        protected ILogger Logger { get; }

        public ModelsUpdatedEventHandler<TModel> ModelsSentToRedis;

        private void OnTableChanged(object sender, Watcher.Events.Args.TableChangedEventArgs<TModel> args)
        {
            var totalModels = args.TotalChangedModels;
            var sent = new List<TModel>();

            foreach (var model in totalModels)
            {
                if (!_redisClient.TrySendModel(model))
                {
                    Logger.LogWarning($"Can't sent model: UPDATE_ID: {model.UpdateId}");
                    continue;
                }

                Logger.LogDebug($"Model with UPDATE_ID: {model.UpdateId} sent to redis");
                sent.Add(model);

                var tempTable = model.GetTempTableName();
                var idProperty = model.GetDbProperty(nameof(model.UpdateId));

                var command = $"DELETE FROM {tempTable} WHERE {idProperty} = {model.UpdateId}";
                var isOk = _fbSqlExecuter.ExecuteNonQuery(command, (ex) =>
                {
                    Logger.LogWarning(ex.ToString());
                });

                if(isOk)
                    Logger.LogDebug($"Model with UPDATE_ID: {model.UpdateId} deleted from {tempTable}");
            }

            ModelsSentToRedis?.Invoke(this, new ModelsUpdatedEventArgs<TModel>()
            {
                Models = sent,
            });
        }

        public void Start()
        {
            _watcher.StartListening();
            Logger.LogDebug("Started");
        }

        public void Dispose()
        {
            _watcher.Dispose();
        }
    }
}
