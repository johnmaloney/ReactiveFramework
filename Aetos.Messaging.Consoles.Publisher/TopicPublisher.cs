using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Aetos.Messaging.Domain;
using Aetos.Messaging.Domain.Events;
using Aetos.Messaging.Domain.Clients;
using Aetos.Messaging.Interfaces;

namespace Aetos.Messaging.Consoles.Publisher
{
    public class TopicPublisher
    {
        #region Fields

        private static ITopicClient TopicClient;

        #endregion

        #region Properties
        #endregion

        #region Methods

        public static void Run(string cloud, string method)
        {
            SetupTopicClient(cloud);

            if (method == "s")
            {
                PublishSingleMessage();
            }
            else
            {
                PublishSingleMessage();
            }
        }

        private static void SetupTopicClient(string cloud)
        {
            var topicClient = new TopicClient(Topic.GeneralEvent);

            switch (cloud)
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

        public static GeneralEvent GetGeneralMessage()
        {
            // This is a default or test message generator //
            return new GeneralEvent()
            {
                Message = "New Topic, identifier: #" + new Random().Next().ToString(),
                LastUpdated = DateTime.UtcNow
            };
        }

        #endregion
    }
}
