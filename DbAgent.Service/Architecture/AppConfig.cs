using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DbAgent.Service.Architecture
{
    [Serializable]
    public class AppConfig
    {
        [JsonIgnore]
        private string _filePath;

        private AppConfig()
        {

        }

       
        public static AppConfig GetDefault(string filePath)
        {
            var config = new AppConfig { _filePath = filePath };
            config.Save();
            return config;
        }

        public static AppConfig FromFile(string filePath)
        {
            var jsonStr = File.ReadAllText(filePath);
            var config = JsonConvert.DeserializeObject<AppConfig>(jsonStr);
            config._filePath = filePath;
            config.Save();
            return config;
        }

        public void Save()
        {
            File.WriteAllText(_filePath, JObject.FromObject(this).ToString());
        }
    }
}
