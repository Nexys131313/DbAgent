using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBAgent.Watcher.Enums;

namespace DbAgent.Watcher.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DbPropertyAttribute: Attribute
    {
        public DbPropertyAttribute(string propertyName)
        {
            PropertyName = propertyName;
            OnlyTempDbField = false;
            TriggerProperty = null;
        }

        public DbPropertyAttribute(string propertyName, bool isOnlyTempDbField)
        {
            PropertyName = propertyName;
            OnlyTempDbField = isOnlyTempDbField;
            TriggerProperty = null;
        }

        public DbPropertyAttribute(string propertyName, bool isOnlyTempDbField, TriggerType triggerPropertyType)
        {
            PropertyName = propertyName;
            OnlyTempDbField = isOnlyTempDbField;
            TriggerProperty = triggerPropertyType;
        }

        public string PropertyName { get; set; }
        public bool OnlyTempDbField { get; set; } = false;
        public TriggerType? TriggerProperty { get; set; }
    }
}
