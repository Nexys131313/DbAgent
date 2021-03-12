using System;

namespace DBAgent.Watcher
{
    [Serializable]
    public class FbSqlWatcherOptions
    {
        public FbSqlWatcherOptions(string mainDbConnectionStr,
            string tempDbConnectionStr)
        {
            MainDbConnectionString = mainDbConnectionStr;
            TempDbConnectionString = tempDbConnectionStr;
        }

        public string MainDbConnectionString { get; set; }
        public string TempDbConnectionString { get; set; }
    }
}
