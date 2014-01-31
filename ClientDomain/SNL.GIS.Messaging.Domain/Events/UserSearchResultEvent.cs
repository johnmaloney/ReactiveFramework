using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aetos.Messaging.Interfaces.Events;

namespace SNL.GIS.Messaging.Domain.Events
{
    public class UserSearchResultEvent : AEvent
    {
        public int ResultCount { get; set; }

        public int TotalResultsForSearch { get; set; }

        public List<dynamic> Results { get; set; }

        public override string Details
        {
            get { return string.Format("Search Results Event for Results Count:{0}", Results.Count.ToString()); }
        }
    }
}
