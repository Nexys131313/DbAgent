using System;
using DBAgent.Watcher.Enums;
using Newtonsoft.Json;

namespace DBAgent.Watcher.Entities
{
    [Serializable]
    public class TriggerMetaData
    {
        [JsonIgnore]
        public string Id => Name;
        public string Name { get; set; }
        public DateTime CreationTime { get; set; }
        public string EventName { get; set; }
        public TriggerType Type { get; set; }
    }
}
