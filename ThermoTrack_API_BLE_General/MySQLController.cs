using System;
using System.IO;
using System.Web.Configuration;

using MySql.Data.MySqlClient;

namespace ThermoTrack_API_BLE_General
{
    public static class MySQLController
    {
        public static ulong GetBLEReaderID(string addressMAC)
        {
            ulong idReader = 0;

            string sqlConnectionString = WebConfigurationManager.ConnectionStrings["MySQL_BLE"].ConnectionString;

            string logPath = @"C:\LOGS_DEV\ThermoTrack_API_BLE_General";

            try
            {
                using (MySqlConnection sqlConnection = new MySqlConnection(sqlConnectionString))
                {
                    sqlConnection.Open();

                    string sqlCommandString = "SELECT tblblereaders_ttble.iReaderID FROM tblblereaders_ttble " +
                                              "WHERE (tblblereaders_ttble.szMACAddress = \"" + addressMAC + "\");";

                    using (MySqlCommand sqlCommand = new MySqlCommand(sqlCommandString, sqlConnection))
                    {
                        using (MySqlDataReader sqlReader = sqlCommand.ExecuteReader())
                        {
                            if (sqlReader.Read())
                            {
                                idReader = sqlReader.GetUInt64(0);
                            }
                        }
                    }

                    sqlConnection.Close();
                }
            }

            catch (MySqlException ex)
            {
                LogToFileWithSubdirectory(ex.Message, logPath);
            }

            return idReader;
        }

        public static void InsertBLEReader(string addressMAC)
        {
            string sqlConnectionString = WebConfigurationManager.ConnectionStrings["MySQL_BLE"].ConnectionString;

            string logPath = @"C:\LOGS_DEV\ThermoTrack_API_BLE_General";

            try
            {
                using (MySqlConnection sqlConnection = new MySqlConnection(sqlConnectionString))
                {
                    sqlConnection.Open();

                    string sqlCommandString = "INSERT INTO tblblereaders_ttble(szMACAddress) " +
                                              "VALUES (\"" + addressMAC + "\");";

                    using (MySqlCommand sqlCommand = new MySqlCommand(sqlCommandString, sqlConnection))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    sqlConnection.Close();
                }
            }

            catch (MySqlException ex)
            {
                LogToFileWithSubdirectory(ex.Message, logPath);
            }
        }

        public static void UpdateBLEReaderLastContact(ulong idReader)
        {
            string sqlConnectionString = WebConfigurationManager.ConnectionStrings["MySQL_BLE"].ConnectionString;

            string logPath = @"C:\LOGS_DEV\ThermoTrack_API_BLE_General";

            try
            {
                using (MySqlConnection sqlConnection = new MySqlConnection(sqlConnectionString))
                {
                    sqlConnection.Open();

                    DateTime newNotificationTime = DateTime.Now;

                    string sqlCommandString = "UPDATE tblblereaders_ttble " +
                                              "SET tblblereaders_ttble.dtLastContact = \'" + newNotificationTime.ToString("yyyy-MM-dd HH:mm:ss") + "\' " +
                                              "WHERE (tblblereaders_ttble.iReaderID = " + idReader + ");";

                    using (MySqlCommand sqlCommand = new MySqlCommand(sqlCommandString, sqlConnection))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    sqlConnection.Close();
                }
            }

            catch (MySqlException ex)
            {
                LogToFileWithSubdirectory(ex.Message, logPath);
            }
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