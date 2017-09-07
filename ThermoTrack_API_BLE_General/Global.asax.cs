using System.Web;
using System.Web.Http;

namespace ThermoTrack_API_BLE_General
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}