﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DbAgent.Watcher.Attributes;
using DBAgent.Watcher.Enums;

namespace DbAgent.Watcher.Models
{
    [Serializable]
    [TransferInfo("EMPLOYEE", "EMPLOYEE_ACTIONS")]
    [EventName(TriggerType.Insert, "EMPLOYEE_INSERT")]
    [EventName(TriggerType.Update, "EMPLOYEE_UPDATE")]
    [EventName(TriggerType.Delete, "EMPLOYEE_DELETE")]
    [TriggerName(TriggerType.Insert, "EMPLOYEE_TRIGGER_INSERT")]
    [TriggerName(TriggerType.Update, "EMPLOYEE_TRIGGER_UPDATE")]
    [TriggerName(TriggerType.Delete, "EMPLOYEE_TRIGGER_DELETE")]
    public class EmployeeActionModel : IModel
    {
        public EmployeeActionModel() { }

        [DbProperty("ID")] public int Id { get; set; }
        [DbProperty("PHONE_HOME")] public string PhoneHome { get; set; }
        [DbProperty("PHONE_MOBILE")] public string PhoneMobile { get; set; }
        [DbProperty("APPOINTMENT")] public int Appointment { get; set; }

        [DbProperty("CREATION_TIME", true)]
        public DateTime CreationTime { get; set; }

        [DbProperty("IS_DELETE_ACTION", true, TriggerType.Delete)]
        public int IsDeleteAction { get; set; }

        [DbProperty("IS_UPDATE_ACTION", true, TriggerType.Update)]
        public int IsUpdateAction { get; set; }

        [DbProperty("IS_INSERT_ACTION", true, TriggerType.Insert)]
        public int IsInsertAction { get; set; }


        public string GetDbProperty(string modelPropertyName)
        {
            return GetAttribute(modelPropertyName).PropertyName;
        }

        public DbPropertyAttribute GetAttribute(string propertyName)
        {
            var property = this.GetType().GetProperties().First(item => item.Name == propertyName);
            var attribute = (DbPropertyAttribute)property.GetCustomAttribute(typeof(DbPropertyAttribute));
            return attribute;

        }
    }
}
