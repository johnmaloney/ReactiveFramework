using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aetos.Messaging.Common;
using Aetos.Messaging.Interfaces;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using azure = Microsoft.ServiceBus.Messaging;

namespace Aetos.Messaging.Azure
{
    public class TopicClient : ITopicClient
    {
        private azure.TopicClient _topicClient;
        private azure.SubscriptionClient _subscriptionClient;
        public string TopicName { get; private set; }
        public string SubscriptionName { get; private set; }
        private Action<Message> _receiveAction;
        private bool _isListening;

        private static string ConnectionString
        {
            get { return Config.GetSetting("Microsoft.ServiceBus.ConnectionString"); }
        }

        public TopicClient(string topicName) : this(topicName, null) { }

        public TopicClient(string topicName, string subscriptionName)
        {
            TopicName = topicName;
            SubscriptionName = subscriptionName;
            Ensure();
        }

        public void Ensure()
        {
            EnsureTopic();
            _topicClient = azure.TopicClient.CreateFromConnectionString(ConnectionString, TopicName);
            if (!string.IsNullOrEmpty(SubscriptionName))
            {
                EnsureSubscription();
                EnsureClient();
            }
        }

        private void EnsureClient()
        {
            if (_subscriptionClient == null || _subscriptionClient.IsClosed)
            {
                _subscriptionClient = azure.SubscriptionClient.CreateFromConnectionString(ConnectionString, TopicName, SubscriptionName);
            }
        }

        public void Publish(Message message)
        {
            var brokeredMessage = AzureMessage.Wrap(message);
            _topicClient.Send(brokeredMessage);
        }

        public void Subscribe(Action<Message> onMessageReceived)
        {
            try
            {
                _isListening = true;
                _receiveAction = onMessageReceived;
                EnsureClient();
                var options = new OnMessageOptions
                {
                    MaxConcurrentCalls = 10
                };
                _subscriptionClient.OnMessage(OnMessageReceived, options);
            }
            catch
            {
                _isListening = false;
                throw;
            }
        }

        public void Unsubscribe()
        {
            _isListening = false;
            _subscriptionClient.Close();
            _subscriptionClient = null;
        }

        public bool IsListening()
        {
            return _isListening;
        }

        public bool HasMessages()
        {
            EnsureClient();
            var message = _subscriptionClient.Peek();
            return (message != null);
        }

        public void DeleteSubscription()
        {
            Unsubscribe();
            var namespaceManager = NamespaceManager.CreateFromConnectionString(ConnectionString);
            namespaceManager.DeleteSubscription(TopicName, SubscriptionName);
        }

        private void OnMessageReceived(BrokeredMessage brokeredMessage)
        {
            if (_receiveAction != null)
            {
                try
                {
                    _receiveAction(AzureMessage.Unwrap(brokeredMessage));
                    brokeredMessage.Complete();
                }
                catch
                {
                    throw;
                }
            }
        }

        private void EnsureTopic()
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(ConnectionString);
            if (!namespaceManager.TopicExists(TopicName))
            {
                namespaceManager.CreateTopic(TopicName);
            }
        }

        private void EnsureSubscription()
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(ConnectionString);
            if (!namespaceManager.SubscriptionExists(TopicName, SubscriptionName))
            {
                namespaceManager.CreateSubscription(TopicName, SubscriptionName);
            }
        }
    }
}
