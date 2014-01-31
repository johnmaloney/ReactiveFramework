using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aetos.Messaging.Interfaces.Events
{
    public abstract class AEvent
    {
        public Guid InstanceId { get; set; }
        public string Identifier { get; set; }
        public abstract string Details { get; }
    }
}
