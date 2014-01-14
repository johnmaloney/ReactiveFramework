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
            string topicName;
            if (args.Length < 3)
            {
                Console.WriteLine("Subscription name?");
                subName = Console.ReadLine();
                Console.WriteLine("Topic name?");
                topicName = Console.ReadLine();
            }
            else
            {
                subName = args[1];
                topicName = args[2];
            }
            Console.Title = "Topic subscriber: " + subName;
            Subscribe(subName, topicName);
        }

        private static void Subscribe(string subName, string topicName)
        {
            if (string.IsNullOrEmpty(topicName))
                topicName = Topic.GeneralEvent;

            TopicClient = new TopicClient(topicName, subName);
            TopicClient.Subscribe(OnMessageReceived);

            Log.Info("--Subscriber ready--");
            Console.ReadLine();
        }

        private static void OnMessageReceived(Message message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Log.Debug("** New General Event published, at: {0}, from: {1}", DateTime.Now, message.TransportedBy);
            Console.ForegroundColor = ConsoleColor.Green;

            if (message.Body is GeneralEvent)
            {
                var general = message.Body as GeneralEvent;
                Log.Debug("*** General Event with message: {0}", general.Message);
            }
            else if (message.Body is GeneralSubscriptionEvent)
            {
                var generalSubscriptionEvent = message.Body as GeneralSubscriptionEvent;
                if (generalSubscriptionEvent != null && generalSubscriptionEvent.SubscriptionName == TopicClient.SubscriptionName)
                    Log.Debug("**** General event for subscription name: {0} received. Title: {1}",
                        generalSubscriptionEvent.SubscriptionName, generalSubscriptionEvent.Title);
                else
                    Log.Error("**** General event for subscription name: {0} recieved Title: {1}, {2}but does not match the this Subscription name.",
                        generalSubscriptionEvent.SubscriptionName, generalSubscriptionEvent.Title, Environment.NewLine);
            }
            else
                Log.Debug("**** General Event was received but type could not be determined.");
        }

        #endregion
    }
}
