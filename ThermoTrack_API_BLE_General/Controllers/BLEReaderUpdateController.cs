using System.Web.Http;

using ThermoTrack_API_BLE_General.Models;

namespace ThermoTrack_API_BLE_General.Controllers
{
    [RoutePrefix("api/blereaders/update/{idReader}")]
    public class BLEReaderUpdateController : ApiController
    {
        // GET: api/blereaders/update/rpi_ble_scanner/x
        [HttpGet]
        [Route("rpi_ble_scanner")]
        public bool GetRPi_BLE_Scanner(ulong idReader)
        {
            bool update = false;

            update = MySQLController.GetBLEReaderUpdate(idReader, BLEReaderApplicationType.RPi_BLE_Scanner);

            return update;
        }

        // GET: api/blereaders/update/rpi_watchdog/x
        [HttpGet]
        [Route("rpi_watchdog")]
        public bool GetRPi_WatchDog(ulong idReader)
        {
            bool update = false;

            update = MySQLController.GetBLEReaderUpdate(idReader, BLEReaderApplicationType.RPi_WatchDog);

            return update;
        }

        // GET: api/blereaders/update/rpi_updater/x
        [HttpGet]
        [Route("rpi_updater")]
        public bool GetRPi_Updater(ulong idReader)
        {
            bool update = false;

            update = MySQLController.GetBLEReaderUpdate(idReader, BLEReaderApplicationType.RPi_Updater);

            return update;
        }

        // GET: api/blereaders/update/vpn/x
        [HttpGet]
        [Route("vpn")]
        public bool GetVPN(ulong idReader)
        {
            bool update = false;

            update = MySQLController.GetBLEReaderUpdateVPN(idReader);

            return update;
        }

        // POST: api/blereaders/update/rpi_ble_scanner
        [HttpPost]
        [Route("rpi_ble_scanner")]
        public void PostRPi_BLE_Scanner(ulong idReader, [FromBody]BLEReaderUpdatePacket body)
        {
            bool update = body.Update;

            MySQLController.UpdateBLEReaderUpdate(idReader, BLEReaderApplicationType.RPi_BLE_Scanner, update);
        }

        // POST: api/blereaders/update/rpi_watchdog
        [HttpPost]
        [Route("rpi_watchdog")]
        public void PostRPi_WatchDog(ulong idReader, [FromBody]BLEReaderUpdatePacket body)
        {
            bool update = body.Update;

            MySQLController.UpdateBLEReaderUpdate(idReader, BLEReaderApplicationType.RPi_WatchDog, update);
        }

        // POST: api/blereaders/update/rpi_updater
        [HttpPost]
        [Route("rpi_updater")]
        public void PostRPi_Updater(ulong idReader, [FromBody]BLEReaderUpdatePacket body)
        {
            bool update = body.Update;

            MySQLController.UpdateBLEReaderUpdate(idReader, BLEReaderApplicationType.RPi_Updater, update);
        }

        // POST: api/blereaders/update/vpn
        [HttpPost]
        [Route("vpn")]
        public void PostVPN(ulong idReader, [FromBody]BLEReaderUpdatePacket body)
        {
            bool update = body.Update;

            MySQLController.UpdateBLEReaderUpdateVPN(idReader, update);
        }
    }
}