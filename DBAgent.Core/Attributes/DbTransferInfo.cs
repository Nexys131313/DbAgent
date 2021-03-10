using System;

namespace DbAgent.Watcher.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DbTransferInfo: Attribute
    {
        public DbTransferInfo(string mainTableName, string actionsTableName)
        {
            MainTableName = mainTableName;
            ActionsTableName = actionsTableName;
        }

        public string MainTableName { get; }
        public string ActionsTableName { get;  }
    }
}
