using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aetos.Messaging.Common;
using Aetos.Messaging.Interfaces;

namespace Aetos.Messaging.Domain.Clients
{
    public class TopicClient : ClientBase<ITopicClient>, ITopicClient
    {
        #region Fields

        #endregion

        #region Properties

        public string TopicName { get; private set; }
        public string SubscriptionName { get; private set; }

        #endregion

        #region Methods

        public TopicClient(string topicName) : this(topicName, null) { }

        public TopicClient(string topicName, string subscriptionName)
            : base()
        {
            TopicName = topicName;
            SubscriptionName = subscriptionName;
        }

        public void Publish(Message message)
        {           
            ExecutePrimary(x => x.Publish(message), x => ExecuteSecondary(y => y.Publish(message)));
        }

        public void Subscribe(Action<Message> onMessageReceived)
        {
            _onMessageReceived = onMessageReceived;
            Subscribe();
        }

        public void Listen(Action<Message> onMessageReceived)
        {
            /// This can be supported for the TopicClient but there is not an immediate need for it //
            throw new NotImplementedException("Listening is not supported on the TopicClient, See QueueClient for implementation details.");
        }

        public void DeleteSubscription()
        {
            ExecutePrimary(x => x.DeleteSubscription());
            ExecuteSecondary(y => y.DeleteSubscription());
        }

        protected override void LoadClientTypes(List<Type> clientTypes)
        {
            clientTypes.Clear();
            clientTypes.Add(Type.GetType(Config.GetSetting("PrimaryTopicClientType")));
            clientTypes.Add(Type.GetType(Config.GetSetting("SecondaryTopicClientType")));
        }

        protected override ITopicClient CreateInstance(Type type)
        {
            return (ITopicClient)Activator.CreateInstance(type, new object[] { TopicName, SubscriptionName });
        }

        #endregion
    }
}
