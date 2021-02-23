using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using DBAgent.Watcher.Entities;
using DBAgent.Watcher.Enums;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DBAgent.Watcher
{
    public class TriggersStorage
    {
        [JsonProperty]
        private List<TriggerMetaData> _triggers = new List<TriggerMetaData>();

        public TriggersStorage(string filePath, bool isAutoSave)
        {
            FilePath = filePath;
            IsAutoSave = isAutoSave;
            Logger = new LoggerFactory().CreateLogger<TriggersStorage>();
        }

        public TriggersStorage(string filePath, bool isAutoSave, ILogger logger)
        {
            FilePath = filePath;
            IsAutoSave = isAutoSave;
            Logger = logger;
        }

        [JsonIgnore]
        public IEnumerable<TriggerMetaData> Triggers => _triggers.ToArray();
        public string FilePath { get; private set; }
        public bool IsAutoSave { get; set; }

        [JsonIgnore]
        protected ILogger Logger { get; }

        public static bool TryLoadFromFile(string filePath, out TriggersStorage storage)
        {
            try
            {
                storage = LoadFromFile(filePath);
                return true;
            }
            catch
            {
                storage = null;
                return false;
            }

        }

        public static TriggersStorage LoadFromFile(string filePath)
        {
            var jsonStr = File.ReadAllText(filePath);
            var storage = JsonConvert.DeserializeObject<TriggersStorage>(jsonStr);
            return storage;
        }

        public TableType ToTable(string eventName)
        {
            var trigger = _triggers.FirstOrDefault(item => item.EventName == eventName);
            if (trigger == null)
                throw new Exception($"Can't find event: {eventName}");

            return trigger.TableType;
        }

        public bool IsContains(TriggerMetaData trigger)
        {
            var local = _triggers.FirstOrDefault(item => item.Id == trigger.Id);
            return local != null;
        }

        public void AddTrigger(TriggerMetaData trigger)
        {
            if (IsContains(trigger))
                throw new Exception("Trigger already exists");

            _triggers.Add(trigger);
            Logger.LogDebug($"Trigger added to storage: {trigger.Name}");

            if (IsAutoSave)
                Save();
        }

        public void RemoveTrigger(TriggerMetaData trigger)
        {
            if (IsContains(trigger) == false)
                throw new Exception("Trigger not found");

            var local = _triggers.First(item => item.Id == trigger.Id);
            _triggers.Remove(local);
            Logger.LogDebug($"Trigger removed from storage: {trigger.Name}");

            if(IsAutoSave)
                Save();
        }

        public void Save()
        {
            var jsonStr = JObject.FromObject(this).ToString();
            File.WriteAllText(FilePath, jsonStr);
            Logger.LogDebug("Triggers saved to file");
        }

        public void AddTriggersFromFileSafe(string filePath)
        {
            if(TryLoadFromFile(filePath, out var storage))
                _triggers.AddRange(storage._triggers);

            if(IsAutoSave)
                Save();
        }
    }
}
