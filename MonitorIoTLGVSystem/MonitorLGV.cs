using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Azure.Devices.Client;
using System.Collections;

namespace MonitorIoTLGVSystem
{
    enum MessageType { 
        LGVData=1,
        LGVTime

    }

    public class MonitorLGV
    {
        #region PROPERTIES

        ConnectionFactory? factory;
        IConnection? connection;
        IModel? channel;
        string user = string.Empty;
        string password = string.Empty;
        private DeviceClient? deviceClient;
        private string connectionString=string.Empty;// 
        private string stragvId= string.Empty;
        private int agvId = 0;
        private Dictionary<int,string> alarms = new Dictionary<int, string>();
        
        private DateTime startDateTime;
        private bool startCount=false;
        private int countAutomatic=0;
        private int countInserted=0;
        private int countAllocated=0;
        private int countLoaded=0;
        private int countMoving=0;
        private int countTrafficBlocked=0;
        private int countAlarm = 0;
        private int countInBatteryCharge = 0;
        private int countMsgInanHour = 0;

        #endregion

        #region CONSTRUCTOR

        public MonitorLGV(string p_user, string p_password,string p_connString, int p_agvId)
        {
            user = p_user;
            password = p_password;
            connectionString= p_connString;
            agvId = p_agvId;
            stragvId ="agv"+ p_agvId;
            initAlarmList();
            startDateTime= DateTime.Now;
        }

        #endregion

        #region METHODS

