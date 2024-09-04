using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorIoTLGVSystem
{
    public class LGVTimes_Dto:Dto
    {       
        public int PercentAutomatic { get; set; }
        public int PercentManual { get; set; }
        public int PercentInserted { get; set; }
        public int PercentNotInserted { get; set; }
        public int PercentAllocated { get; set; }
        public int PercentFree { get; set; }
        public int PercentLoaded { get; set; }
        public int PercentUnloaded { get; set; }
        public int PercentMoving { get; set; }
        public int PercentNotMoving { get; set; }
        public int PercentTrafficBlocked { get; set; }
        public int PercentNoTrafficBlocked { get; set; }
        public int PercentInAlarm { get; set; }
        public int PercentNoInAlarm { get; set; }
        public int PercentInCharge { get; set; }
        public int PercentNoInCharge { get; set; }
    }
}
