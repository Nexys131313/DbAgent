using System;

namespace DbAgent.Watcher.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TransferInfo: Attribute
    {
        public TransferInfo(string mainTableName, string actionsTableName)
        {
            MainTableName = mainTableName;
            ActionsTableName = actionsTableName;
        }

        public string MainTableName { get; }
        public string ActionsTableName { get;  }
    }
}
