using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aetos.Messaging.Domain;
using Aetos.Messaging.Domain.Clients;
using Aetos.Messaging.Domain.Commands;
using Aetos.Messaging.Domain.Events;
using Aetos.Messaging.Interfaces;
using Aetos.Messaging.Interfaces.Commands;

namespace Aetos.Messaging.Handlers.Commands
{
    public class GeneralCommandHandler : IMessageHandler
    {
        private static ITopicClient _TopicClient = new TopicClient(Topic.GeneralEvent);

        public void Handle(Message message)
        {
            var generalCommand = message.Body as GeneralCommand;
            if (generalCommand != null)
            {
                // Store the title 
                var generalEvent = new Message
                {
                    Body = new GeneralEvent() { Message = generalCommand.Title }
                };

                _TopicClient.Publish(generalEvent);
            }
        }
    }
}
