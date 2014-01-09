﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Aetos.Messaging.Common.Logging;
using Aetos.Messaging.Domain;
using Aetos.Messaging.Domain.Clients;
using Aetos.Messaging.Domain.Commands;
using Aetos.Messaging.Interfaces;

namespace Aetos.Messaging.Consoles.Publisher
{
    public class QueuePublisher
    {
        #region Fields

        private static IQueueClient QueueClient;

        #endregion

        #region Properties
        #endregion

        #region Methods

        public static void Run(string cloud, string method)
        {
            SetupQueueClient(cloud);
            if (method == "s")
            {
                SendQueueMessage();
            }
            else
            {
                PublishQueueMessages();
            }
        }

        private static void SetupQueueClient(string cloud)
        {
            var queueClient = new QueueClient(Queue.GeneralCommand);
            switch(cloud)
            {
                case "p":
                    QueueClient = queueClient.Primary;
                    break;
                case "s":
                    QueueClient = queueClient.Secondary;
                    break;
                case "b":
                    QueueClient = queueClient;
                    break;
            }
            Console.WriteLine("Queue Publisher: general-message");
        }

        private static void SendQueueMessage()
        {
            Log.Info("--Queue Publisher Ready--");
            
            var cmd = Console.ReadLine();
            while (cmd != "x")
            {
                var message = new Message
                {
                    Body = new GeneralCommand
                    {
                        Title = "Update request: command: " + cmd
                    }
                };
                QueueClient.Send(message);
                cmd = Console.ReadLine();
            }
        }

        private static void  PublishQueueMessages()
        {
            Log.Info("--Queue Publisher Ready--");
            var timer = new Timer();
            timer.Interval = 500;
            timer.Elapsed += SendQueueMessage;
            Log.Info("--Starting to send--");
            timer.Start();
            Console.ReadLine();
        }

        protected static void SendQueueMessage(object sender, ElapsedEventArgs e)
        {
            var title = Program.GetRandomUpdate();
            var message = new Message
            {
                Body = new GeneralCommand
                {
                   Title = title
                }
            };

            QueueClient.Send(message);
            Log.Debug("--Sent GeneralCommand with title: " + title);
        }

        #endregion
    }
}
