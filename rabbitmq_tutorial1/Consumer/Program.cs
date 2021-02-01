using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {            
            var resetEvent = new ManualResetEvent(false);

            AppDomain.CurrentDomain.ProcessExit += (s, e) => resetEvent.Set();
            Console.CancelKeyPress += (s, e) =>
            {             
                resetEvent.Set();
                e.Cancel = true;
            };

            Console.WriteLine("[Ctrl] + [C] to exit program ");

            ConsumerClient clinet = new ConsumerClient();
            clinet.RunReceive(resetEvent);

            
        }

     
    }
}
