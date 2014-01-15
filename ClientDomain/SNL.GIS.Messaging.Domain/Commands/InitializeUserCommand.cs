using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNL.GIS.Messaging.Domain.Commands
{
    public class InitializeUserCommand
    {
        public Guid Identifier { get; set; }
        public string UserIdentification { get; set; }
    }
}
