using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aetos.Messaging.Interfaces.Commands;

namespace SNL.GIS.Messaging.Domain.Commands
{
    public class AuthenticateUserCommand : ACommand
    {
        public string UserName { get; set; }
        public string AuthToken { get; set; }

        public override string Details
        {
            get { return string.Format("Authenticate User Command for username:{0}", UserName); }
        }
    }
}
