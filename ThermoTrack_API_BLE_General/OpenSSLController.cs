using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ThermoTrack_API_BLE_General
{
    public static class OpenSSLController
    {
        public static void GenerateRSAFiles(string nameCommon)
        {
            Process process = new Process();

            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;

            process.StartInfo.Arguments = @"openssl req -days 3650 -nodes -new -keyout %KEY_DIR%\%1.key -out %KEY_DIR%\%1.csr -config %KEY_CONFIG% -batch";

            process.StartInfo.EnvironmentVariables["HOME"] = @"%ProgramFiles%\OpenVPN\easy-rsa";

            process.StartInfo.EnvironmentVariables["KEY_CONFIG"] = "openssl-1.0.0.cnf";
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

            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();

            Thread.Sleep(1000);

            string output = process.StandardOutput.ReadToEnd();

            LogToFileWithSubdirectory(output, @"C:\LOGS_DEV\OpenSSL");

            process.WaitForExit();
            process.Close();
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