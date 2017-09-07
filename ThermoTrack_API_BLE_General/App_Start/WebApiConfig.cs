using System.Web.Http;

namespace ThermoTrack_API_BLE_General
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services.

            // Web API routes.

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "BLEReaders",
                routeTemplate: "api/blereaders/{addressMAC}",
                defaults: new { controller = "blereader", addressMAC = RouteParameter.Optional }
            );
        }
    }
}