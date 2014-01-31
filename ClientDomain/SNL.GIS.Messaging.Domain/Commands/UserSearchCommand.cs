using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aetos.Messaging.Interfaces.Commands;

namespace SNL.GIS.Messaging.Domain.Commands
{
    public class UserSearchCommand : ACommand
    {
        public string SearchCriteria { get; set; }

        public int ResultCountDesired { get; set; }

        public override string Details
        {
            get { return string.Format("User Search Command with search criteria:{0}", SearchCriteria); }
        }
    }
}
