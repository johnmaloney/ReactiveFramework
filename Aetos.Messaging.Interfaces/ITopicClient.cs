﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aetos.Messaging.Interfaces
{
    public interface ITopicClient : IClient
    {
        void Publish(Message message);
        string SubscriptionName { get; }
        string TopicName { get; }
        void DeleteSubscription();
    }
}
