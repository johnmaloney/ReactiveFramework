using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aetos.Messaging.Consoles.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            string cloud;
            string mode;
            string method;

            if (args.Length == 0)
            {
                Console.WriteLine("Run from the CLOUD? ({p} - primary | {s} - secondary | {b} both)");
                cloud = Console.ReadLine();
                Console.WriteLine("Which action? ({p} - publish | {s} - send)");
                method = Console.ReadLine();
                Console.WriteLine("Publish? ({t} - topic | {q} queue)");
                mode = Console.ReadLine();
            }
            else
            {
                cloud = args[0];
                method = args[1];
                mode = args[2];
            }

            switch (mode)
            {
                case "t":
                    TopicPublisher.Run(cloud, method);
                    break;
                case "q":
                    QueuePublisher.Run(cloud, method);
                    break;
            }
        }
    }
}
