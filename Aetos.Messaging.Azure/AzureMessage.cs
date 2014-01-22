using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aetos.Messaging.Interfaces;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace Aetos.Messaging.Azure
{
    public class AzureMessage
    {
        public static Message Unwrap(BrokeredMessage brokeredMessage)
        {
            var raw = brokeredMessage.GetBody<string>();
            var message = JsonConvert.DeserializeObject<Message>(raw);
            message.TransportedBy = "Azure";
            message.SequenceNumber = brokeredMessage.SequenceNumber;
            return message;
        }

        public static BrokeredMessage Wrap(Message message)
        {
            var raw = JsonConvert.SerializeObject(message);
            return new BrokeredMessage(raw);
        }
    }
}
