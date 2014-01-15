using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aetos.Messaging.Domain.Clients;
using Aetos.Messaging.Interfaces;
using Aetos.Messaging.Interfaces.Commands;
using SNL.GIS.Messaging.Domain.Commands;
using SNL.GIS.Messaging.Domain.Events;

namespace SNL.GIS.Messaging.Domain.MessageHandlers
{
    public class AuthenticateUserHandler : IMessageHandler
    {
        #region Fields

        private static ITopicClient TopicClient = new TopicClient(SNLTopic.UserAuthenticatedEvent);

        #endregion
        
        #region Methods

        public void Handle(Message message)
        {
            var authenticateCmd = message.Body as AuthenticateUserCommand;
            if (authenticateCmd != null)
            {
                var authenticationMessage = new Message
                {
                    Body = new UserAuthenticatedEvent
                    {
                        Identifier = authenticateCmd.Identifier,
                        InstanceId = authenticateCmd.InstanceId,
                        IsAuthenticated = true,
                        AuthToken = Guid.NewGuid().ToString()
                    }
                };

                TopicClient.Publish(authenticationMessage);
            }
        }

        #endregion
    }
}
