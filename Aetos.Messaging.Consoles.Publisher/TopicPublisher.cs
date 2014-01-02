using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aetos.Messaging.Interfaces;

namespace Aetos.Messaging.Consoles.Publisher
{
    public class TopicPublisher
    {
        private static ITopicClient TopicClient;

        public static void Run(string cloud, string method)
        {
            SetupTopicClient(cloud);

            if (method == "s")
            {
                PublishSingleMessage();
            }
            else
            {
                PublishTopic();
            }
        }

        private static void SetupTopicClient(string cloud)
        {
            var topicClient = new TopicClient(Topic.General);

            switch(cloud)
            {
                case "p":
                    TopicClient = topicClient.Primary;
                    break;
                case "s":
                    TopicClient = topicClient.Secondary;
                    break;
                case "b":
                    TopicClient = topicClient;
                    break;
            }
            Console.WriteLine("--Publisher Ready--");
        }

        private static void PublishSingleMessage()
        {
            var cmd = Console.ReadLine();
            while (cmd != "x")
            {
                PublishMockGeneralMessage();
                cmd = Console.ReadLine();
            }
        }

        private static void PublishMockGeneralMessage()
        {
            var generalMessage = GetGeneralMessage();
            TopicClient.Publish(new Message
                {
                    Body = generalMessage
                });
            Console.WriteLine("Published new General Message.");
        }

        public static dynamic GetGeneralMessage()
        {
            // This is a default or test message generator //
            return null;
        }
    }
}
