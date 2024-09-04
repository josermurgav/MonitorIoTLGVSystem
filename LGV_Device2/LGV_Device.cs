using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;

namespace LGV_Device
{
    public class LGV_Device
    {
        #region PROPERTIES

        ConnectionFactory? factory;
        IConnection? connection;
        IModel? channel;
        string user = string.Empty;
        string password = string.Empty;

        #endregion

        #region CONSTRUCTOR

        public LGV_Device(string p_user, string p_password)
        {
            user = p_user;
            password = p_password;
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

        public void sendMessage(string agvId,string msg)
        {
            try
            {
                var exchangeName = "AgvData";
                var routingKey = "data."+agvId;

                var bytes=Encoding.UTF8.GetBytes(msg);
                channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true);
                channel.BasicPublish(exchangeName, routingKey, null, bytes);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }


        public void sendObjectMessage(LGV_Dto lgv)
        {
            try
            {
                var exchangeName = "AgvData";
                var routingKey = "data.agv" + lgv.ID;

                Console.WriteLine($"{routingKey}: ID LGV {lgv?.ID} , Fecha {lgv?.Date_Time.ToString()}, Segmento {lgv?.IDSegmento}, Punto {lgv?.IDPunto}");

                string msg= JsonConvert.SerializeObject(lgv);
                var bytes = Encoding.UTF8.GetBytes(msg);
                channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true);
                channel.BasicPublish(exchangeName, routingKey, null, bytes);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }




        public void disconnect() {

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
