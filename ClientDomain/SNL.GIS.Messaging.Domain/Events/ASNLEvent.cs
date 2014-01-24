using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNL.GIS.Messaging.Domain.Events
{
    public abstract class ASNLEvent
    {
        public virtual Guid InstanceId { get; set; }
        public virtual string Identifier { get; set; }
        public abstract string Details { get; }
    }
}
