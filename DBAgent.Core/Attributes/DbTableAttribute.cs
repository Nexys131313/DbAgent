using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBAgent.Watcher.Enums;

namespace DbAgent.Watcher.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DbTableAttribute: Attribute
    {
        public DbTableAttribute(string tableName,  string[] eventNames)
        {
            TableName = tableName;
            EventNames = eventNames.ToList();
        }

        public List<string> EventNames { get; }
        public string TableName { get; }
    }
}
