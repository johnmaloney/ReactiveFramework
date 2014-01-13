using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aetos.Messaging.Handlers.Commands;
using Aetos.Messaging.Interfaces;
using Aetos.Messaging.Interfaces.Commands;
using Aetos.Messaging.Common.Extensions;
using Aetos.Messaging.Domain.Events;
using Aetos.Messaging.Domain.Commands;

namespace Aetos.Messaging.Handlers
{
    public static class MessageHandlerFactory
    {
        private static Dictionary<string, IMessageHandler> _Handlers;

        static MessageHandlerFactory()
        {
            _Handlers = new Dictionary<string, IMessageHandler>();
            _Handlers.Add(typeof(GeneralCommand).GetMessageType(), new GeneralCommandHandler());
        }

        public static IMessageHandler GetMessageHandler(Message message)
        {
            return _Handlers[message.Body.GetMessageType()];
        }
    }
}
