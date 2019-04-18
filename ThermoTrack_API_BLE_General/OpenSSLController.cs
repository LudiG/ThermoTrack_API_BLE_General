using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web.Configuration;

namespace ThermoTrack_API_BLE_General
{
    public static class OpenSSLController
    {
        public static bool IsCertificateRegistered(string nameCommon)
        {
            string directorySSL = WebConfigurationManager.AppSettings["PATH_SSL"];
            string logPath = WebConfigurationManager.AppSettings["PATH_LOGS_SSL"];

            List<string> lines = new List<string>();

            try
            {
                lines = new List<string>(File.ReadAllLines(directorySSL + @"\keys\index.txt"));
            }

            catch (Exception ex)
            {
                LogToFileWithSubdirectory(ex.Message, logPath);

                return false;
            }

            foreach (string line in lines)
            {
                string attributes = line.Split(null)[5];

                foreach (string token in attributes.Split('/'))
                    if (token.StartsWith("CN"))
                        if (token.Substring(3) == nameCommon)
                            return true;
            }

            return false;
        }

        public static void CreateCertificateRequest(string nameCommon)
        {
            string directorySSL = WebConfigurationManager.AppSettings["PATH_SSL"];
            string logPath = WebConfigurationManager.AppSettings["PATH_LOGS_SSL"];

            Debug.WriteLine(directorySSL);

            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;

                process.StartInfo.WorkingDirectory = directorySSL;

                process.StartInfo.Arguments = @"/c openssl req -days 3650 -nodes -new -keyout %KEY_DIR%\%KEY_NAME%.key -out %KEY_DIR%\%KEY_NAME%.csr -config openssl-1.0.0.cnf -batch";

                process.StartInfo.EnvironmentVariables["HOME"] = directorySSL;

                process.StartInfo.EnvironmentVariables["KEY_DIR"] = "keys";
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
            }
        }

        public static void SignCertificateRequest(string nameCommon)
        {
            string directorySSL = WebConfigurationManager.AppSettings["PATH_SSL"];
            string logPath = WebConfigurationManager.AppSettings["PATH_LOGS_SSL"];

            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;

                process.StartInfo.WorkingDirectory = directorySSL;

                process.StartInfo.Arguments = @"/c openssl ca -days 3650 -out %KEY_DIR%\%KEY_NAME%.crt -in %KEY_DIR%\%KEY_NAME%.csr -config openssl-1.0.0.cnf -batch";

                process.StartInfo.EnvironmentVariables["HOME"] = directorySSL;

                process.StartInfo.EnvironmentVariables["KEY_DIR"] = "keys";
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
            }
        }

        public static bool CopyCertificateFiles(string nameCommon, string directoryTarget)
        {
            string directorySSL = WebConfigurationManager.AppSettings["PATH_SSL"];
            string logPath = WebConfigurationManager.AppSettings["PATH_LOGS_SSL"];

            string pathCertficateSource = directorySSL + @"\keys\" + nameCommon + ".crt";
            string pathKeySource = directorySSL + @"\keys\" + nameCommon + ".key";
            string pathRequestSource = directorySSL + @"\keys\" + nameCommon + ".csr";

            string pathCertficateTarget = directoryTarget + @"\" + nameCommon + ".crt";
            string pathKeyTarget = directoryTarget + @"\" + nameCommon + ".key";

            if (File.Exists(pathCertficateTarget))
                File.Delete(pathCertficateTarget);

            if (File.Exists(pathKeyTarget))
                File.Delete(pathKeyTarget);

            try
            {
                File.Copy(pathCertficateSource, pathCertficateTarget);
                File.Copy(pathKeySource, pathKeyTarget);
            }

            catch (Exception ex)
            {
                LogToFileWithSubdirectory(ex.Message, logPath);

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