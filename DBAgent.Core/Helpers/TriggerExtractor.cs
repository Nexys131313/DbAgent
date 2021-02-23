using System;
using System.Text.RegularExpressions;
using DBAgent.Watcher.Enums;

namespace DBAgent.Watcher.Helpers
{
    public static class TriggerExtractor
    {
        public static string ExtractTriggerName(string sqlQuery)
        {
            var pattern = @"(\w+)\s+FOR PROCESS_EVENTS";
            var name = Regex.Match(sqlQuery, pattern).Groups[1].Value;

            if (string.IsNullOrWhiteSpace(name))
                throw new Exception($"Can't extract trigger name");

            return name;
        }

        public static string ExtractEventName(string sqlQuery)
        {
            var pattern = @"POST_EVENT\s+'(\w+)'";
            var name = Regex.Match(sqlQuery, pattern).Groups[1].Value;

            if (string.IsNullOrWhiteSpace(name))
                throw new Exception($"Can't extract event name");

            return name;
        }

        public static TriggerType ExtractTriggerType(string sqlQuery)
        {
            var pattern = @"(\w+)\s+POSITION 0";
            var name = Regex.Match(sqlQuery, pattern).Groups[1].Value;

            if (string.IsNullOrWhiteSpace(name))
                throw new Exception($"Can't extract trigger type");

            return TriggerTypeConverter.ToType(name);
        }
    }
}
