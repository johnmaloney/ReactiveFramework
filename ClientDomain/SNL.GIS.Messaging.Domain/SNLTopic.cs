using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNL.GIS.Messaging.Domain
{
    public struct SNLTopic
    {
        public const string LayerRetrievedEvent = "layer-retrieved-event";
        public const string UserAuthenticatedEvent = "user-authenticated-event";
        public const string UserLayersEvent = "user-layers-event";
        public const string UserSearchResultEvent = "user-search-result-event";
    }
}
