using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DbAgent.Watcher.Attributes;
using DBAgent.Watcher.Enums;
using Newtonsoft.Json;

namespace DbAgent.Watcher.Scheme
{
    public class SqlTriggerScheme<TModel> 
    {
        private SqlTriggerScheme(TriggerType triggerType)
        {
            var eventsData = typeof(TModel).GetCustomAttributes<DbEventAttribute>();
            var triggersData = typeof(TModel).GetCustomAttributes<DbTriggerAttribute>();

            var externalTableName = typeof(TModel).GetCustomAttribute<DbTransferInfo>().ActionsTableName;
            var mainTableName = typeof(TModel).GetCustomAttribute<DbTransferInfo>().MainTableName;

            var eventName = eventsData.First(item => item.TriggerType == triggerType).EventName;
            var triggerName = triggersData.First(item => item.TriggerType == triggerType).TriggerName;

            TriggerType = triggerType;
            ExternalTableName = externalTableName;
            EventName = eventName;
            TriggerName = triggerName;
            MainTableName = mainTableName;
        }

        public string TriggerName { get;  }
        public string MainTableName { get;  }
        public TriggerType TriggerType { get;  }
        public string EventName { get;  }
        public string ExternalTableName { get;  }

        public string ExternalDataSource { get; set; }
        public string ExternalUser { get; set; }
        public string ExternalPassword { get; set; }

        [JsonIgnore]
        public Type ModelType => typeof(TModel);


        internal static SqlTriggerScheme<TModel> CreateScheme(string externalDataSource,
            string externalUser, string externalPassword, TriggerType trigger)
        {
            var scheme = new SqlTriggerScheme<TModel>(trigger)
            {
                ExternalDataSource = externalDataSource,
                ExternalPassword = externalPassword,
                ExternalUser = externalUser,
            };

            return scheme;
        }

        internal static IEnumerable<SqlTriggerScheme<TModel>> CreateSchemes(string externalDataSource,
            string externalUser, string externalPassword, IEnumerable<TriggerType> triggers)
        {
            return triggers.Select(trigger => CreateScheme(externalDataSource, externalUser, externalPassword, trigger)).ToList();
        }
    }
}
