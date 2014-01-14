using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aetos.Messaging.Domain.Commands
{
    public class GeneralSubscriptionCommand
    {
        public string Title { get; set; }
        public int PauseTimeInSeconds { get; set; }
        public string SubscriptionName { get; set; }
    }
}
