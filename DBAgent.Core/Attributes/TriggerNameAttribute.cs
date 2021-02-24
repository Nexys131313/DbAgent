using System;
using DBAgent.Watcher.Enums;

namespace DbAgent.Watcher.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class TriggerNameAttribute: Attribute
    {
        public TriggerNameAttribute(TriggerType triggerType, string triggerName)
        {
            TriggerType = triggerType;
            TriggerName = triggerName;
        }

        public TriggerType TriggerType { get; }
        public string TriggerName { get; }
    }
}
