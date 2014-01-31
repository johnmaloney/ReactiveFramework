using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aetos.Messaging.Interfaces.Events;

namespace SNL.GIS.Messaging.Domain.Events
{
    public class LayerRetrievedEvent : AEvent
    {
        public string LayerId { get; set; }
        public string Layer { get; set; }

        public override string Details
        {
            get { return string.Format("Layer retrieved Event for LayerId:{0}", LayerId); }
        }
    }
}
