using System.Web.Configuration;
using System.Web.Http;

namespace ThermoTrack_API_BLE_General.Controllers
{
    [RoutePrefix("api/blereaders")]
    public class BLEReaderController : ApiController
    {
        // GET: api/blereaders/vpn/{idReader}
        [HttpGet]
        [Route("vpn/{idReader}")]
        public bool GetVPN(ulong idReader)
        {
            string nameCommon = WebConfigurationManager.AppSettings["CN_Prefix"] + idReader;

            if (!OpenSSLController.IsCertificateRegistered(nameCommon))
            {
                OpenSSLController.CreateCertificateRequest(nameCommon);
                OpenSSLController.SignCertificateRequest(nameCommon);
            }

            if (OpenSSLController.CopyCertificateFiles(nameCommon, WebConfigurationManager.AppSettings["PATH_SSL_Target"]))
            {
                MySQLController.UpdateBLEReaderUpdateVPN(idReader, true);

                return true;
            }

            return false;
        }

        // GET: api/blereaders/{addressMAC}
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

        // POST: api/blereaders/{idReader}
        [HttpPost]
        [Route("{idReader}")]
        public void Post(ulong idReader)
        {
            MySQLController.UpdateBLEReaderLastContact(idReader);
        }
    }
}