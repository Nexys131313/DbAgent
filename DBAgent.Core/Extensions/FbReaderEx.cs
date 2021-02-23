using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAgent.Watcher.Extensions
{
    public static class FbReaderEx
    {
        public static T? Value<T>(this object dbValue) where T: struct
        {
            if (dbValue is DBNull)
            {
                return null;
            }
            else
            {
                return (T) dbValue;
            }
        }

        public static string ValueStr(this object dbValue)
        {
            if (dbValue is DBNull)
            {
                return "";
            }
            else
            {
                return (string) dbValue;
            }
        }
    }
}
