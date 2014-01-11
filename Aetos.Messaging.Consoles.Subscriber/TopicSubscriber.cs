using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aetos.Messaging.Common.Logging;
using Aetos.Messaging.Domain;
using Aetos.Messaging.Domain.Clients;
using Aetos.Messaging.Domain.Events;
using Aetos.Messaging.Interfaces;

namespace Aetos.Messaging.Consoles.Subscriber
{
    public static class TopicSubscriber
    {
        #region Fields

        private static Random Random = new Random();
        private static ITopicClient TopicClient;

        #endregion

        #region Properties
        #endregion

        #region Methods

        public static void Run(string[] args)
        {
            string subName;
            if (args.Length < 2)
            {
                Console.WriteLine("Subscription name?");
                subName = Console.ReadLine();
            }
            else
            {
                subName = args[1];
            }
            Console.Title = "Topic subscriber: " + subName;
            Subscribe(subName);
        }

        private static void Subscribe(string subName)
        {
            TopicClient = new TopicClient(Topic.GeneralEvent, subName);
            TopicClient.Subscribe(OnMessageReceived);

            Log.Info("--Subscriber ready--");
            Console.ReadLine();
        }

        private static void OnMessageReceived(Message message)
        {
            Log.Debug("** New General Event published, at: {0}, from: {1}", DateTime.Now, message.TransportedBy);

            var general = message.Body as GeneralEvent;
            Log.Debug("General Event with message: {0}", general.Message);
        }

        #endregion
    }
}
