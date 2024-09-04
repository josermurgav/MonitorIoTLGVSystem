using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGV_Device
{
    class Program
    {
        static void Main(string[] args)
        { 
            LGV_Device lgv=new LGV_Device("agv1","agv1");
            CSVDataReader reader = new CSVDataReader();
            lgv.connect();
            
            Console.WriteLine("lectura Datos LGV");

            var lgvData=reader.GetLGVData("D:\\TFM\\Files\\agv1.csv");

            Console.WriteLine("Envio de datos");
            if (lgvData != null) {
                foreach (var lgv_dto in lgvData)
                {
                    lgv_dto.Date_Time = DateTime.Now;
                    lgv.sendObjectMessage(lgv_dto);
                    Thread.Sleep(1000);
                }                
            }
            

            Console.WriteLine("Escribe algo para cerrar");
            Console.ReadLine();
            lgv.disconnect();


        }
    }
}
