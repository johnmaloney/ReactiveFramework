using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNL.GIS.Messaging.Domain.Events
{
    public class LayerRetrievedEvent
    {
        public string Identifier { get; set; }
        public string LayerId { get; set; }
        public string Layer { get; set; }
    }
}
