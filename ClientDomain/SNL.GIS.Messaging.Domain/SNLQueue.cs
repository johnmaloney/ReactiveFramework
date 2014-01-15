using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNL.GIS.Messaging.Domain
{
    public struct SNLQueue
    {
        public const string AuthenticateUserCommand = "authenticate-user-command";
        public const string InitializeUserLayersCommand = "initialize-user-layers-command";
        public const string RetrieveLayerCommand = "retrieve-layer-command";
        public const string UserSearchCommand = "user-search-command";
    }
}
