using System;
using System.Collections.Generic;
using DBAgent.Watcher.Models;

namespace DBAgent.Watcher.Events.Args
{
    [Serializable]
    public class ProcessEventsChangedEventArgs
    {
        public List<ProcessEventsActionModel> Models { get; set; } = new List<ProcessEventsActionModel>();
    }
}
