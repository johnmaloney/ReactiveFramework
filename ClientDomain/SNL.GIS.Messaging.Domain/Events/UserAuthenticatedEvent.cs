using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNL.GIS.Messaging.Domain.Events
{
    public class UserAuthenticatedEvent
    {
        public Guid InstanceId { get; set; }
        public string Identifier { get; set; }
        public bool IsAuthenticated { get; set; }
        public string AuthToken { get; set; }
    }
}
