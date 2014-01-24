using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNL.GIS.Messaging.Domain.Events
{
    public class UserSearchResultEvent
    {
        public string Identifier { get; set; }
        public List<string> Results { get; set; }
    }
}
