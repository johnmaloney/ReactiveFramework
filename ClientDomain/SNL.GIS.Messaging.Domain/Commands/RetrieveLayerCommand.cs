using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNL.GIS.Messaging.Domain.Commands
{
    public class RetrieveLayerCommand
    {
        public Guid Identifier { get; set; }
        public string LayerId { get; set; }
    }
}
