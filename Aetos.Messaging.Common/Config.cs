using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aetos.Messaging.Common
{
    public static class Config
    {
        public static string GetSetting(string name)
        {
            var value = ConfigurationManager.AppSettings[name];

            try
            {
                //if (RoleEnvironment.IsAvailable)
                //    value = CloudConfigurationManager.GetSettings(name);
            }
            catch (Exception)
            {
                // Do nothing - outside of Azure & the Emulator using the AppSettings is correct.
            }
            return value;
        }
    }
}
