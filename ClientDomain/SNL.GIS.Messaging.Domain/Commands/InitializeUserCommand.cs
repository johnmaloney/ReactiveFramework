using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aetos.Messaging.Interfaces.Commands;

namespace SNL.GIS.Messaging.Domain.Commands
{
    public class InitializeUserCommand : ACommand
    {
        public string UserIdentification { get; set; }

        public override string Details
        {
            get { return string.Format("Initialize User Command for userIdentifier:{0}", UserIdentification); }
        }
    }
}
