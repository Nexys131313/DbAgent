using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAgent.Watcher.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DbTableAttribute: Attribute
    {
        public DbTableAttribute(string tableName)
        {
            TableName = tableName;
        }

        public string TableName { get; }
    }
}
