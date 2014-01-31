using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aetos.Messaging.Domain.Clients;
using Aetos.Messaging.Interfaces;
using Aetos.Messaging.Web.Common;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using SNL.GIS.Messaging.Domain;
using SNL.GIS.Messaging.Domain.Events;

namespace SNL.GIS.Clients.Web.Models
{
    [HubName("searchHub")]
    public class SearchHub : Hub
    {
        #region Fields

        private IQueueClient searchQueueClient = new QueueClient(SNLQueue.InitializeUserLayersCommand);

        #endregion

        #region Properties
        #endregion

        #region Methods
        #endregion
    }

    public class SearchSubscriber : ASubscriber<SearchHub, SearchSubscriber>
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Methods

        private SearchSubscriber()
            :base(SNLTopic.UserSearchResultEvent)
        {

        }
          
        protected override void OnMessageReceived(Message message)
        {
            var searchEvent = message.Body as UserSearchResultEvent;

            var json = JsonConvert.SerializeObject(searchEvent);
            Clients.Client(searchEvent.Identifier).searchResultsReceived(json);
        }

        #endregion
    }
}