using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DbAgent.Client.Common;
using DbAgent.Client.Helpers;
using DbAgent.Service;
using DbAgent.Watcher;
using DBAgent.Watcher;
using DbAgent.Watcher.Attributes;
using DbAgent.Watcher.Models;
using DBAgent.Watcher.Models;
using Microsoft.Extensions.Logging;

namespace DbAgent.Client
{
    public class Program
    {
        private static readonly List<IDisposable> _services = new List<IDisposable>();

        public static void Main(string[] args)
        {
            Logger = LogSettings.LoggerFactory.CreateLogger<Program>();
            Logger.LogDebug("Logger initialized");
            AppConfig.Initialize();
            Logger.LogDebug("App config initialized");

            try
            {
                Start();

                while (true)
                {
                    Console.ReadLine();
                }
            }
            catch(Exception ex)
            {
                Logger.LogError(ex.ToString());
                foreach (var service in _services)
                {
                    service.Dispose();
                }

                Logger.LogWarning($"All services disposed");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        protected static ILogger Logger { get; private set; }

        private static void Start()
        {
            StartService<ProcessEventsActionModel>();
        }

        private static void StartService<TModel>()
            where TModel : IModel, new()
        {
            var serviceFactory = new DbAgentServiceFactory();
            var service = serviceFactory.CreateService<TModel>();
            _services.Add(service);
            service.Start();
            Logger.LogDebug($"{typeof(TModel)} service started");
        }
    }
}
