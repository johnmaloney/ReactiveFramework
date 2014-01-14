using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aetos.Messaging.Domain.Events
{
    public class GeneralSubscriptionEvent
    {
        public string Title { get; set; }
        public string SubscriptionName { get; set; }
    }
}
