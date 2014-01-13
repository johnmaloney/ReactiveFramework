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
    public class QueueClient : IQueueClient
    {
        private azure.QueueClient _queueClient;
        public string QueueName { get; private set; }
        private Action<Message> _receiveAction;
        private bool _isListening;

        private static string ConnectionString
        {
            get { return Config.GetSetting("Microsoft.ServiceBus.ConnectionString"); }
        }

        public QueueClient(string queueName)
        {
            QueueName = queueName;
            Ensure();
            EnsureClient();
        }

        private void EnsureClient()
        {
            _queueClient = azure.QueueClient.CreateFromConnectionString(ConnectionString, QueueName);
        }

        public void Ensure()
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(ConnectionString);
            if (!namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.CreateQueue(QueueName);
            }
        }

        public void Send(Message message)
        {
            var brokeredMessage = AzureMessage.Wrap(message);
            _queueClient.Send(brokeredMessage);
        }

        public void Subscribe(Action<Message> onMessageReceived)
        {
            try
            {
                _receiveAction = onMessageReceived;
                EnsureClient();
                var options = new OnMessageOptions
                {
                    MaxConcurrentCalls = 10
                };
                _queueClient.OnMessage(OnMessageReceived, options);
                _isListening = true;
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
            _queueClient.Close();
            _queueClient = null;
        }

        private void OnMessageReceived(BrokeredMessage brokeredMessage)
        {
            if (_receiveAction != null)
            {
                _receiveAction(AzureMessage.Unwrap(brokeredMessage));
                brokeredMessage.Complete();
            }
        }

        public bool HasMessages()
        {
            var message = _queueClient.Peek();
            return (message != null);
        }

        public bool IsListening()
        {
            return _isListening;
        }

        public void DeleteQueue()
        {
            Unsubscribe();
            var namespaceManager = NamespaceManager.CreateFromConnectionString(ConnectionString);
            namespaceManager.DeleteQueue(QueueName);
        }
    }
}
