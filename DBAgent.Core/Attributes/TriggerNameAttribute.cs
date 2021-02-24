using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
