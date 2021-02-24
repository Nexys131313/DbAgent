using System;
using DBAgent.Watcher.Enums;
using Newtonsoft.Json;

namespace DbAgent.Watcher.Helpers
{
    public class SqlTriggerScheme<TModel> 
    {
        public string TriggerName { get; set; }
        public string TableName { get; set; }
        public TriggerType TriggerType { get; set; }
        public string EventName { get; set; }

        public string ExternalTableName { get; set; }
        public string ExternalDataSource { get; set; }
        public string ExternalUser { get; set; }
        public string ExternalPassword { get; set; }

        [JsonIgnore]
        public Type ModelType => typeof(TModel);
    }
}
