using System;
using System.Collections.Generic;
using DBAgent.Watcher.Enums;

namespace DBAgent.Watcher.Helpers
{
    public static class TriggerTypeConverter
    {
        private static readonly Dictionary<TriggerType, string> Type2Name 
            = new Dictionary<TriggerType, string>()
        {
            {TriggerType.Delete, "DELETE"},
            {TriggerType.Insert, "INSERT"},
            {TriggerType.Update, "UPDATE"},
        };

        public static string ToName(TriggerType triggerType)
        {
            if (Type2Name.TryGetValue(triggerType, out var result))
                return result;

            throw new NotSupportedException($"Can't convert {triggerType} to name");
        }

        public static TriggerType ToType(string name)
        {
            foreach (var item in Type2Name)
                if (item.Value.ToLower() == name.ToLower())
                    return item.Key;

            throw new NotSupportedException($"Can't convert {name} to Type");
        }

    }
}
