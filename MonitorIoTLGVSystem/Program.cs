using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorIoTLGVSystem
{
    class Program
    {
        private static readonly string connStringDevice1 = "HostName=IotHubTFMLGVMonitor.azure-devices.net;DeviceId=AGVDevice1;SharedAccessKey=/yOnI0DO5yX6Exy7mBpDUM/ivz2BYB13bAIoTMUqZL4=";
        private static readonly string connStringDevice2 = "HostName=IotHubTFMLGVMonitor.azure-devices.net;DeviceId=AGVDevice2;SharedAccessKey=GthumHniNwYU3THrb0kOh2oj+gCMmmIxFAIoTOiTRgI=";

        static void Main(string[] args)
        {
            Console.WriteLine("Nodo en el borde en espera de datos LGVs. Ctrl-C para salir.\n");
            
            MonitorLGV monitorLGV1=new MonitorLGV("agv1","agv1", connStringDevice1,1);
            MonitorLGV monitorLGV2 = new MonitorLGV("agv1", "agv1", connStringDevice2, 2);
            
            monitorLGV1.connect();
            monitorLGV2.connect();
            
            Console.WriteLine("Espera de datos");
            monitorLGV1.consume();
            monitorLGV2.consume();
            
            Console.ReadLine();
            monitorLGV1.disconnect();
            monitorLGV2.disconnect();
        }
    }
}
