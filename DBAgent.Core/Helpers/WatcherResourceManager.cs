using System;
using System.Collections.Generic;
using System.IO;
using DBAgent.Watcher.Enums;
using DbAgent.Watcher.Properties;

namespace DBAgent.Watcher.Helpers
{
    public static class WatcherResourceManager
    {
        public static string MainDbConnectionStringSql
            => GetContent(nameof(Resources.MainDb_ConnectionString_SQL));

        public static string TempDbConnectionStringSql
            => GetContent(nameof(Resources.TempDb_ConnectionString_SQL));

        public static string ProcessEventsDeleteTriggerSql
            => GetContent(nameof(Resources.Process_Events_Delete_Trigger_SQL));
        public static string ProcessEventsUpdateTriggerSql
            => GetContent(nameof(Resources.Process_Events_Update_Trigger_SQL));
        public static string ProcessEventsInsertTriggerSql
            => GetContent(nameof(Resources.Process_Events_Insert_Trigger_SQL));

        public static IEnumerable<string> GetSqlTriggerQueries(TableType tableType)
        {
            switch (tableType)
            {
                case TableType.ProcessEvents:
                    return new List<string>()
                    {
                        ProcessEventsDeleteTriggerSql,
                        ProcessEventsInsertTriggerSql,
                        ProcessEventsUpdateTriggerSql,
                    };
                default:
                    throw new Exception($"Table type: {tableType} not supported");
            }
        }

        private static string GetContent(string resourceFileName)
        {
            var content = Resources.ResourceManager.GetString(resourceFileName);
            var filePath = ValidateSqlFilePath(resourceFileName);

            if (File.Exists(filePath) == false)
            {
                File.WriteAllText(filePath, content);
                return content;
            }
            else
            {
                return File.ReadAllText(filePath);
            }
        }

        private static string ValidateSqlFilePath(string sqlFileName)
        {
            var directory = "SqlCommands";
            if (Directory.Exists(directory) == false)
                Directory.CreateDirectory(directory);

            var path = Path.Combine(directory, $"{sqlFileName}.txt");
            return path;
        }
    }
}
