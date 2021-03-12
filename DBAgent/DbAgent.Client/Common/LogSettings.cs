using System;
using System.IO;
using Melnik.Logging.ConsoleProvider;
using Melnik.Logging.Extensions;
using Melnik.Logging.FileProvider;
using Microsoft.Extensions.Logging;

namespace DbAgent.Client.Common
{
    public static class LogSettings
    {
        static LogSettings()
        {
            LoggerFactory = new LoggerFactory();

            var fileOptions = new FileLoggerOptions
            {
                IsLogCallingMethodsSequence = false, 
                LogFilePath = GetLogSessionFilePath()
            };
            LoggerFactory.AddFileProvider(fileOptions);

            var consoleOptions = new ConsoleLoggerOptions()
            {
                IsLogCallingMethodsSequence = false,
            };

            LoggerFactory.AddConsoleProvider(consoleOptions);
        }

        public static ILoggerFactory LoggerFactory { get; }

        private static string GetLogSessionFilePath()
        {
            var fileName = $"{DateTime.Now:dd-MM-yy HH-mm-ss}";
            var filePath = Path.Combine(AppSettings.LogsDirectoryPath, fileName);
            return filePath;
        }
    }
}
