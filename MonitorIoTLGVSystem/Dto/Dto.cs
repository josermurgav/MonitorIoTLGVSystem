using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorIoTLGVSystem
{
    public class Dto
    {
        public Dto()
        {
            
        }

        public int ID { get; set; }
        public DateTime Date_Time { get; set; }
        public int MsgType { get; set; }
    }
}
