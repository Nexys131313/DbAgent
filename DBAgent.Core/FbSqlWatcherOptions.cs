using System;

namespace DBAgent.Watcher
{
    [Serializable]
    public class FbSqlWatcherOptions
    {
        public FbSqlWatcherOptions(string mainDbConnectionStr,
            string tempDbConnectionStr, string triggersFilePath)
        {
            MainDbConnectionString = mainDbConnectionStr;
            TempDbConnectionString = tempDbConnectionStr;
            TriggersFilePath = triggersFilePath;
        }

        public string MainDbConnectionString { get; set; }
        public string TempDbConnectionString { get; set; }
        public string TriggersFilePath { get; set; } 
    }
}
