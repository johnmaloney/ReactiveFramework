﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNL.GIS.Messaging.Domain.Commands
{
    public class UserSearchCommand
    {
        public Guid Identifier { get; set; }
        public string SearchCriteria { get; set; }
    }
}
