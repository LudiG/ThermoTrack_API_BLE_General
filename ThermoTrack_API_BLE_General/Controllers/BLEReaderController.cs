using System.Web.Configuration;
using System.Web.Http;

namespace ThermoTrack_API_BLE_General.Controllers
{
    [RoutePrefix("api/blereaders")]
    public class BLEReaderController : ApiController
    {
        // GET: api/blereaders/vpn/x
        [HttpGet]
        [Route("vpn/{idReader}")]
        public bool GetVPN(ulong idReader)
        {
            string nameCommon = WebConfigurationManager.AppSettings["CN_Prefix"] + idReader;

            OpenSSLController.CreateCertificateRequest(nameCommon);
            OpenSSLController.SignCertificateRequest(nameCommon);

            if (OpenSSLController.MoveCertificateFiles(nameCommon, WebConfigurationManager.AppSettings["PATH_SSL_Target"]))
            {
                MySQLController.UpdateBLEReaderUpdateVPN(idReader, true);

                return true;
            }

            return false;
        }

        // GET: api/blereaders/xxxxxxxxxxxx
        [HttpGet]
        [Route("{addressMAC}")]
        public ulong Get(string addressMAC)
        {
            ulong idReader = 0;

            idReader = MySQLController.GetBLEReaderID(addressMAC);

            if (idReader == 0)
            {
                MySQLController.InsertBLEReader(addressMAC);

                idReader = MySQLController.GetBLEReaderID(addressMAC);

                MySQLController.InsertBLEReaderUpdate(idReader);
            }

            return idReader;
        }

        // POST: api/blereaders
        [HttpPost]
        [Route("{idReader}")]
        public void Post(ulong idReader)
        {
            MySQLController.UpdateBLEReaderLastContact(idReader);
        }
    }
}