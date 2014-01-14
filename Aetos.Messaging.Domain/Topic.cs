using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aetos.Messaging.Domain
{
    public struct Topic
    {
        public const string GeneralEvent = "general-event"; // azure requires lowercase
        public const string GeneralSubscriptionEvent = "general-subscription-event";
    }
}
