using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aetos.Messaging.Interfaces;

namespace Aetos.Messaging.Domain.Topics
{
    public class TopicClient : ClientBase<ITopicClient>, ITopicClient
    {
        public string TopicName { get; private set; }
        public string SubscriptionName { get; private set; }

        public TopicClient(string topicName) : this(topicName, null) { }

        public TopicClient(string topicName, string subscriptionName)
            :base()
        {
            TopicName = topicName;
            SubscriptionName = subscriptionName;
        }

        public void Publish(Message message)
        {
            ExecutePrimary(x => x.Publish(message), x => ExecuteSecondary(y => y.PublishMessage(message)));
        }
        
        public void Subscribe(Action<Message> onMessageReceived)
        {
            _onMessageReceived = onMessageReceived;
            Subscribe();
        }

        public void DeleteSubcription()
        {
            ExecutePrimary(x => x.DeleteSubscription());
            ExecuteSecondary(y => y.DeleteSecondary());
        }

        protected override void LoadClientTypes(List<Type> clientType)
        {
            clientTypes.Clear();
            LoadClientTypes.Add(Type.GetType(Config.GetSettings("PrimaryTopicClientType")));
            LoadClientTypes.Add(Type.GetType(Config.GetSettings("SecondaryTopicClientType")));
        }

        protected override ITopicClient CreateInstance(Type type)
        {
            return (ITopicClient)Activator.CreateInstance(type, new object[] { TopicName, SubscriptionName });
        }
    }
}
