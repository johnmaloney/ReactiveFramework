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
    public class RetrieveLayerHandler : IMessageHandler
    {
        private static ITopicClient TopicClient = new TopicClient(SNLTopic.LayerRetrievedEvent);

        private IDictionary<string, string> layersRespository;

        public void Handle(Message message)
        {
            if (layersRespository == null)
            {
                layersRespository = new Dictionary<string, string>();
                layersRespository.Add("points", "https://a.tiles.mapbox.com/v3/maloney1.h4am381i/markers.geojson");
                layersRespository.Add("shapes", "");
            }

            var layerRetrievalCommand = message.Body as RetrieveLayerCommand;

            if (layerRetrievalCommand != null)
            {
                string layer = string.Empty;
                if (layersRespository.ContainsKey(layerRetrievalCommand.LayerId))
                    layer = layersRespository[layerRetrievalCommand.LayerId];

                var layerEventMessage = new Message
                {
                    Body = new LayerRetrievedEvent
                    {
                        LayerId = layerRetrievalCommand.LayerId,
                        Layer = layer,
                        Identifier = layerRetrievalCommand.Identifier,
                        InstanceId = layerRetrievalCommand.InstanceId
                    }
                };

                TopicClient.Publish(layerEventMessage);
            }
        }
    }
}
