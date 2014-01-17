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
    /// <summary>
    /// Windows Azure Service Bus Client 
    /// http://msdn.microsoft.com/en-us/library/windowsazure/ee732537.aspx
    /// </summary>
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

        /// <summary>
        /// Handles the entry of a new command in the Queue, will remove 
        /// the command from the queue.
        /// </summary>
        /// <param name="onMessageReceived"></param>
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

        private void OnMessageReceived(BrokeredMessage brokeredMessage)
        {
            if (_receiveAction != null)
            {
                _receiveAction(AzureMessage.Unwrap(brokeredMessage));
                brokeredMessage.Complete();
            }
        }

        /// <summary>
        /// This method is different than Subscribe in that it only listens
        /// for new queued entries, it will not remove them from the Queue.
        /// </summary>
        /// <param name="onMessageReceived"></param>
        public void Listen(Action<Message> onMessageReceived)
        {
            try
            {
                _receiveAction = onMessageReceived;
                EnsureClient();
                var options = new OnMessageOptions
                {
                    MaxConcurrentCalls = 10
                };
                _queueClient.OnMessage(OnMessageReceivedLeaveInQueue, options);
                _isListening = true;
            }
            catch
            {
                _isListening = false;
                throw;
            }
        }

        private void OnMessageReceivedLeaveInQueue(BrokeredMessage brokeredMessage)
        {
            if (_receiveAction != null)
            {
                _receiveAction(AzureMessage.Unwrap(brokeredMessage));
                brokeredMessage.Abandon();
            }
        }

        public void Unsubscribe()
        {
            _isListening = false;
            _queueClient.Close();
            _queueClient = null;
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

        public IEnumerable<Message> PeekAtAllMessages()
        {
            var messagesInQueue = new List<Message>();
            BrokeredMessage currentMessage = null;
            do
            {
                /// The Peek method will take the 1st item off the Queue //
                /// Subsequent calls to it actually move through the list //
                /// as long as the _queueClient is a static variable //
                /// http://stackoverflow.com/questions/18520714/azure-service-bus-queue-peekbatch-locking
                currentMessage = _queueClient.Peek();
                
                if (currentMessage != null)
                    messagesInQueue.Add(AzureMessage.Unwrap(currentMessage));

            } while (currentMessage != null);

            return messagesInQueue;
        }

        public void DeleteQueue()
        {
            Unsubscribe();
            var namespaceManager = NamespaceManager.CreateFromConnectionString(ConnectionString);
            namespaceManager.DeleteQueue(QueueName);
        }
    }
}
