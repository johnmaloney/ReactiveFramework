using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using Aetos.Messaging.Domain.Clients;
using Aetos.Messaging.Interfaces;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Aetos.Messaging.Web.Common
{
    /// <summary>
    /// Base class for subscribers, manages the creation of the 
    /// Singleton instance of the TSubscriber, assigns the THub's Clients to
    /// an internal collection and provides an abstract implementation of the 
    /// OnMessageReceieved delegate.
    /// </summary>
    /// <typeparam name="THub">SignalR Hub class</typeparam>
    /// <typeparam name="TSubscriber">Subscriber to Topics</typeparam>
    public abstract class ASubscriber<THub, TSubscriber>
        where TSubscriber : class
        where THub : Hub
    {
        #region Fields



        #endregion

        #region Properties

        public static TSubscriber Instance
        {
            get;
            set;
        }

        protected virtual string SubscriberName
        {
            get
            {
                return Dns.GetHostName();
            }
        }

        protected TopicClient TopicClient
        {
            get; 
            set;
        }
        
        protected IHubConnectionContext Clients 
        { 
            get; 
            set; 
        }

        #endregion

        #region Methods

        public ASubscriber(string topic)
        {
            Clients = GlobalHost.ConnectionManager.GetHubContext<THub>().Clients;
            
            this.TopicClient = new TopicClient(topic, this.SubscriberName);
            TopicClient.Subscribe(this.OnMessageReceived);

        }

        /// <summary>
        /// Delegate handler for when the TopicClient receives a broadcast message.
        /// </summary>
        /// <param name="message"></param>
        protected abstract void OnMessageReceived(Message message);

        /// <summary>
        /// Generates a singleton instance of the TSubscriber.
        /// </summary>
        public static void Start()
        {
            if (Instance == null)
                Instance = (TSubscriber)Activator.CreateInstance(
                    typeof(TSubscriber),
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    null,
                    null); 
        }

        /// <summary>
        /// Deconstructs the singleton instance of the TSubscriber.
        /// </summary>
        public static void Stop()
        {
            if (Instance != null)
            {
                var subscriber = Instance as ASubscriber<THub, TSubscriber>;
                if (subscriber != null)
                {
                    subscriber.StopInternal();
                    Instance = null;
                }
            }
        }

        protected void StopInternal()
        {
            TopicClient.DeleteSubscription();
            TopicClient.Unsubscribe();
            TopicClient = null;
        }

        #endregion
    }
}