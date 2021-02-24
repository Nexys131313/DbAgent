﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DbAgent.Watcher.Attributes;
using DBAgent.Watcher.Enums;
using Newtonsoft.Json;

namespace DbAgent.Watcher.Helpers
{
    public class SqlTriggerScheme<TModel> 
    {
        public SqlTriggerScheme(TriggerType triggerType)
        {
            var eventsData = typeof(TModel).GetCustomAttributes<EventNameAttribute>();
            var triggersData = typeof(TModel).GetCustomAttributes<TriggerNameAttribute>();

            var tableName = typeof(TModel).GetCustomAttribute<DbTableNameAttribute>().TableName;
            var eventName = eventsData.First(item => item.TriggerType == triggerType).EventName;
            var triggerName = triggersData.First(item => item.TriggerType == triggerType).TriggerName;

            TriggerType = triggerType;
            TableName = tableName;
            EventName = eventName;
            TriggerName = triggerName;
        }

        public string TriggerName { get;  }
        public string TableName { get;  }
        public TriggerType TriggerType { get;  }
        public string EventName { get;  }

        public string ExternalTableName { get; set; }
        public string ExternalDataSource { get; set; }
        public string ExternalUser { get; set; }
        public string ExternalPassword { get; set; }

        [JsonIgnore]
        public Type ModelType => typeof(TModel);

        public static IEnumerable<SqlTriggerScheme<TModel>> InitializeSchemes(string externalTableName,
            string externalDataSource, string externalUser, string externalPassword, IEnumerable<TriggerType> triggers)
        {
            var result = new List<SqlTriggerScheme<TModel>>();

            foreach (var trigger in triggers)
            {
                var scheme = new SqlTriggerScheme<TModel>(trigger)
                {
                    ExternalTableName = externalTableName,
                    ExternalDataSource = externalDataSource,
                    ExternalPassword = externalPassword,
                    ExternalUser = externalUser,
                };
                result.Add(scheme);
            }

            return result;
        }
    }
}
