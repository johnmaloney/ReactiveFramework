using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aetos.Messaging.Common;
using Aetos.Messaging.Interfaces;

namespace Aetos.Messaging.Domain.Clients
{
    public class QueueClient : ClientBase<IQueueClient>, IQueueClient
    {
        #region Fields
        #endregion

        #region Properties

        public string QueueName { get; private set; }

        #endregion

        #region Methods

        public QueueClient(string queueName) : base()
        {
            QueueName = queueName;
        }

        public void Send(Message message)
        {
            ExecutePrimary(x => x.Send(message), x => ExecuteSecondary(y => y.Send(message)));
        }

        public void Subscribe(Action<Message> onMessageReceived)
        {
            _onMessageReceived = onMessageReceived;
            Subscribe();
        }

        public void Listen(Action<Message> onMessageReceived)
        {
            _onMessageReceived = onMessageReceived;
            Listen();
            
        }

        public void DeleteQueue()
        {
            ExecutePrimary(x => x.DeleteQueue());

            ExecuteSecondary(x => x.DeleteQueue());
        }

        public IEnumerable<Message> PeekAtAllMessages()
        {
            try
            {
                return ExecutePrimary<IEnumerable<Message>>(x =>
                {
                    return x.PeekAtAllMessages();
                });
            }
            catch
            {
                return ExecuteSecondary<IEnumerable<Message>>(x =>
                {
                    return x.PeekAtAllMessages();
                });
            }
        }

        protected override void LoadClientTypes(List<Type> clientTypes)
        {
            clientTypes.Clear();
            clientTypes.Add(Type.GetType(Config.GetSetting("PrimaryQueueClientType")));
            clientTypes.Add(Type.GetType(Config.GetSetting("SecondaryQueueClientType")));
        }

        protected override IQueueClient CreateInstance(Type type)
        {
            return (IQueueClient)Activator.CreateInstance(type, new object[] { QueueName });
        }

        #endregion
    }
}
