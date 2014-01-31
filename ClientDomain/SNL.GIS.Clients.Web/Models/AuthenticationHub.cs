using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Aetos.Messaging.Domain;
using Aetos.Messaging.Domain.Clients;
using Aetos.Messaging.Interfaces;
using Aetos.Messaging.Web.Common;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using SNL.GIS.Messaging.Domain;
using SNL.GIS.Messaging.Domain.Commands;
using SNL.GIS.Messaging.Domain.Events;

namespace SNL.GIS.Clients.Web.Models
{
    [HubName("authenticationHub")]
    public class AuthenticationHub : Hub
    {
        #region Fields

        private IQueueClient queueClient = new QueueClient(SNLQueue.AuthenticateUserCommand);

        #endregion

        #region Methods
        
        public void Login(string identifier)
        {
            var message = new Message()
            {
                Body = new AuthenticateUserCommand()
                {
                    InstanceId = Guid.NewGuid(),
                    Identifier = Context.ConnectionId
                }
            };

            queueClient.Send(message);
        }

        #endregion
    }

    public class AuthenticationSubscriber : ASubscriber<AuthenticationHub, AuthenticationSubscriber>
    {
        #region Fields

        /// <summary>
        /// This client will be used to react to the Authentication Event and send a initialize command //
        /// </summary>
        private IQueueClient initializationQueueClient = new QueueClient(SNLQueue.InitializeUserLayersCommand);

        #endregion

        #region Properties
        #endregion

        #region Methods

        private AuthenticationSubscriber() 
            : base(SNLTopic.UserAuthenticatedEvent)
        {
        }

        protected override void OnMessageReceived(Message message)
        {
            var authEvent = message.Body as UserAuthenticatedEvent;

            var json = JsonConvert.SerializeObject(authEvent);
            Clients.Client(authEvent.Identifier).authenticated(json);

            // READ ME - Once the AuthenticationTopic is received we want to also process the initialize Command ///
            var initializeMessage = new Message
            {
                Body = new InitializeUserCommand
                {
                    InstanceId = Guid.NewGuid(),
                    Identifier = authEvent.Identifier
                }
            };

            this.initializationQueueClient.Send(initializeMessage);

        }

        #endregion
    }
}