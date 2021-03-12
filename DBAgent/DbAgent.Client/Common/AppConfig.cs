using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DbAgent.Client.Common
{
    [Serializable]
    public class AppConfig
    {
        public static AppConfig Main { get; private set; }

        [JsonIgnore]
        private string _filePath;

        private AppConfig()
        {

        }

        public static void Initialize()
        {
            try
            {
                Main = FromFile(AppSettings.AppConfigPath);
            }
            catch
            {
                GetDefault(AppSettings.DefaultAppConfigPath);
            }
        }

        public string RedisConnectionString { get; set; } = "10.44.127.201:6379, password=PnRD0BiAFj8F%7P$09G9";
        public string MainDbId { get; set; } = "MY_DB";
        public string ExternalDbSource { get; set; } = @"C:\DataBases\ACTIONSDB.FDB";
        public string ExternalUser { get; set; } = "SYSDBA";
        public string ExternalPassword { get; set; } = "masterkey";

        public static AppConfig GetDefault(string filePath)
        {
            var config = new AppConfig { _filePath = filePath };
            config.Save();
            return config;
        }

        private static AppConfig FromFile(string filePath)
        {
            var jsonStr = File.ReadAllText(filePath);
            var config = JsonConvert.DeserializeObject<AppConfig>(jsonStr);
            config._filePath = filePath;
            config.EnsureIsValid();
            config.Save();

            return config;
        }

        private void Save()
        {
            File.WriteAllText(_filePath, JObject.FromObject(this).ToString());
        }

        private void EnsureIsValid()
        {
            if (string.IsNullOrWhiteSpace(RedisConnectionString))
                throw new Exception($"{nameof(RedisConnectionString)} can't be null or empty");

            if (string.IsNullOrWhiteSpace(MainDbId))
                throw new Exception($"{nameof(MainDbId)} can't be null or empty");
        }
    }
}
