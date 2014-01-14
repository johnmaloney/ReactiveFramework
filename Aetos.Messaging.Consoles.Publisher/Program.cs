using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aetos.Messaging.Consoles.Publisher
{
    class Program
    {
        public static Random Random = new Random();
        public static List<string> UpdateTitles { get; set; }

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
                Console.WriteLine("Which action? ({s} - publish single item | {ss} publish single item to subscription only | {m} - publish mulitple items)");
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

        static Program()
        {
            UpdateTitles = new List<string>()
                {
                    "Update user file name preferences.", 
                    "Update login information.",
                    "Update max amount of xyz.", 
                    "Change the default load time for abc.", 
                    "Set the message title when entering application.", 
                    "Add new payment methods to account.", 
                    "Update contact information.", 
                    "Alter the max return values for search.", 
                    "Change user preferences for notification.", 
                    "Add new default maps to user account"
                };
            
        }

        public static string GetRandomUpdate()
        {
            return UpdateTitles[Random.Next(0, UpdateTitles.Count - 1)];
        }
    }
}
