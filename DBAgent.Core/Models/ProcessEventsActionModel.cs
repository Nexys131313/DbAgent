using System;

namespace DBAgent.Watcher.Models
{
    [Serializable]
    public class ProcessEventsActionModel
    {
        public int? Id { get; set; }
        public int? Done { get; set; }
        public int? CategoryId { get; set; }
        public int? ObjectId { get; set; }
        public DateTime? SoundOffTime { get; set; }
        public int? Electrician { get; set; }
        public int? GzId { get; set; }
        public DateTime? ArriveTime { get; set; }
        public DateTime? DoneTimestamp { get; set; }
        public int? SoundOff { get; set; }
        public int? ElectricianOn { get; set; }
        public int? GzOn { get; set; }
        public int? ArriveOn { get; set; }
        public int? ReasonOn { get; set; }
        public DateTime? GzTime { get; set; }
        public int? DescOn { get; set; }
        public int? UserId { get; set; }
        public string WUserName { get; set; }
        public string HostName { get; set; }
        public int? ReasonId { get; set; }
        public int? GObjectId { get; set; }
        public int? DepartureGz { get; set; }
        public DateTime? CreationTime { get; set; }
        public int? IsDeletedAction { get; set; }
        public int? IsUpdateAction { get; set; }
        public int? IsInsertAction { get; set; }
    }
}
