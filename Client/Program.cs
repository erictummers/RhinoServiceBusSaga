using Events;
using Rhino.ServiceBus;
using Rhino.ServiceBus.Hosting;
using System;

namespace Client
{
    class Program
    {
        /// <summary>
        /// Send some <see cref="ReceivedFileEvent"/> to see the
        /// Service handle them in the Saga
        /// </summary>
        static void Main(string[] args)
        {
            var host = new DefaultHost();
            host.Start<ClientBootstrapper>();
            var bus = host.Bus as IServiceBus;

            PublishEventAfterEnter(bus);
            PublishEventAfterEnter(bus);
            PublishEventAfterEnter(bus);
            PublishEventAfterEnter(bus);
            PublishEventAfterEnter(bus);
            PublishEventAfterEnter(bus);

            Console.WriteLine("Press <CR> to end");
            Console.ReadLine();
        }

        private static void PublishEventAfterEnter(IServiceBus bus)
        {
            Console.WriteLine("Press <CR> to publish message");
            Console.ReadLine();
            bus.Publish(new ReceivedFileEvent { PathToFile = @"d:\temp\SrtTrail.txt" });
        }
    }
}
