using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melnik.Logging.Extensions;
using Melnik.Logging.FileProvider;
using Microsoft.Extensions.Logging;

namespace DbAgent.Service.Architecture
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
        }

        public static ILoggerFactory LoggerFactory { get; }

        private static string GetLogSessionFilePath()
        {
            var fileName = $"{DateTime.Now:dd-MM-yy HH-mm}";
            var filePath = Path.Combine(AppSettings.LogsDirectoryPath, fileName);
            return filePath;
        }
    }
}
