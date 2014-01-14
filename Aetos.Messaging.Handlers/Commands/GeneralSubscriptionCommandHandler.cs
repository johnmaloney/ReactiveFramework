using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aetos.Messaging.Domain;
using Aetos.Messaging.Domain.Clients;
using Aetos.Messaging.Domain.Commands;
using Aetos.Messaging.Domain.Events;
using Aetos.Messaging.Interfaces;
using Aetos.Messaging.Interfaces.Commands;

namespace Aetos.Messaging.Handlers.Commands
{
    public class GeneralSubscriptionCommandHandler : IMessageHandler
    {
        private static ITopicClient _TopicClient = new TopicClient(Topic.GeneralSubscriptionEvent);

        public void Handle(Message message)
        {
            var generalSubscriptionCmd = message.Body as GeneralSubscriptionCommand;
            if ( generalSubscriptionCmd != null)
            {
                // Pause based on pause timer ///
                if (generalSubscriptionCmd.PauseTimeInSeconds > 0)
                    Thread.Sleep(generalSubscriptionCmd.PauseTimeInSeconds + 1000);

                var generalSubscriptionEvent = new Message
                {
                    Body = new GeneralSubscriptionEvent
                    {
                        SubscriptionName = generalSubscriptionCmd.SubscriptionName,
                        Title = string.Format(
                            "Received and Processed queue \r for subscription name: {0} \r paused processing for {1} seconds.", 
                            generalSubscriptionCmd.SubscriptionName, 
                            generalSubscriptionCmd.PauseTimeInSeconds.ToString())
                    }
                };

                _TopicClient.Publish(generalSubscriptionEvent);
            }
        }
    }
}
