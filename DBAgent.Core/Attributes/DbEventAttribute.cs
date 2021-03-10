using System;
using DBAgent.Watcher.Enums;

namespace DbAgent.Watcher.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DbEventAttribute : Attribute
    {
        public DbEventAttribute(TriggerType triggerType, string eventName)
        {
            EventName = eventName;
            TriggerType = triggerType;
        }

        public TriggerType TriggerType { get; }
        public string EventName { get; }
    }
}
