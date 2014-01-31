using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Aetos.Messaging.Domain.Clients;
using Aetos.Messaging.Interfaces;
using Aetos.Messaging.Interfaces.Commands;
using SNL.GIS.Messaging.Domain.Commands;
using SNL.GIS.Messaging.Domain.Events;

namespace SNL.GIS.Messaging.Domain.MessageHandlers
{
    public class UserSearchHandler : IMessageHandler
    {
        private List<dynamic> SearchableList;

        private static ITopicClient TopicClient = new TopicClient(SNLTopic.UserSearchResultEvent);

        public void Handle(Message message)
        {
            var searchCommand = message.Body as UserSearchCommand;
            if (searchCommand != null)
            {
                var regex = new Regex(@"\b\w*z+\w*\b|" + searchCommand.SearchCriteria.ToLower());
                var results = SearchableList
                        .Where(b => 
                        {
                            if (b.description != null && !string.IsNullOrEmpty(b.description.ToString()))
                            {
                                if (regex.IsMatch(b.description.ToString().ToLower()))
                                    return true;
                            }
                            return false;
                        })
                        .Take(searchCommand.ResultCountDesired)
                        .ToList();

                var searchResultEvent = new Message
                {
                    Body = new UserSearchResultEvent
                    {
                        Identifier = searchCommand.Identifier,
                        ResultCount = searchCommand.ResultCountDesired, 
                        TotalResultsForSearch = results.Count(),
                        Results = results
                    }
                };

                TopicClient.Publish(searchResultEvent);
            }
        }

        public UserSearchHandler(List<dynamic> searchableList)
        {
            this.SearchableList = searchableList;
        }

        public UserSearchHandler()
        {
            this.SearchableList = new List<dynamic>();
        }
    }
}
