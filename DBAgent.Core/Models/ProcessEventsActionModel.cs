using System;
using System.Linq;
using System.Reflection;
using DbAgent.Watcher.Attributes;
using DBAgent.Watcher.Enums;
using DbAgent.Watcher.Models;

namespace DBAgent.Watcher.Models
{
    [Serializable]
    [DbTransferInfo("PROCESS_EVENTS", "PROCESS_EVENTS_ACTIONS")]
    [DbEvent(TriggerType.Insert, "PROCESS_EVENTS_INSERT")]
    [DbEvent(TriggerType.Update, "PROCESS_EVENTS_UPDATE")]
    [DbEvent(TriggerType.Delete, "PROCESS_EVENTS_DELETE")]
    [DbTrigger(TriggerType.Insert, "PE_TRIGGER_INSERT")]
    [DbTrigger(TriggerType.Update, "PE_TRIGGER_UPDATE")]
    [DbTrigger(TriggerType.Delete, "PE_TRIGGER_DELETE")]
    public class ProcessEventsActionModel : IModel
    {
        public ProcessEventsActionModel() { }

        [DbProperty("ID")] public int? Id { get; set; }
        [DbProperty("DONE")] public int? Done { get; set; }
        [DbProperty("CATEGORY_ID")] public int? CategoryId { get; set; }
        [DbProperty("OBJECT_ID")] public int? ObjectId { get; set; }
        [DbProperty("SOUND_OFF_TIME")] public DateTime? SoundOffTime { get; set; }
        [DbProperty("ELECTRICIAN")] public int? Electrician { get; set; }
        [DbProperty("GZ_ID")] public int? GzId { get; set; }
        [DbProperty("ARRIVE_TIME")] public DateTime? ArriveTime { get; set; }
        [DbProperty("DONE_TIMESTAMP")] public DateTime? DoneTimestamp { get; set; }
        [DbProperty("SOUND_OFF")] public int? SoundOff { get; set; }
        [DbProperty("ELECTRICIAN_ON")] public int? ElectricianOn { get; set; }
        [DbProperty("GZ_ON")] public int? GzOn { get; set; }
        [DbProperty("ARRIVE_ON")] public int? ArriveOn { get; set; }
        [DbProperty("REASON_ON")] public int? ReasonOn { get; set; }
        [DbProperty("GZ_TIME")] public DateTime? GzTime { get; set; }
        [DbProperty("DESC_ON")] public int? DescOn { get; set; }
        [DbProperty("USER_ID")] public int? UserId { get; set; }
        [DbProperty("WUSER_NAME")] public string WUserName { get; set; }
        [DbProperty("HOST_NAME")] public string HostName { get; set; }
        [DbProperty("REASON_ID")] public int? ReasonId { get; set; }
        [DbProperty("GOBJECT_ID")] public int? GObjectId { get; set; }
        [DbProperty("DEPARTURE_GZ")] public int? DepartureGz { get; set; }


        [DbProperty("CREATION_TIME", true)]
        public DateTime? CreationTime { get; set; }

        [DbProperty("IS_DELETE_ACTION", true, TriggerType.Delete)]
        public int? IsDeleteAction { get; set; }
        [DbProperty("IS_UPDATE_ACTION", true, TriggerType.Update)]
        public int? IsUpdateAction { get; set; }
        [DbProperty("IS_INSERT_ACTION", true, TriggerType.Insert)]
        public int? IsInsertAction { get; set; }

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
