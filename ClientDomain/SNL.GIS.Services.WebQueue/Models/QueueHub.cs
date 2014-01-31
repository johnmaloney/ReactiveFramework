using System;
using System.Collections.Generic;
using Aetos.Messaging.Domain.Clients;
using Aetos.Messaging.Handlers;
using Aetos.Messaging.Interfaces;
using Aetos.Messaging.Interfaces.Commands;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SNL.GIS.Messaging.Domain;
using SNL.GIS.Messaging.Domain.Commands;
using SNL.GIS.Messaging.Domain.MessageHandlers;
using SNL.GIS.Services.WebQueue.Models;

namespace SNL.GIS.Services.WebQueue
{
    /// <summary>
    /// SignalR : http://www.asp.net/signalr/overview/signalr-20/hubs-api/hubs-api-guide-server#hubmethods
    /// </summary>
    [HubName("queueHub")]
    public class QueueHub : Hub
    {
        #region Fields
        
        #endregion

        #region Methods

        public void ProcessQueueItem(string message)
        {
            dynamic container = JObject.Parse(message);
            var jsonObject = JObject.Parse(container.Body.ToString());
            var messageBody = jsonObject.ToObject(Type.GetType(container.MessageType.ToString()));

            var commandMessage = new Message
            {
                Body = messageBody,
                SequenceNumber = container.SequenceNumber
            };

            var handler = MessageHandlerFactory.GetMessageHandler(commandMessage);
            handler.Handle(commandMessage);

            QueueSubscriber.Instance.FinishedProcessingQueueItem(container.Identifier.ToString());
        }

        #endregion
    }

    public class QueueSubscriber
    {
        #region Fields

        private Dictionary<string, IQueueClient> queueClients;

        #endregion

        #region Properties

        private IHubConnectionContext Clients { get; set; }

        public static QueueSubscriber Instance { get; set; }

        public List<dynamic> CurrentQueuedCommands
        {
            get;
            set;
        }

        #endregion

        #region Methods

        private QueueSubscriber()
        {
            Clients = GlobalHost.ConnectionManager.GetHubContext<QueueHub>().Clients;
            CurrentQueuedCommands = new List<dynamic>();
   
            queueClients = new Dictionary<string, IQueueClient>();
            queueClients.Add(SNLQueue.AuthenticateUserCommand, new QueueClient(SNLQueue.AuthenticateUserCommand));
            queueClients.Add(SNLQueue.InitializeUserLayersCommand, new QueueClient(SNLQueue.InitializeUserLayersCommand));
            queueClients.Add(SNLQueue.RetrieveLayerCommand, new QueueClient(SNLQueue.RetrieveLayerCommand));
            queueClients.Add(SNLQueue.UserSearchCommand, new QueueClient(SNLQueue.UserSearchCommand));

            foreach (IQueueClient client in queueClients.Values)
            {
                client.Listen(onMessageReceived);
            }

            /// Sets up the Message handlers for this domain //
            MessageHandler.Initialize(BranchesRepository.AllBranches());
        }

        private void onMessageReceived(Message message)
        {
            var aCommand = message.Body as ACommand;

            dynamic item = new 
            { 
                Identifier = Guid.NewGuid().ToString(),
                Details =  aCommand != null ? aCommand.Details : "Unknown Body Type",
                MessageType = message.MessageType, 
                Body = message.Body, 
                SequenceNumber = message.SequenceNumber 
            };

            var json = JsonConvert.SerializeObject(item);
            CurrentQueuedCommands.Add(json);
            Clients.All.queueUpdated(json);
        }

        public void FinishedProcessingQueueItem(string identifier)
        {
            dynamic itemToClear = new
                {
                    Identifier = identifier
                };

            var json = JsonConvert.SerializeObject(itemToClear);
            Clients.All.clearProcessedItem(itemToClear);
        }

        public static void Start()
        {
            if (Instance == null)
                Instance = new QueueSubscriber();
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
            foreach (IQueueClient client in queueClients.Values)
            {
                client.DeleteQueue();
                client.Unsubscribe();
            }
            queueClients.Clear();
        }

        #endregion
    }
}