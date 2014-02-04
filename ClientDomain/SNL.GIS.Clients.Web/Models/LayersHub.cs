using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aetos.Messaging.Azure;
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
    [HubName("layersHub")]
    public class LayersHub : Hub
    {
        #region Fields

        private IQueueClient initialQueueClient = new QueueClient(SNLQueue.InitializeUserLayersCommand);
        private IQueueClient layerRetrievalClient = new QueueClient(SNLQueue.RetrieveLayerCommand);

        #endregion

        #region Methods

        public void InitializeLayers(string identifier)
        {

        }

        public void RetrieveLayer(string layerId)
        {
            var message = new Message()
            {
                Body = new RetrieveLayerCommand
                {
                    LayerId = layerId,
                    InstanceId = Guid.NewGuid(),
                    Identifier = Context.ConnectionId
                }
            };

            layerRetrievalClient.Send(message);
        }

        #endregion
    }

    public class UserLayersSubscriber : ASubscriber<LayersHub, UserLayersSubscriber>
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Methods

        private UserLayersSubscriber()
            :base(SNLTopic.UserLayersEvent)
        {

        }
        
        protected override void OnMessageReceived(Message message)
        {
            var userLayersEvent = message.Body as UserLayersEvent;

            var json = JsonConvert.SerializeObject(userLayersEvent);
            Clients.Client(userLayersEvent.Identifier).layersInitialized(json);
        }

        #endregion
    }

    public class LayersSubscriber : ASubscriber<LayersHub, LayersSubscriber>
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Methods

        private LayersSubscriber()
            :base(SNLTopic.LayerRetrievedEvent)
        {
        }

        protected override void OnMessageReceived(Message message)
        {
            var layersEvent = message.Body as LayerRetrievedEvent;
            
            var json = JsonConvert.SerializeObject(layersEvent);
            Clients.Client(layersEvent.Identifier).layerReceived(json);
        }

        #endregion
    }
}