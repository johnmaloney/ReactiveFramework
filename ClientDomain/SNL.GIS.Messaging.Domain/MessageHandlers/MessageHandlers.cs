using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aetos.Messaging.Handlers;
using SNL.GIS.Messaging.Domain.Commands;
using SNL.GIS.Messaging.Domain.MessageHandlers;

namespace SNL.GIS.Messaging.Domain.MessageHandlers
{
    public static class MessageHandler
    {
        public static void Initialize()
        {
            MessageHandlerFactory.AddOrReplace(typeof(AuthenticateUserCommand), new AuthenticateUserHandler());
            MessageHandlerFactory.AddOrReplace(typeof(InitializeUserCommand), new InitializeUserHandler());
            MessageHandlerFactory.AddOrReplace(typeof(RetrieveLayerCommand), new RetrieveLayerHandler());
            MessageHandlerFactory.AddOrReplace(typeof(UserSearchCommand), new UserSearchHandler());
        }
    }
}
