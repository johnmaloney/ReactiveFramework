using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Aetos.Messaging.Domain;
using Aetos.Messaging.Domain.Clients;
using Aetos.Messaging.Interfaces;
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

    public class AuthenticationSubscriber
    {
        #region Fields

        private TopicClient topicClient;

        #endregion

        #region Properties

        public static AuthenticationSubscriber Instance { get; set; }
        private IHubConnectionContext Clients { get; set; }

        #endregion

        #region Methods

        private AuthenticationSubscriber()
        {
            Clients = GlobalHost.ConnectionManager.GetHubContext<AuthenticationHub>().Clients;
            var subscriberName = Dns.GetHostName();
            topicClient = new TopicClient(SNLTopic.UserAuthenticatedEvent, subscriberName);
            topicClient.Subscribe(OnMessageReceived);
        }

        private void OnMessageReceived(Message message)
        {
            var authEvent = message.Body as UserAuthenticatedEvent;

            var json = JsonConvert.SerializeObject(authEvent);
            Clients.Client(authEvent.Identifier).authenticated(json);
        }

        public static void Start()
        {
            if (Instance == null)
                Instance = new AuthenticationSubscriber();
        }

        public static void Stop()
        {
            if (Instance != null)
            {
                Instance.StopInternal();
                Instance = null;
            }
        }

        private void StopInternal()
        {
            topicClient.DeleteSubscription();
            topicClient.Unsubscribe();
            topicClient = null;
        }

        #endregion
    }
}