        public void connect()
        {
            try
            {
                factory = new ConnectionFactory();
                factory.Uri = new Uri("amqp://" + user + ":" + password + "@localhost:5672");
                connection = factory.CreateConnection();  
                channel = connection.CreateModel();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

        }

        public void consume()
        {
            try
            {
                var exchangeName = "AgvData";
                var exchangeQueue = "AgvDataQueue" + stragvId;
                var routingKey = "data." + stragvId;
                if(channel!=null)
                {
                    channel.QueueDeclare(exchangeQueue, true, false, false);
                    channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true);
                    channel.QueueBind(exchangeQueue, exchangeName, routingKey);
                    deviceClient = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Mqtt);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, eventArgs) =>
                    {                        
                        var msg = Encoding.UTF8.GetString(eventArgs.Body.Span);
                        LGVData_Dto? lgvdto = JsonConvert.DeserializeObject<LGVData_Dto>(msg);
                        Console.WriteLine($"{eventArgs.RoutingKey}: ID LGV {lgvdto?.ID} , Fecha {lgvdto?.Date_Time.ToString()}, Segmento {lgvdto?.IDSegmento}, Punto {lgvdto?.IDPunto}");                        
                        var message = GetLGVDataMessage(lgvdto);
                        Task.Run(async () => await deviceClient.SendEventAsync(message));
                        //check eficiencia LGV
                        IncreaseEfficiencyLGVCounters(lgvdto);
                        TimeSpan diff = lgvdto.Date_Time - startDateTime;
                        //Si ha pasado una hora envio mensaje con porcentaje
                        if (diff.TotalHours > 0)
                        {
                            //creo un mensaje con los porcentajes
                            message = GetLGVTimesMessage();
                            Task.Run(async () => await deviceClient.SendEventAsync(message));
                            ResetCounters();
                        }
                    };
                    channel.BasicConsume(exchangeQueue, true, consumer);
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private List<LGV_alarm> GetLGVAlarms(int p_byte)
        {
            List<LGV_alarm> list = new List<LGV_alarm>();
            try
            {
                BitArray myBitArray = new BitArray(System.BitConverter.GetBytes(p_byte));
                for (int i = 0; i < myBitArray.Length; i++)
                {
                    if (myBitArray[i])
                    {
                        list.Add(new LGV_alarm() { Code = i + 1, Description = alarms[i + 1] });
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }            
            return list;
        }

        private Message GetLGVDataMessage(LGVData_Dto lgvdto)
        {
            Message message = new Message();

            lgvdto.LGVAlarm = GetLGVAlarms(lgvdto.AlarmasByte1);
            lgvdto.MsgType = (int)MessageType.LGVData;
            //lleno lista de alarmas
            string msgfinal = JsonConvert.SerializeObject(lgvdto);
            message = new Message(Encoding.ASCII.GetBytes(msgfinal));

            return message;

        }

        private Message GetLGVTimesMessage()
        {
            Message message = new Message();

             LGVTimes_Dto times_Dto = new LGVTimes_Dto();

            times_Dto.Date_Time = startDateTime;
            times_Dto.ID = agvId;
            times_Dto.MsgType=(int)MessageType.LGVTime;
            times_Dto.PercentAutomatic = (countMsgInanHour!=0) ? ((countAutomatic / countMsgInanHour) * 100) : 0;
            times_Dto.PercentManual = (countMsgInanHour != 0) ? (((countMsgInanHour - countAutomatic) / countMsgInanHour) * 100) : 0;
            times_Dto.PercentInserted = (countMsgInanHour != 0) ? ((countInserted / countMsgInanHour) * 100) : 0;
            times_Dto.PercentNotInserted = (countMsgInanHour != 0) ? (((countMsgInanHour - countInserted) / countMsgInanHour) * 100) : 0;
            times_Dto.PercentAllocated = (countMsgInanHour != 0) ? ((countAllocated / countMsgInanHour) * 100) : 0;
            times_Dto.PercentFree = (countMsgInanHour != 0) ? (((countMsgInanHour - countAllocated) / countMsgInanHour) * 100) : 0;
            times_Dto.PercentLoaded = (countMsgInanHour != 0) ? ((countLoaded / countMsgInanHour) * 100) : 0;
            times_Dto.PercentUnloaded = (countMsgInanHour != 0) ? (((countMsgInanHour - countLoaded) / countMsgInanHour) * 100) : 0;
            times_Dto.PercentMoving = (countMsgInanHour != 0) ? ((countMoving / countMsgInanHour) * 100) : 0;
            times_Dto.PercentNotMoving = (countMsgInanHour != 0) ? (((countMsgInanHour - countMoving) / countMsgInanHour) * 100) : 0;
            times_Dto.PercentTrafficBlocked = (countMsgInanHour != 0) ? ((countTrafficBlocked / countMsgInanHour) * 100) : 0;
            times_Dto.PercentNoTrafficBlocked = (countMsgInanHour != 0) ? (((countMsgInanHour - countTrafficBlocked) / countMsgInanHour) * 100) : 0;
            times_Dto.PercentInAlarm = (countMsgInanHour != 0) ? ((countAlarm / countMsgInanHour) * 100) : 0;
            times_Dto.PercentNoInAlarm = (countMsgInanHour != 0) ? (((countMsgInanHour - countAlarm) / countMsgInanHour) * 100) : 0;
            times_Dto.PercentInCharge = (countMsgInanHour != 0) ? ((countInBatteryCharge / countMsgInanHour) * 100) : 0;
            times_Dto.PercentNoInCharge = (countMsgInanHour != 0) ? (((countMsgInanHour - countInBatteryCharge) / countMsgInanHour) * 100) : 0;

            string msgfinal = JsonConvert.SerializeObject(times_Dto);
            message = new Message(Encoding.ASCII.GetBytes(msgfinal));

            return message;
        }

        private void ResetCounters()
        {
            startDateTime= DateTime.Now;            
            countAutomatic = 0;
            countInserted = 0;
            countAllocated = 0;
            countLoaded = 0;
            countMoving = 0;
            countTrafficBlocked = 0;
            countAlarm = 0;
            countInBatteryCharge = 0;
            countMsgInanHour = 0;
        }

        private void initAlarmList()
        {
            alarms.Add(1, "Lectura obstaculo sensor delantero");
            alarms.Add(2, "Lectura obstaculo sensor trasero");
            alarms.Add(3, "Hongo de seguridad delantero ON");
            alarms.Add(4, "Hongo de seguridad trasero ON");
            alarms.Add(5, "Hongo de seguridad derecho ON");
            alarms.Add(6, "Hongo de seguridad izquierdo ON");
            alarms.Add(7, "Nivel Bajo de navegacion");
            alarms.Add(8, "Bateria descargada");
        }

        private void IncreaseEfficiencyLGVCounters(LGVData_Dto? lgv)
        {
            try
            {
                if (!startCount)
                {
                    startDateTime = lgv.Date_Time;
                    startCount = true;
                    countMsgInanHour = 0;
                }
                TimeSpan diff = lgv.Date_Time - startDateTime;
                if (diff.TotalHours == 0)
                {
                    if (lgv.LGVEnAutomatico)
                        countAutomatic++;
                    if(lgv.LGVIntroducido)
                        countInserted++;
                    if (lgv.TareaAsginada)
                        countAllocated++;                    
                    if(lgv.LGVCargado)
                        countLoaded++;  
                    if(lgv.LGVEnMovimiento)
                        countMoving++;
                    if (lgv.LGVBloqueadoPorTrafico)
                        countTrafficBlocked++;
                    if (lgv.AlarmasByte1 != 0 || lgv.AlarmasByte2 != 0)
                        countAlarm++;
                    if (lgv.LGVCargandoBateria)
                        countInBatteryCharge++;
                    countMsgInanHour++; //incremento numero de mensajes
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void disconnect()
        {

            try
            {
                channel?.Close();
                connection?.Close();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

        }

        #endregion 
    }
}
