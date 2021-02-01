using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Publisher
{
    public class PublisherClient
    {
        IConnection connection;
        IModel channel;
        public PublisherClient()
        {
            connection = GetConnection();
            channel = GetChannel();
        }

        ~PublisherClient()
        {
            channel?.Close();
            connection?.Close();
        }

        public void SendMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                var messageEncod = System.Text.Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("", "zerpico", null, messageEncod);
                Console.WriteLine($"[{DateTime.Now.ToString()}]\tmessage sent\tsize: {messageEncod.Length}");
            }
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

        
    }
}
