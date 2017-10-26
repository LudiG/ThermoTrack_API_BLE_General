using System;
using System.Diagnostics;
using System.IO;
using System.Web.Configuration;

namespace ThermoTrack_API_BLE_General
{
    public static class OpenSSLController
    {
        public static string CreateCertificateRequest(string nameCommon)
        {
            string error;

            string directorySSL = WebConfigurationManager.AppSettings["PATH_SSL"];

            Process process = new Process();

            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;

            process.StartInfo.Arguments = @"/c openssl req -days 3650 -nodes -new -keyout %KEY_DIR%\%KEY_NAME%.key -out %KEY_DIR%\%KEY_NAME%.csr -config %HOME%\openssl-1.0.0.cnf -batch";

            //process.StartInfo.EnvironmentVariables["HOME"] = @"%ProgramFiles%\OpenVPN\easy-rsa";
            process.StartInfo.EnvironmentVariables["HOME"] = directorySSL;

            process.StartInfo.EnvironmentVariables["KEY_DIR"] = directorySSL + @"\keys";
            process.StartInfo.EnvironmentVariables["KEY_SIZE"] = "1024";

            process.StartInfo.EnvironmentVariables["KEY_COUNTRY"] = "ZA";
            process.StartInfo.EnvironmentVariables["KEY_PROVINCE"] = "WP";
            process.StartInfo.EnvironmentVariables["KEY_CITY"] = "CapeTown";
            process.StartInfo.EnvironmentVariables["KEY_ORG"] = "ThermoTrack";
            process.StartInfo.EnvironmentVariables["KEY_OU"] = "";
            process.StartInfo.EnvironmentVariables["KEY_EMAIL"] = "jaco@istm.co.za";
            process.StartInfo.EnvironmentVariables["KEY_CN"] = nameCommon;
            process.StartInfo.EnvironmentVariables["KEY_NAME"] = nameCommon;

            process.StartInfo.EnvironmentVariables["PKCS11_MODULE_PATH"] = "changeme";
            process.StartInfo.EnvironmentVariables["PKCS11_PIN"] = "1234";

            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();
            process.WaitForExit();

            error = process.StandardError.ReadToEnd();

            process.Close();

            return error;
        }

        public static void SignCertificateRequest(string nameCommon)
        {
            string error;

            string directorySSL = WebConfigurationManager.AppSettings["PATH_SSL"];

            Process process = new Process();

            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;

            process.StartInfo.Arguments = @"/c openssl ca -days 3650 -out %KEY_DIR%\%KEY_NAME%.crt -in %KEY_DIR%\%KEY_NAME%.csr -config %HOME%\openssl-1.0.0.cnf -batch";

            //process.StartInfo.EnvironmentVariables["HOME"] = @"%ProgramFiles%\OpenVPN\easy-rsa";
            process.StartInfo.EnvironmentVariables["HOME"] = directorySSL;

            process.StartInfo.EnvironmentVariables["KEY_DIR"] = directorySSL + @"\keys";
            process.StartInfo.EnvironmentVariables["KEY_SIZE"] = "1024";

            process.StartInfo.EnvironmentVariables["KEY_COUNTRY"] = "ZA";
            process.StartInfo.EnvironmentVariables["KEY_PROVINCE"] = "WP";
            process.StartInfo.EnvironmentVariables["KEY_CITY"] = "CapeTown";
            process.StartInfo.EnvironmentVariables["KEY_ORG"] = "ThermoTrack";
            process.StartInfo.EnvironmentVariables["KEY_OU"] = "";
            process.StartInfo.EnvironmentVariables["KEY_EMAIL"] = "jaco@istm.co.za";
            process.StartInfo.EnvironmentVariables["KEY_CN"] = nameCommon;
            process.StartInfo.EnvironmentVariables["KEY_NAME"] = nameCommon;

            process.StartInfo.EnvironmentVariables["PKCS11_MODULE_PATH"] = "changeme";
            process.StartInfo.EnvironmentVariables["PKCS11_PIN"] = "1234";

            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();
            process.WaitForExit();

            error = process.StandardError.ReadToEnd();

            process.Close();
        }

        public static bool MoveCertificateFiles(string nameCommon, string directoryTarget)
        {
            string directorySSL = WebConfigurationManager.AppSettings["PATH_SSL"];

            string logPath = WebConfigurationManager.AppSettings["PATH_LOGS_SSL"];

            string pathCertficateSource = directorySSL + @"\keys\" + nameCommon + ".crt";
            string pathKeySource = directorySSL + @"\keys\" + nameCommon + ".key";
            string pathRequestSource = directorySSL + @"\keys\" + nameCommon + ".csr";

            string pathCertficateTarget = directoryTarget + @"\" + nameCommon + ".crt";
            string pathKeyTarget = directoryTarget + @"\" + nameCommon + ".key";

            if (File.Exists(pathRequestSource))
                File.Delete(pathRequestSource);

            if (File.Exists(pathCertficateTarget))
                File.Delete(pathCertficateTarget);

            if (File.Exists(pathKeyTarget))
                File.Delete(pathKeyTarget);

            try
            {
                File.Move(pathCertficateSource, pathCertficateTarget);
                File.Move(pathKeySource, pathKeyTarget);
            }

            catch (Exception ex)
            {
                LogToFileWithSubdirectory(ex.Message, logPath);

                if (File.Exists(pathCertficateSource))
                    File.Delete(pathCertficateSource);

                if (File.Exists(pathKeySource))
                    File.Delete(pathKeySource);

                if (File.Exists(pathCertficateTarget))
                    File.Delete(pathCertficateTarget);

                if (File.Exists(pathKeyTarget))
                    File.Delete(pathKeyTarget);

                return false;
            }

            return true;
        }

        private static void LogToFileWithSubdirectory(string message, string subdirectoryName)
        {
            DateTime timestamp = DateTime.Now;

            Directory.CreateDirectory(subdirectoryName);

            using (StreamWriter logWriter = File.CreateText(Path.Combine(subdirectoryName, timestamp.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt")))
            {
                logWriter.WriteLine(message);
            }
        }
    }
}