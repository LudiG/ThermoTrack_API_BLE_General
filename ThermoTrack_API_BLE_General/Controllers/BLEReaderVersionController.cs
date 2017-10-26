using System.Web.Http;

using ThermoTrack_API_BLE_General.Models;

namespace ThermoTrack_API_BLE_General.Controllers
{
    [RoutePrefix("api/blereaders/version/{idReader}")]
    public class BLEReaderVersionController : ApiController
    {
        // GET: api/blereaders/version/rpi_ble_scanner/x
        [HttpGet]
        [Route("rpi_ble_scanner")]
        public string GetRPi_BLE_Scanner(ulong idReader)
        {
            string version = "NULL";

            version = MySQLController.GetBLEReaderVersion(idReader, BLEReaderApplicationType.RPi_BLE_Scanner);

            return version;
        }

        // GET: api/blereaders/version/rpi_watchdog/x
        [HttpGet]
        [Route("rpi_watchdog")]
        public string GetRPi_WatchDog(ulong idReader)
        {
            string version = "NULL";

            version = MySQLController.GetBLEReaderVersion(idReader, BLEReaderApplicationType.RPi_WatchDog);

            return version;
        }

        // GET: api/blereaders/version/rpi_updater/x
        [HttpGet]
        [Route("rpi_updater")]
        public string GetRPi_Updater(ulong idReader)
        {
            string version = "NULL";

            version = MySQLController.GetBLEReaderVersion(idReader, BLEReaderApplicationType.RPi_Updater);

            return version;
        }

        // POST: api/blereaders/version/rpi_ble_scanner/x
        [HttpPost]
        [Route("rpi_ble_scanner")]
        public void PostRPi_BLE_Scanner(ulong idReader, [FromBody]BLEReaderVersionPacket body)
        {
            string version = body.Version;

            MySQLController.UpdateBLEReaderVersion(idReader, BLEReaderApplicationType.RPi_BLE_Scanner, version);
        }

        // POST: api/blereaders/version/rpi_watchdog/x
        [HttpPost]
        [Route("rpi_watchdog")]
        public void PostRPi_WatchDog(ulong idReader, [FromBody]BLEReaderVersionPacket body)
        {
            string version = body.Version;

            MySQLController.UpdateBLEReaderVersion(idReader, BLEReaderApplicationType.RPi_WatchDog, version);
        }

        // POST: api/blereaders/version/rpi_updater/x
        [HttpPost]
        [Route("rpi_updater")]
        public void PostRPi_Updater(ulong idReader, [FromBody]BLEReaderVersionPacket body)
        {
            string version = body.Version;

            MySQLController.UpdateBLEReaderVersion(idReader, BLEReaderApplicationType.RPi_Updater, version);
        }
    }
}