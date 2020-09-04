using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace systemapps.DBops
{
    class Functions
    {

        public static bool TestDbConnection(string dbip, string dbname, string dbusername, string dbpassword)
        {
            bool bresult = false;
            Console.WriteLine("SourceServerIpTmp = " + dbip + " ;SourceDbNameTmp = " + dbname + " ;SourceDbUsernameTmp = " + dbusername);

            try
            {
                String sqlString = "select cam_id from camera limit 1;";
                System.Data.DataTable dttestconn = DBops.Functions.TestDataTableFromDB(dbip, dbname, dbusername, dbpassword, sqlString);
                bresult = true;

            }

            catch (Exception ex)
            {

            }


            return bresult;
        }

        public static DataTable TestDataTableFromDB(string dbip, string dbname, string dbusername, string dbpassword, string sqlstatement)
        {
            DataTable dt = new DataTable();
            String connectionStr = "Server=" + dbip + "; Database=" + dbname + "; UID=" + dbusername + ";Password=" + dbpassword + "; SslMode=none";
            MySqlConnection con = new MySqlConnection(connectionStr);

            try
            {
                MySqlCommand cmd = new MySqlCommand(sqlstatement, con);
                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                adap.Fill(dt);
                con.Close();

            }
            catch (Exception ex)
            {
                throw ex;

            }

            finally
            {
                con.Close();
            }

            return dt;
        }

        public static bool CreateNewDatabase(string SourceServerIp, string SourceDbName, string SourceDbUsername, string SourceDbPassword)
        {
            bool bResult = false;

            try
            {
                String sqlString = "CREATE DATABASE " + SourceDbName + ";";
                int result = CreateNewDB(SourceServerIp, SourceDbName, SourceDbUsername, SourceDbPassword, sqlString);
                if (result >= 0)
                {
                    string createTableSqlString = File.ReadAllText("database_script.sql");
                    Console.WriteLine(createTableSqlString);
                    result = CreateNewTables(SourceServerIp, SourceDbName, SourceDbUsername, SourceDbPassword, createTableSqlString);
                    if (result >= 0)
                    {
                        bResult = true;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return bResult;
        }

        public static int CreateNewDB(string SourceServerIp, string SourceDbName, string SourceDbUsername, string SourceDbPassword, string sqlStatement)
        {
            int retVal = -1;

            MySqlConnection connection = new MySqlConnection("Data Source=" + SourceServerIp + ";UserId=" + SourceDbUsername + ";PWD=" + SourceDbPassword + "; SslMode=none");
            MySqlCommand command = new MySqlCommand(sqlStatement, connection);

            try
            {
                connection.Open();
                retVal = command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                connection.Close();

            }

            return retVal;
        }

        public static int CreateNewTables(string SourceServerIp, string SourceDbName, string SourceDbUsername, string SourceDbPassword, string sqlStatement)
        {
            int retVal = -1;
            String connectionStr = "Server=" + SourceServerIp + "; Database=" + SourceDbName + "; UID=" + SourceDbUsername + ";Password=" + SourceDbPassword + "; SslMode=none";
            MySqlConnection con = new MySqlConnection(connectionStr);

            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(sqlStatement);
                cmd.Connection = con;
                retVal = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                con.Close();

            }

            return retVal;
        }

        public static bool DropDatabase(string SourceServerIp, string SourceDbName, string SourceDbUsername, string SourceDbPassword)
        {
            bool bResult = false;

            try
            {
                String sqlString = "DROP DATABASE " + SourceDbName + ";";
                int result = CreateNewDB(SourceServerIp, SourceDbName, SourceDbUsername, SourceDbPassword, sqlString);
                if (result >= 0)
                {
                    bResult = true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return bResult;
        }

        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            System.Configuration.AppSettingsReader settingsReader =
                                                new AppSettingsReader();
            // Get the key from config file

            string key = (string)settingsReader.GetValue("SecurityKey",
                                                             typeof(String));
            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public void FillGrid(string ip, string dbname, string username, string password, string query, DataGrid grid)
        {
            string myConnectionString = "server=" + ip + ";database=" + dbname + ";uid=" + username + ";pwd=" + password;
            //string query = "SELECT id,ip_address FROM server_info";
            MySqlConnection con = new MySqlConnection(myConnectionString);

            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "server_info");
                grid.DataContext = ds;


                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }




        }


    }


}
