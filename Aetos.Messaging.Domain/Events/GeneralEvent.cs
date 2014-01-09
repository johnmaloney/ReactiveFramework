using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aetos.Messaging.Domain.Events
{
    /// <summary>
    /// Represents an event publication.
    /// </summary>
    public class GeneralEvent
    {
        public string Message { get; set; }
        public string RoutedBy { get; set; }
        public string PublishedBy { get; set; }
        public string TransportedBy { get; set; }
        public string StoredBy { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
