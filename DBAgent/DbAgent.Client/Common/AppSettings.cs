﻿using System.IO;

namespace DbAgent.Client.Common
{
    public static class AppSettings
    {
        static AppSettings()
        {
            EnsureDirectoryExists(DataDirectoryPath);
            EnsureDirectoryExists(LogsDirectoryPath);
        }

        public static string DataDirectoryPath => "Data";
        public static string LogsDirectoryPath => Path.Combine(DataDirectoryPath, "Logs");

        public static string AppConfigPath => Path.Combine(DataDirectoryPath, "AppConfig.json");
        public static string DefaultAppConfigPath => Path.Combine(DataDirectoryPath, "Default_AppConfig.json");

        public static string TriggersFilePath => Path.Combine(DataDirectoryPath, "Triggers.json");

        private static void EnsureDirectoryExists(string directoryPath)
        {
            if (Directory.Exists(directoryPath) == false)
                Directory.CreateDirectory(directoryPath);
        }
    }
}
