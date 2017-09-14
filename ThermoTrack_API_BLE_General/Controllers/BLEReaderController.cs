using System.Web.Http;

using ThermoTrack_API_BLE_General.Models;

namespace ThermoTrack_API_BLE_General.Controllers
{
    public class BLEReaderController : ApiController
    {
        // GET: api/BLEReader/xxxxxxxxxxxx
        public ulong Get(string addressMAC)
        {
            ulong idReader = 0;

            idReader = MySQLController.GetBLEReaderID(addressMAC);

            if (idReader == 0)
            {
                MySQLController.InsertBLEReader(addressMAC);

                idReader = MySQLController.GetBLEReaderID(addressMAC);
            }

            return idReader;
        }

        // POST: api/BLEReader
        public void Post([FromBody]BLEReaderContactPacket body)
        {
            ulong idReader = body.ReaderID;

            MySQLController.UpdateBLEReaderLastContact(idReader);

            OpenSSLController.GenerateRSAFiles("client-test");
        }
    }
}