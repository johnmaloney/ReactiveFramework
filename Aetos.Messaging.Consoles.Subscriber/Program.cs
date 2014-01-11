using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aetos.Messaging.Consoles.Subscriber
{
    class Program
    {
        private static Random Random = new Random();

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            string mode;

            if (args.Length == 0)
            {
                Console.WriteLine("Subscribe to?   {t}-topic or {q}-queue");
                mode = Console.ReadLine();
            }
            else
            {
                mode = args[0];
            }

            switch(mode)
            {
                case "t":
                    TopicSubscriber.Run(args);
                    break;
                case "q":
                    QueueSubscriber.Run(args);
                    break;
            }
        }
    }
}
