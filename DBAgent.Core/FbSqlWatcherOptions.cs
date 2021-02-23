using System;
using System.Collections.Generic;
using DBAgent.Watcher.Enums;

namespace DBAgent.Watcher
{
    [Serializable]
    public class FbSqlWatcherOptions
    {
        public string TriggersFilePath { get; set; }
        public List<TableType> Tables { get; set; } = new List<TableType>();
    }
}
