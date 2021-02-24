using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBAgent.Watcher.Enums;

namespace DbAgent.Watcher.Helpers
{
    public class SqlTriggerScheme<TModel>
    {
        public string TriggerName { get; set; }
        public string TableName { get; set; }
        public TriggerType TriggerType { get; set; }
        public string EventName { get; set; }

        public string ExternalTableName { get; set; }
        public string ExternalDataSource { get; set; }
        public string ExternalUser { get; set; }
        public string ExternalPassword { get; set; }

        public TModel InsertDataModel { get; set; }
    }
}
