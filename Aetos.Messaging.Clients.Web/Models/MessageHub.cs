using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Aetos.Messaging.Domain;
using Aetos.Messaging.Domain.Clients;
using Aetos.Messaging.Domain.Events;
using Aetos.Messaging.Interfaces;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;

namespace Aetos.Messaging.Clients.Web
{
    [HubName("messageHub")]
    public class MessageHub : Hub
    {

    }

    public class MessageSubscriber
    {
        #region Fields

        private TopicClient topicClient;

        #endregion

        #region Properties

        public static MessageSubscriber Instance { get; set; }
        private IHubConnectionContext Clients { get; set; }

        #endregion

        #region Methods

        public static void Start()
        {
            if (Instance == null)
                Instance = new MessageSubscriber();
        }

        public static void Stop()
        {
            if(Instance != null)
            {
                Instance.StopInternal();
                Instance = null;
            }
        }

        private MessageSubscriber()
        {
            Clients = GlobalHost.ConnectionManager.GetHubContext<MessageHub>().Clients;
            var subscriberName = Dns.GetHostName();
            topicClient = new TopicClient(Topic.GeneralEvent, subscriberName);
            topicClient.Subscribe(OnMessageReceived);
        }

        private void OnMessageReceived(Message message)
        {
            var generalEvent = message.Body as GeneralEvent;
            generalEvent.PublishedBy = Environment.MachineName +" - via " + message.TransportedBy;

            var json = JsonConvert.SerializeObject(generalEvent);
            Clients.All.newMessageEvent(json);
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