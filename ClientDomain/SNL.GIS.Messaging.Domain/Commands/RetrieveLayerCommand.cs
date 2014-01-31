using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aetos.Messaging.Interfaces.Commands;

namespace SNL.GIS.Messaging.Domain.Commands
{
    public class RetrieveLayerCommand : ACommand
    {
        public string LayerId { get; set; }

        public override string Details
        {
            get { return string.Format("Retrieve Layer(s) Command with Layer Id:{0}", LayerId); }
        }
    }
}
