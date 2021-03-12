using System;
using System.Collections.Generic;
using System.Text;

namespace DbAgent.Manage
{
    public class RedisManagerOptions
    {
        public string MainDbConnectionString { get; set; }
        public string TempDbConnectionString { get; set; }
        public string TriggersFilePath { get; set; }
    }
}
