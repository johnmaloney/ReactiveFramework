using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Aetos.Messaging.Common.Extensions;

namespace Aetos.Messaging.Interfaces
{
    public class Message
    {
        public string MessageType { get; set; }

        public string ReplyTo { get; set; }

        public long SequenceNumber { get; set; }

        private object _body;

        public object Body
        {
            get { return _body; }
            set
            {
                //if deserializing, set the body as a typed object:
                var jBody = value as JObject;
                if (jBody != null)
                {
                    _body = jBody.ToObject(Type.GetType(MessageType));
                }
                else
                {
                    _body = value;
                    if (_body != null)
                    {
                        MessageType = _body.GetMessageType();
                    }
                }
            }
        }

        public string TransportedBy { get; set; }
    }
}
