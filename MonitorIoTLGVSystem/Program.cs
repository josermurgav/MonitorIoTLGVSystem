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
        private static readonly string connStringDevice1 = "[Connection string device 1]";
        private static readonly string connStringDevice2 = "[Connection string device 2]";

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
