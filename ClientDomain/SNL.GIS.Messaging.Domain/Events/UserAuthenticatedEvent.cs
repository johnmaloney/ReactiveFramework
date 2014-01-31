﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aetos.Messaging.Interfaces.Events;

namespace SNL.GIS.Messaging.Domain.Events
{
    public class UserAuthenticatedEvent : AEvent
    {
        public bool IsAuthenticated { get; set; }
        public string AuthToken { get; set; }

        public override string Details 
        { 
            get 
            { 
                return string.Format("User Authentication Request. IsAuthenticated: {0}", this.IsAuthenticated.ToString()); 
            } 
        }
    }
}
