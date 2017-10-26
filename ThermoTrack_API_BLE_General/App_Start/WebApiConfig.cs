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
                name: "BLEReadersVPN",
                routeTemplate: "api/blereaders/vpn/{idReader}",
                defaults: new { controller = "BLEReader", idReader = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "BLEReadersVersion",
                routeTemplate: "api/blereaders/version/{idReader}",
                defaults: new { controller = "BLEReaderVersion", idReader = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "BLEReadersUpdate",
                routeTemplate: "api/blereaders/update/{idReader}",
                defaults: new { controller = "BLEReaderUpdate", idReader = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "BLEReadersDefault",
                routeTemplate: "api/blereaders/{addressMAC}",
                defaults: new { controller = "BLEReader", addressMAC = RouteParameter.Optional }
            );
        }
    }
}