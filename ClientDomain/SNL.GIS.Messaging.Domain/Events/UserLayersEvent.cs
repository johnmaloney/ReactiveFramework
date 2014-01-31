using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aetos.Messaging.Interfaces.Events;

namespace SNL.GIS.Messaging.Domain.Events
{
    public class UserLayersEvent : AEvent
    {
        public string Message { get; set; }
        public List<string> Layers { get; set; }

        public override string Details
        {
            get
            {
                return string.Format("User Layer(s) Initialized. Layer Count: {0}", this.Layers.Count.ToString());
            }
        }
    }
}
