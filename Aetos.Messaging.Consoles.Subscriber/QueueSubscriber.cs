using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aetos.Messaging.Common.Logging;
using Aetos.Messaging.Domain.Clients;
using Aetos.Messaging.Handlers;
using Aetos.Messaging.Interfaces;

namespace Aetos.Messaging.Consoles.Subscriber
{
    public static class QueueSubscriber
    {
        #region Fields

        private static Random Random = new Random();
        private static IQueueClient QueueClient;

        #endregion

        #region Properties
        #endregion

        #region Methods

        public static void Run(string[] args)
        {
            string queueName;
            if (args.Length < 2)
            {
                Console.WriteLine("Queue Name?");
                queueName = Console.ReadLine();
            }
            else
            {
                queueName = args[1];
            }

            Console.Title = "Queue Subscriber: " + queueName;
            Subscribe(queueName);
        }
        
        private static void Subscribe(string queueName)
        {
            QueueClient = new QueueClient(queueName);
            QueueClient.Subscribe(OnMessageReceived);

            Log.Info("--Subscriber ready--");
            Console.ReadLine();
        }

        private static void OnMessageReceived(Message message)
        {
            Log.Debug("** Received message type: {0}, from: {1} at: {2}", message.MessageType, message.TransportedBy, DateTime.Now);
            try
            {
                var handler = MessageHandlerFactory.GetMessageHandler(message);
                handler.Handle(message);
                Log.Debug(string.Format("Handled message with handler type: {0}", handler.GetType()));
            }
            catch (Exception ex)
            {
                Log.Error("Host.OnMessageReceived, handler errored: {0}", ex.Message);
            }
        }

        #endregion
    }
}
