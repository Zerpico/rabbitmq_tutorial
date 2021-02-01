using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

namespace Publisher
{
    class Program
    {
        static bool isExit = false;
        static void Main(string[] args)
        {
            var resetEvent = new ManualResetEvent(false);

            //AppDomain.CurrentDomain.ProcessExit += (s, e) => resetEvent.Set();
            Console.CancelKeyPress += (s, e) =>
            {
                isExit = true;
                resetEvent.Set();
                e.Cancel = true;
            };
            
            Console.WriteLine("Type text and press Enter to send to the queue ");
            
            PublisherClient client = new PublisherClient();

            while(!isExit)
            {
                var message = Console.ReadLine();   
                client.SendMessage(message);
            }
            
        }
    }
}
