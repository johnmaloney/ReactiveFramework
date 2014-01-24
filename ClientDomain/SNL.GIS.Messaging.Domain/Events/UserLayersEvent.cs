using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNL.GIS.Messaging.Domain.Events
{
    public class UserLayersEvent
    {
        public string Identifier { get; set; }
        public string Message { get; set; }
        public List<string> Layers { get; set; }
    }
}
