using System;
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
        public bool OnlyTempDbField { get; set; }
        public TriggerType? TriggerProperty { get; set; }
    }
}
