using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aetos.Messaging.Domain.Clients;
using Aetos.Messaging.Interfaces;
using Aetos.Messaging.Interfaces.Commands;
using SNL.GIS.Messaging.Domain.Commands;
using SNL.GIS.Messaging.Domain.Events;

namespace SNL.GIS.Messaging.Domain.MessageHandlers
{
    public class InitializeUserHandler : IMessageHandler
    {
        private static ITopicClient TopicClient = new TopicClient(SNLTopic.UserLayersEvent);

        public void Handle(Message message)
        {
            var initializeUserCmd = message.Body as InitializeUserCommand;
            if (initializeUserCmd != null)
            {
                var userLayerMessage = new Message
                {
                    Body = new UserLayersEvent
                    {
                        Identifier = initializeUserCmd.Identifier,
                        Message = "Layers retrieved for user.",
                        Layers = new List<string>
                        {
                            "http://www.openstreetmap.org/?minlon=22.3418234&minlat=57.5129102&maxlon=22.5739625&maxlat=57.6287332#map=8/57.467/21.891&layers=T", 
                            "http://www.openstreetmap.org/?minlon=22.3418234&minlat=57.5129102&maxlon=22.5739625&maxlat=57.6287332#map=8/57.467/21.891&layers=Q", 
                            "http://www.openstreetmap.org/way/223131374"
                        }
                    }
                };
                
                TopicClient.Publish(userLayerMessage);
            }
        }
    }
}
