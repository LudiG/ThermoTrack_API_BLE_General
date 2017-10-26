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
            string logPath = WebConfigurationManager.AppSettings["PATH_LOGS_MySQL"];

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

        public static string GetBLEReaderVersion(ulong idReader, BLEReaderApplicationType application)
        {
            string version = "NULL";

            string sqlConnectionString = WebConfigurationManager.ConnectionStrings["MySQL_BLE"].ConnectionString;
            string logPath = WebConfigurationManager.AppSettings["PATH_LOGS_MySQL"];

            try
            {
                using (MySqlConnection sqlConnection = new MySqlConnection(sqlConnectionString))
                {
                    sqlConnection.Open();

                    string sqlCommandString = "SELECT tblblereadersupdate_ttble.szVersion_";

                    if (application == BLEReaderApplicationType.RPi_BLE_Scanner)
                        sqlCommandString += "RPi_BLE_Scanner ";

                    else if (application == BLEReaderApplicationType.RPi_WatchDog)
                        sqlCommandString += "RPi_WatchDog ";

                    else if (application == BLEReaderApplicationType.RPi_Updater)
                        sqlCommandString += "RPi_Updater ";

                    sqlCommandString += "FROM tblblereadersupdate_ttble " +
                                        "WHERE (tblblereadersupdate_ttble.iReaderID = " + idReader + ");";

                    using (MySqlCommand sqlCommand = new MySqlCommand(sqlCommandString, sqlConnection))
                    {
                        using (MySqlDataReader sqlReader = sqlCommand.ExecuteReader())
                        {
                            if (sqlReader.Read())
                            {
                                version = sqlReader.GetString(0);
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

            return version;
        }

        public static bool GetBLEReaderUpdate(ulong idReader, BLEReaderApplicationType application)
        {
            bool update = false;

            string sqlConnectionString = WebConfigurationManager.ConnectionStrings["MySQL_BLE"].ConnectionString;
            string logPath = WebConfigurationManager.AppSettings["PATH_LOGS_MySQL"];

            try
            {
                using (MySqlConnection sqlConnection = new MySqlConnection(sqlConnectionString))
                {
                    sqlConnection.Open();

                    string sqlCommandString = "SELECT tblblereadersupdate_ttble.bUpdate_";

                    if (application == BLEReaderApplicationType.RPi_BLE_Scanner)
                        sqlCommandString += "RPi_BLE_Scanner ";

                    else if (application == BLEReaderApplicationType.RPi_WatchDog)
                        sqlCommandString += "RPi_WatchDog ";

                    else if (application == BLEReaderApplicationType.RPi_Updater)
                        sqlCommandString += "RPi_Updater ";

                    sqlCommandString += "FROM tblblereadersupdate_ttble " +
                                        "WHERE (tblblereadersupdate_ttble.iReaderID = " + idReader + ");";

                    using (MySqlCommand sqlCommand = new MySqlCommand(sqlCommandString, sqlConnection))
                    {
                        using (MySqlDataReader sqlReader = sqlCommand.ExecuteReader())
                        {
                            if (sqlReader.Read())
                            {
                                update = sqlReader.GetBoolean(0);
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

            return update;
        }

        public static bool GetBLEReaderUpdateVPN(ulong idReader)
        {
            bool update = false;

            string sqlConnectionString = WebConfigurationManager.ConnectionStrings["MySQL_BLE"].ConnectionString;
            string logPath = WebConfigurationManager.AppSettings["PATH_LOGS_MySQL"];

            try
            {
                using (MySqlConnection sqlConnection = new MySqlConnection(sqlConnectionString))
                {
                    sqlConnection.Open();

                    string sqlCommandString = "SELECT tblblereadersupdate_ttble.bUpdate_VPN FROM tblblereadersupdate_ttble " +
                                        "WHERE (tblblereadersupdate_ttble.iReaderID = " + idReader + ");";

                    using (MySqlCommand sqlCommand = new MySqlCommand(sqlCommandString, sqlConnection))
                    {
                        using (MySqlDataReader sqlReader = sqlCommand.ExecuteReader())
                        {
                            if (sqlReader.Read())
                            {
                                update = sqlReader.GetBoolean(0);
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

            return update;
        }

        public static void InsertBLEReader(string addressMAC)
        {
            string sqlConnectionString = WebConfigurationManager.ConnectionStrings["MySQL_BLE"].ConnectionString;
            string logPath = WebConfigurationManager.AppSettings["PATH_LOGS_MySQL"];

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

        public static void InsertBLEReaderUpdate(ulong idReader)
        {
            string sqlConnectionString = WebConfigurationManager.ConnectionStrings["MySQL_BLE"].ConnectionString;
            string logPath = WebConfigurationManager.AppSettings["PATH_LOGS_MySQL"];

            try
            {
                using (MySqlConnection sqlConnection = new MySqlConnection(sqlConnectionString))
                {
                    sqlConnection.Open();

                    string sqlCommandString = "INSERT INTO tblblereadersupdate_ttble(iReaderID) " +
                                              "VALUES (\"" + idReader + "\");";

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

        public static void UpdateBLEReaderVersion(ulong idReader, BLEReaderApplicationType application, string version)
        {
            string sqlConnectionString = WebConfigurationManager.ConnectionStrings["MySQL_BLE"].ConnectionString;
            string logPath = WebConfigurationManager.AppSettings["PATH_LOGS_MySQL"];

            try
            {
                using (MySqlConnection sqlConnection = new MySqlConnection(sqlConnectionString))
                {
                    sqlConnection.Open();

                    DateTime newNotificationTime = DateTime.Now;

                    string sqlCommandString = "UPDATE tblblereadersupdate_ttble " +
                                              "SET tblblereadersupdate_ttble.szVersion_";

                    if (application == BLEReaderApplicationType.RPi_BLE_Scanner)
                        sqlCommandString += "RPi_BLE_Scanner ";

                    else if (application == BLEReaderApplicationType.RPi_WatchDog)
                        sqlCommandString += "RPi_WatchDog ";

                    else if (application == BLEReaderApplicationType.RPi_Updater)
                        sqlCommandString += "RPi_Updater ";

                    sqlCommandString += "= \'" + version + "\' " +
                                        "WHERE (tblblereadersupdate_ttble.iReaderID = " + idReader + ");";

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

        public static void UpdateBLEReaderUpdate(ulong idReader, BLEReaderApplicationType application, bool update)
        {
            string sqlConnectionString = WebConfigurationManager.ConnectionStrings["MySQL_BLE"].ConnectionString;
            string logPath = WebConfigurationManager.AppSettings["PATH_LOGS_MySQL"];

            try
            {
                using (MySqlConnection sqlConnection = new MySqlConnection(sqlConnectionString))
                {
                    sqlConnection.Open();

                    DateTime newNotificationTime = DateTime.Now;

                    string sqlCommandString = "UPDATE tblblereadersupdate_ttble " +
                                              "SET tblblereadersupdate_ttble.bUpdate_";

                    if (application == BLEReaderApplicationType.RPi_BLE_Scanner)
                        sqlCommandString += "RPi_BLE_Scanner ";

                    else if (application == BLEReaderApplicationType.RPi_WatchDog)
                        sqlCommandString += "RPi_WatchDog ";

                    else if (application == BLEReaderApplicationType.RPi_Updater)
                        sqlCommandString += "RPi_Updater ";

                    sqlCommandString += "= " + update + " " +
                                        "WHERE (tblblereadersupdate_ttble.iReaderID = " + idReader + ");";

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

        public static void UpdateBLEReaderUpdateVPN(ulong idReader, bool update)
        {
            string sqlConnectionString = WebConfigurationManager.ConnectionStrings["MySQL_BLE"].ConnectionString;
            string logPath = WebConfigurationManager.AppSettings["PATH_LOGS_MySQL"];

            try
            {
                using (MySqlConnection sqlConnection = new MySqlConnection(sqlConnectionString))
                {
                    sqlConnection.Open();

                    DateTime newNotificationTime = DateTime.Now;

                    string sqlCommandString = "UPDATE tblblereadersupdate_ttble " +
                                              "SET tblblereadersupdate_ttble.bUpdate_VPN = " + update + " " +
                                              "WHERE (tblblereadersupdate_ttble.iReaderID = " + idReader + ");";

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
            string logPath = WebConfigurationManager.AppSettings["PATH_LOGS_MySQL"];

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