using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DbAgent.Service.Architecture;
using Melnik.Logging;
using Microsoft.Extensions.Logging;

namespace DbAgent.Service
{
    public class DbAgentService
    {
        private static DbAgentService _main;
        public static DbAgentService Main => _main ?? (_main = new DbAgentService());

        private AppConfig _config;

        public DbAgentService()
        {
            Logger = LogSettings.LoggerFactory.CreateLogger<DbAgentService>();
        }

        protected ILogger Logger { get; }

        public async Task InitializeAsync()
        {
            using (Logger.BeginScope(LoggerHelper.GetCaller()))
            {
                InitializeAppConfig();
            }
        }

        public async Task WorkingAsync()
        {

        }

        private void InitializeAppConfig()
        {
            using (Logger.BeginScope(LoggerHelper.GetCaller()))
            {
                var configPath = AppSettings.AppConfigPath;

                try
                {
                    _config = AppConfig.FromFile(AppSettings.AppConfigPath);
                    Logger.LogDebug($"{configPath} loaded");
                }
                catch
                {
                    var defaultConfig = AppConfig.GetDefault(AppSettings.DefaultAppConfigPath);
                    defaultConfig.Save();
                    Logger.LogError($"Can't load {configPath}, default config created");
                    throw;
                }
            }
        }
    }
}
