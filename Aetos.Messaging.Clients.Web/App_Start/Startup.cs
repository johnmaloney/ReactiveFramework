using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;

namespace Aetos.Messaging.Clients.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}