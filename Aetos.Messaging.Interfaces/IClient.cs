using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aetos.Messaging.Interfaces
{
    public interface IClient
    {
        void Subscribe(Action<Message> onMessageReceived);
        void Listen(Action<Message> onMessageReceived);
        void Unsubscribe();
        bool HasMessages();
        bool IsListening();
    }
}
