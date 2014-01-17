using System.Web;
using System.Web.Mvc;

namespace SNL.GIS.Services.WebQueue
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}