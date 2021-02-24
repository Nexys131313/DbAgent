using System;

namespace DbAgent.Watcher.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DbTableNameAttribute: Attribute
    {
        public DbTableNameAttribute(string tableName)
        {
            TableName = tableName;
        }

        public string TableName { get; }
    }
}
