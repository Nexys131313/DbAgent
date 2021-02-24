using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBAgent.Watcher.Enums;

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
