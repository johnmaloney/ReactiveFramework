using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aetos.Messaging.Interfaces
{
    public interface IQueueClient : IClient
    {
        string QueueName { get; }
        void Send(Message message);
        void DeleteQueue();

        IEnumerable<Message> PeekAtAllMessages();
    }
}
