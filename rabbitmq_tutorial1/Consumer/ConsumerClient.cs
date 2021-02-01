using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Consumer
{
    class ConsumerClient
    {
        IConnection connection;
        IModel channel;
        public ConsumerClient()
        {
            connection = GetConnection();
            channel = GetChannel();


        }

        ~ConsumerClient()
        {
            channel?.Close();
            connection?.Close();
        }

        public void RunReceive(ManualResetEvent resetEvent)
        {
            
            var consumerEvent = new EventingBasicConsumer(channel);
            consumerEvent.Received += (s, e) =>
            {
                var receive = System.Text.Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine($"Receive message: \t" + receive);
                (s as IBasicConsumer).Model.BasicAck(e.DeliveryTag, false);
            };


            channel.BasicConsume("zerpico", false, consumerEvent);

            Console.WriteLine("start receive ..");

            resetEvent.WaitOne();
        }

        private IConnection GetConnection()
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
                HostName = "192.168.1.138"
            };
            IConnection conn = factory.CreateConnection();
            return conn;
        }

        private IModel GetChannel()
        {
            IModel model = GetConnection().CreateModel();
            return model;
        }


        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
