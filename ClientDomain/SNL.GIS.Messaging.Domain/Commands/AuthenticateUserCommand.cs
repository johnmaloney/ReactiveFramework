using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNL.GIS.Messaging.Domain.Commands
{
    public class AuthenticateUserCommand
    {
        public Guid Identifier { get; set; }
        public string UserName { get; set; }
        public string AuthToken { get; set; }
    }
}
