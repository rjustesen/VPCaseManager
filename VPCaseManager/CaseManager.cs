using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using VPCaseManager.Model;
using Microsoft.Data.Sqlite;

namespace VPCaseManager
{
    public class CaseManager
    {
        public enum INSTALL_TYPES
        {
            SINGLEUSER = 0,
            MULTIUSER = 1
        }

        public enum USER_TYPES
        {
            SUPER_USER = 1,
            ADMINISTRATOR_USER = 2,
            STANDARD_USER = 3,
            STANDALONE_USER = 4,
            HOMEOFFICE_USER = 5
        }

        private string userConnectionString;
        private string caseConnectionString;
        private string productConnectionString;

        public CaseManager(string dbpath)
        {
            userConnectionString = "Data Source=" + dbpath + @"\Users.db;";
            caseConnectionString = "Data Source=" + dbpath + @"\Clients.db;";
            productConnectionString= "Data Source=" + dbpath + @"\BlicData.db;"; 
        }
        /// <summary>
        /// Return a connection to Oracle
        /// </summary>
        /// <returns></returns>
        private SqliteConnection GetUserConnection()
        {
            try
            {
                SqliteConnection connection = new SqliteConnection(userConnectionString);
                connection.Open();
                return connection;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;
        }

        private SqliteConnection GetCaseConnection()
        {
            try
            {
                SqliteConnection connection = new SqliteConnection(caseConnectionString);
                connection.Open();
                return connection;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;
        }

        private SqliteConnection GetProductConnection()
        {
            try
            {
                SqliteConnection connection = new SqliteConnection(productConnectionString);
                connection.Open();
                return connection;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;
        }


        /// <summary>
        /// Get a user object
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User GetUser(string userId, string password)
        {
            string sql = string.Format("SELECT * from Users where UserID='{0:s}' AND PassWord='{1:s}'", userId, password);
            User user = null;
            using (var connection = GetUserConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            user = new User();
                            user.UserKey = Convert.ToInt32(dr["Key"]);
                            user.UserID = dr["UserID"].ToString();
                            user.UserName = dr["UserName"].ToString();
                            user.PassWord = dr["PassWord"].ToString();
                            user.AgentKey = Convert.ToInt32(dr["AgentKey"]);
                            user.GroupKey = Convert.ToInt32(dr["GroupKey"]);
                        }
                    }
                }
                connection.Close();
            }
            return user;
        }

        /// <summary>
        /// Return a list of users
        /// </summary>
        /// <returns></returns>
        public List<User> GetUserList()
        {
            List<User> list = new List<User>();

            string sql = "select * from Users";
            using (var connection = GetUserConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            User user = new User();
                            user.UserKey = Convert.ToInt32(dr["Key"]);
                            user.UserID = dr["UserID"].ToString();
                            user.UserName = dr["UserName"].ToString();
                            user.PassWord = dr["PassWord"].ToString();
                            user.AgentKey = Convert.ToInt32(dr["AgentKey"]);
                            user.GroupKey = Convert.ToInt32(dr["GroupKey"]);
                            list.Add(user);
                        }
                    }
                }
                connection.Close();
            }
            return list;
        }

        /// <summary>
        /// Get a client list given a user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Client> GetClientList(int userId, string orderby)
        {
            List<Client> list = new List<Client>();

            string sql = "select * from Clients where UserKey = " + userId + " Order " + orderby;
            using (var connection = GetCaseConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Client client = new Client();
                            client.Address1 = dr["ADDRESS_1"].ToString();
                            client.Address2 = dr["ADDRESS_2"].ToString();
                            client.Age = Convert.ToInt32(dr["Age"]);
                            if (Convert.ToInt32(dr["DATE_OF_BIRTH"]) > 0)
                            {
                                client.BirthDate = DateCalcs.ConvertVBDate(Convert.ToInt32(dr["DATE_OF_BIRTH"]));
                            }
                            else
                            {
                                client.BirthDate = DateCalcs.DateFromAge(client.Age);
                            }
                            client.City = dr["City"].ToString();
                            client.ClientID = Convert.ToInt32(dr["MX_CLIENT_ID"]);
                            client.FirstName = dr["FIRST_NAME"].ToString();
                            client.Gender = Convert.ToInt32(dr["Gender"]);
                            if (!DBNull.Value.Equals(dr["HOME_PHONE"]))
                            {
                                client.HomePhone = dr["HOME_PHONE"].ToString();
                            }
                            else
                            {
                                client.HomePhone = "";
                            }
                            client.Income = dr["Income"].ToString();
                            client.LastName = dr["LAST_NAME"].ToString();
                            client.MarStat = Convert.ToInt32(dr["MARITAL_STATUS"]);
                            client.MiddleName = dr["MIDDLE_NAME"].ToString();
                            client.SSN = dr["SSN"].ToString();
                            client.State = dr["State"].ToString();
                            client.UserKey = Convert.ToInt32(dr["UserKey"]);
                            client.Zip = dr["Zip"].ToString();
                            client.Email = dr["E_MAIL_ADDRESS"].ToString();
                            list.Add(client);
                        }
                    }
                }
                connection.Close();
            }
            return list;
        }

        /// <summary>
        /// Load a client from the database
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public Client GetClient(int clientId)
        {
            Client client = null;

            string sql = string.Format("select * from Clients where MX_CLIENT_ID = {0:d}", clientId);
            using (var connection = GetCaseConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            client = new Client();
                            client.Address1 = dr["ADDRESS_1"].ToString();
                            client.Address2 = dr["ADDRESS_2"].ToString();
                            client.Age = Convert.ToInt32(dr["Age"]);
                            if (Convert.ToInt32(dr["DATE_OF_BIRTH"]) > 0)
                            {
                                client.BirthDate = DateCalcs.ConvertVBDate(Convert.ToInt32(dr["DATE_OF_BIRTH"]));
                            }
                            else
                            {
                                client.BirthDate = DateCalcs.DateFromAge(client.Age);
                            }
                            client.City = dr["City"].ToString();
                            client.ClientID = Convert.ToInt32(dr["MX_CLIENT_ID"]);
                            client.FirstName = dr["FIRST_NAME"].ToString();
                            client.Gender = Convert.ToInt32(dr["Gender"]);
                            if (!DBNull.Value.Equals(dr["HOME_PHONE"]))
                            {
                                client.HomePhone = dr["HOME_PHONE"].ToString();
                            }
                            else
                            {
                                client.HomePhone = "";
                            }
                            client.Income = dr["Income"].ToString();
                            client.LastName = dr["LAST_NAME"].ToString();
                            client.MarStat = Convert.ToInt32(dr["MARITAL_STATUS"]);
                            client.MiddleName = dr["MIDDLE_NAME"].ToString();
                            client.SSN = dr["SSN"].ToString();
                            client.State = dr["State"].ToString();
                            client.UserKey = Convert.ToInt32(dr["UserKey"]);
                            client.Zip = dr["Zip"].ToString();
                            client.Email = dr["E_MAIL_ADDRESS"].ToString();
                        }
                    }
                }
                connection.Close();
            }
            return client;
        }

        public List<Case> GetCaseList(int clientKey)
        {
            List<Case> list = new List<Case>();

            string sql = "select * from Cases where ClientKey=" + clientKey;
            using (var connection = GetCaseConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Case caseinfo = new Case();
                            caseinfo.Key = Convert.ToInt32(dr["Key"]);
                            caseinfo.ClientKey = Convert.ToInt32(dr["ClientKey"]);
                            if (!DBNull.Value.Equals(dr["Servertag"]))
                            {
                                caseinfo.ServerTag = dr["Servertag"].ToString();
                            }
                            else
                            {
                                caseinfo.ServerTag = "";
                            }
                            caseinfo.Plan = Convert.ToInt32(dr["Plan"]);
                            caseinfo.CaseName = dr["CaseName"].ToString();
                            caseinfo.CaseNotes = dr["CaseNotes"].ToString();
                            caseinfo.CaseDate = Convert.ToDouble(dr["CaseDate"]);
                            if (!DBNull.Value.Equals(dr["PlanName"]))
                            {
                                caseinfo.PlanName = dr["PlanName"].ToString();
                            }
                            else
                            {
                                caseinfo.PlanName = "";
                            }
                            caseinfo.PType = dr["PType"].ToString();
                            caseinfo.UserKey = Convert.ToInt32(dr["UserKey"]);
                            caseinfo.ServerVersion = dr["ServerVersion"].ToString();
                            caseinfo.WrapperVersion = dr["WrapperVersion"].ToString();
                            list.Add(caseinfo);
                        }
                    }
                }
                connection.Close();
            }
            return list;
        }

        public Case GetCase(int caseKey)
        {

            Case caseinfo = null; ;

            string sql = "select * from Cases where Key =" + caseKey;
            using (var connection = GetCaseConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            caseinfo = new Case();
                            caseinfo.Key = Convert.ToInt32(dr["Key"]);
                            caseinfo.ClientKey = Convert.ToInt32(dr["ClientKey"]);
                            if (!DBNull.Value.Equals(dr["Servertag"]))
                            {
                                caseinfo.ServerTag = dr["Servertag"].ToString();
                            }
                            else
                            {
                                caseinfo.ServerTag = "";
                            }
                            caseinfo.Plan = Convert.ToInt32(dr["Plan"]);
                            caseinfo.CaseName = dr["CaseName"].ToString();
                            caseinfo.CaseNotes = dr["CaseNotes"].ToString();
                            caseinfo.CaseDate = Convert.ToDouble(dr["CaseDate"]);
                            if (!DBNull.Value.Equals(dr["PlanName"]))
                            {
                                caseinfo.PlanName = dr["PlanName"].ToString();
                            }
                            else
                            {
                                caseinfo.PlanName = "";
                            }
                            caseinfo.PType = dr["PType"].ToString();
                            caseinfo.UserKey = Convert.ToInt32(dr["UserKey"]);
                            caseinfo.ServerVersion = dr["ServerVersion"].ToString();
                            caseinfo.WrapperVersion = dr["WrapperVersion"].ToString();
                        }
                    }
                }
                connection.Close();
            }
            return caseinfo;
        }

        public void ITGSaveCase(Case caseInfo)
        {
            using (SqliteConnection connection = GetCaseConnection())
            {
                // 1. Check for record existence
                string checkSql = string.Format("SELECT COUNT(*) FROM Cases WHERE Key = {0:d} ", caseInfo.Key);
                using (SqliteCommand checkCommand = new SqliteCommand(checkSql, connection))
                {
                    int recordCount = (int)checkCommand.ExecuteScalar();

                    if (recordCount > 0)
                    {
                        // 2. Record exists, execute UPDATE
                        string updateSql = string.Format("UPDATE Cases SET ServerTag = @ServerTag, Plan = @Plan,CaseName = @CaseName,CaseNotes= @CaseNotes,CaseDate = @CaseDate WHERE Key =  {0:d}", caseInfo.Key);
                        using (SqliteCommand updateCommand = new SqliteCommand(updateSql, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@ServerTag", caseInfo.ServerTag);
                            updateCommand.Parameters.AddWithValue("@Plan", caseInfo.Plan);
                            updateCommand.Parameters.AddWithValue("@CaseName", caseInfo.CaseName);
                            updateCommand.Parameters.AddWithValue("@CaseNotes", caseInfo.CaseNotes);
                            updateCommand.Parameters.AddWithValue("@CaseDate", caseInfo.CaseDate);
                            updateCommand.ExecuteNonQuery();
                            Console.WriteLine($"Record with ID {caseInfo.Key} updated successfully.");
                        }
                    }
                    else
                    {
                        // 2. Record does not exist, execute INSERT
                        string insertSql = string.Format(@"INSERT INTO Cases (UserKey, ClientKey,ServerTag,Plan, CaseName, CaseNotes,CaseDate,PlanName,PType, ServerVersion,WrapperVersion) 
            VALUES ({0:d}, {1:d}, '{2:s}', {3:d},'{4:s}','{5:s}',{6:00000.000000},'{7:s}','{8:s}','{9:s}','{10:s}')",
            caseInfo.UserKey, caseInfo.ClientKey, caseInfo.ServerTag, caseInfo.Plan, caseInfo.CaseName, caseInfo.CaseNotes, caseInfo.CaseDate, caseInfo.PlanName, caseInfo.PType, caseInfo.ServerVersion, caseInfo.WrapperVersion);
                        using (SqliteCommand insertCommand = new SqliteCommand(insertSql, connection))
                        {
                            insertCommand.ExecuteNonQuery();
                        }

                        // 2. Execute the SELECT @@IDENTITY command to get the new ID
                        using (SqliteCommand cmd = new SqliteCommand("SELECT @@IDENTITY", connection))
                            caseInfo.Key = (int)cmd.ExecuteScalar();

                    }
                    Console.WriteLine($"New record with ID {caseInfo.Key} inserted successfully.");
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Save case info
        /// </summary>
        /// <param name="key"></param>
        /// <param name="topicName"></param>
        /// <param name="selector"></param>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        public void ITGSaveCaseInfo(int caseKey, string topicName, int selector, object value, int valueType)
        {
            using (SqliteConnection connection = GetCaseConnection())
            {
                // 1. Check for record existence
                string checkSql = string.Format("SELECT COUNT(*) FROM CaseInfo WHERE CaseKey = {0:d} AND TopicName = '{1:s}' AND Selector =  {2:d} ", caseKey, topicName, selector);
                using (SqliteCommand checkCommand = new SqliteCommand(checkSql, connection))
                {
                    int recordCount = (int)checkCommand.ExecuteScalar();
                    if (recordCount > 0)
                    {
                        int key = 0;
                        // 2. Record exists, find primary key and execute UPDATE
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText = string.Format("SELECT Key FROM CaseInfo WHERE CaseKey = {0:d} AND TopicName = '{1:s}' AND Selector =  {2:d} ", caseKey, topicName, selector);
                            using (var dr = command.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    key = dr.GetInt32(0);
                                }
                            }
                        }
                        string updateSql = string.Format("UPDATE CaseInfo SET TopicName = @TopicName, Selector = @Selector, ValueType = @ValueType, [Value]= @Value, Blobject= @Blobject WHERE Key = {0:d}", key);
                        using (SqliteCommand updateCommand = new SqliteCommand(updateSql, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@TopicName", topicName);
                            updateCommand.Parameters.AddWithValue("@Selector", selector);
                            updateCommand.Parameters.AddWithValue("@ValueType", valueType);
                            if (value is int)
                            {
                                updateCommand.Parameters.Add("@Value", SqliteType.Integer, sizeof(int)).Value = (int)value;
                                updateCommand.Parameters.AddWithValue("@Blobject", DBNull.Value);
                            }
                            else if (value is long)
                            {
                                updateCommand.Parameters.Add("@Value", SqliteType.Integer, sizeof(long)).Value = (long)value;
                                updateCommand.Parameters.AddWithValue("@Blobject", DBNull.Value);
                            }
                            else if (value is byte[])
                            {
                                updateCommand.Parameters.AddWithValue("@Value", 0);
                                updateCommand.Parameters.AddWithValue("@Blobject", value as byte[]);
                                updateCommand.Parameters["@Blobject"].SqliteType = SqliteType.Blob; // Explicitly set OLE DB type
                            }
                            else if (value == null && valueType == 1)
                            {
                                updateCommand.Parameters.AddWithValue("@Value", 0);
                                updateCommand.Parameters.AddWithValue("@Blobject", DBNull.Value);

                            }
                            updateCommand.ExecuteNonQuery();
                            Console.WriteLine($"Record with ID {key} updated successfully.");
                        }
                    }
                    else
                    {
                        // 2. Record does not exist, execute INSERT
                        string insertSql = "INSERT INTO CaseInfo (CaseKey, TopicName,Selector,ValueType, [Value], Blobject) VALUES (@CaseKey, @TopicName,@Selector,@ValueType, @Value, @Blobject)";
                        using (SqliteCommand insertCommand = new SqliteCommand(insertSql, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@CaseKey", caseKey);
                            insertCommand.Parameters.AddWithValue("@TopicName", topicName);
                            insertCommand.Parameters.AddWithValue("@Selector", selector);
                            insertCommand.Parameters.AddWithValue("@ValueType", valueType);
                            if (value is int)
                            {
                                insertCommand.Parameters.Add("@Value", SqliteType.Integer, sizeof(int)).Value = (int)value;
                                insertCommand.Parameters.AddWithValue("@Blobject", DBNull.Value);
                            }
                            else if (value is long)
                            {
                                insertCommand.Parameters.Add("@Value", SqliteType.Integer, sizeof(long)).Value = (long)value;
                                insertCommand.Parameters.AddWithValue("@Blobject", DBNull.Value);
                            }
                            else if (value is byte[])
                            {
                                insertCommand.Parameters.AddWithValue("@Value", 0);
                                insertCommand.Parameters.AddWithValue("@Blobject", value as byte[]);
                                insertCommand.Parameters["@Blobject"].SqliteType = SqliteType.Blob; // Explicitly set OLE DB type
                            }
                            else if (value == null && valueType == 1)
                            {
                                insertCommand.Parameters.AddWithValue("@Value", 0);
                                insertCommand.Parameters.AddWithValue("@Blobject", DBNull.Value);

                            }
                            int key = insertCommand.ExecuteNonQuery();
                            Console.WriteLine($"New record with ID {key} inserted successfully.");
                        }
                    }
                }
                connection.Close();
            }
        }


        public static byte[] ConvertOleBytesToRawBytes(byte[] oleBytes)
        {
            // The default encoding is in my case - Western European (Windows), Code Page 1252
            return Encoding.Convert(Encoding.Unicode, Encoding.Default, (byte[])oleBytes);
        }

        public CaseDetail ITGReadCaseInfo(int caseKey, string topicName, int selector)
        {
            CaseDetail detail = null;
            string sql = string.Format("SELECT * FROM CaseInfo WHERE Key = {0:d} AND TopicName = '{0:s}' AND Selector =  {1:d} ", caseKey, topicName, selector);
            using (SqliteConnection connection = GetCaseConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            detail = new CaseDetail();
                            detail.Key = Convert.ToInt32(dr["Key"]);
                            detail.Selector = Convert.ToInt32(dr["Selector"]);
                            detail.Topic_Name = dr["TopicName"].ToString();
                            detail.CaseKey = Convert.ToInt32(dr["CaseKey"]);
                            detail.ValueType = Convert.ToInt32(dr["ValueType"]);
                            detail.Value = Convert.ToInt32(dr["Value"]);
                            byte[] oleData = (byte[])dr["Blobject"];
                            if (null != oleData)
                            {
                                UTF7Encoding encoding = new UTF7Encoding();
                                detail.Blobject = encoding.GetString((Byte[])oleData);
                            }
                        }
                    }
                }
            }
            return detail;
        }

        public List<CaseDetail> GetCaseDetailByCaseID(int caseKey)
        {
            List<CaseDetail> details = new List<CaseDetail>();
            string sql = string.Format("SELECT * FROM CaseInfo WHERE CaseKey = {0:d} ", caseKey);
            using (SqliteConnection connection = GetCaseConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            CaseDetail detail = new CaseDetail();
                            detail.Key = Convert.ToInt32(dr["Key"]);
                            detail.Selector = Convert.ToInt32(dr["Selector"]);
                            detail.Topic_Name = dr["TopicName"].ToString();
                            detail.CaseKey = Convert.ToInt32(dr["CaseKey"]);
                            detail.ValueType = Convert.ToInt32(dr["ValueType"]);
                            detail.Value = Convert.ToInt32(dr["Value"]);
                            if (dr["Blobject"] != DBNull.Value)
                            {
                                byte[] oleData = (byte[])dr["Blobject"];
                                if (null != oleData)
                                {
                                    UTF7Encoding encoding = new UTF7Encoding();
                                    detail.Blobject = encoding.GetString((Byte[])oleData);
                                }
                            }
                            details.Add(detail);
                        }
                    }
                }
                connection.Close();
            }
            return details;
        }

        public void SaveClient(Model.Client client)
        {

            string stateNum = client.State;
            bool allDigits = client.State.All(char.IsDigit);
            if (!allDigits)
            {
                List<State> states = GetStates();
                for (int i = 0; i < states.Count(); i++)
                {
                    if (states[i].Abbreviation == client.State)
                    {
                        stateNum = i.ToString();
                    }
                }
            }

            using (SqliteConnection connection = GetCaseConnection())
            {
                // 1. Check for record existence
                string checkSql = $"SELECT COUNT(*) FROM Clients WHERE MX_CLIENT_ID = @MX_CLIENT_ID";
                using (var checkCommand = new SqliteCommand(checkSql, connection))
                {
                    checkCommand.Parameters.AddWithValue("@MX_CLIENT_ID", client.ClientID);
                    int recordCount = (int)checkCommand.ExecuteScalar();

                    if (recordCount > 0)
                    {
                        // 2. Record exists, execute UPDATE
                        string updateSql = @"UPDATE Clients SET UserKey = @UserKey, FIRST_NAME = @FIRST_NAME,LAST_NAME= @LAST_NAME, 
MIDDLE_NAME = @MIDDLE_NAME, Age = @Age, DATE_OF_BIRTH = @DATE_OF_BIRTH, SSN = @SSN, GENDER = @GENDER, ADDRESS_1 = @ADDRESS_1, 
ADDRESS_2 = @ADDRESS_2, CITY= @CITY, STATE= @STATE, ZIP = @ZIP, HOME_PHONE = @HOME_PHONE, INCOME= @INCOME, MARITAL_STATUS = @MARITAL_STATUS, E_MAIL_ADDRESS = @E_MAIL_ADDRESS
WHERE MX_CLIENT_ID = @MX_CLIENT_ID";
                        using (SqliteCommand updateCommand = new SqliteCommand(updateSql, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@UserKey", client.UserKey);
                            updateCommand.Parameters.AddWithValue("@FIRST_NAME", client.FirstName);
                            updateCommand.Parameters.AddWithValue("@LAST_NAME", client.LastName);
                            updateCommand.Parameters.AddWithValue("@MIDDLE_NAME", client.MiddleName);
                            updateCommand.Parameters.AddWithValue("@Age", client.Age);
                            updateCommand.Parameters.AddWithValue("@DATE_OF_BIRTH", DateCalcs.ConvertVBDate(client.BirthDate));
                            updateCommand.Parameters.AddWithValue("@SSN", client.SSN);
                            updateCommand.Parameters.AddWithValue("@GENDER", client.Gender);
                            updateCommand.Parameters.AddWithValue("@ADDRESS_1", client.Address1);
                            updateCommand.Parameters.AddWithValue("@ADDRESS_2", client.Address2);
                            updateCommand.Parameters.AddWithValue("@CITY", client.City);
                            updateCommand.Parameters.AddWithValue("@STATE", stateNum);
                            updateCommand.Parameters.AddWithValue("@ZIP", client.Zip);
                            updateCommand.Parameters.AddWithValue("@HOME_PHONE", client.HomePhone);
                            updateCommand.Parameters.AddWithValue("@INCOME", client.Income);
                            updateCommand.Parameters.AddWithValue("@MARITAL_STATUS", client.MarStat);
                            updateCommand.Parameters.AddWithValue("@E_MAIL_ADDRESS", client.Email);
                            updateCommand.ExecuteNonQuery();
                            Console.WriteLine($"Record with ID {client.ClientID} updated successfully.");
                        }
                    }
                    else
                    {
                        // 2. Record does not exist, execute INSERT

                        string insertSql = string.Format(@"INSERT INTO Clients (UserKey, FIRST_NAME,LAST_NAME, MIDDLE_NAME,Age,DATE_OF_BIRTH,SSN,GENDER, ADDRESS_1, ADDRESS_2,CITY, STATE,ZIP,HOME_PHONE,INCOME, MARITAL_STATUS,E_MAIL_ADDRESS ) VALUES ({0:d}, '{1:s}','{2:s}','{3:s}',{4:d},{5:d},'{6:s}',{7:d},'{8:s}','{9:s}','{10:s}','{11:s}','{12:s}','{13:s}',{14:f},'{15:d}','{16:s}');
                            SELECT last_insert_rowid();",
client.UserKey, client.FirstName, client.LastName, client.MiddleName, client.Age, DateCalcs.ConvertVBDate(client.BirthDate), client.SSN, client.Gender, client.Address1, client.Address2, client.City, stateNum, client.Zip, client.HomePhone, client.Income, client.MarStat, client.Email);
                        using (SqliteCommand insertCommand = new SqliteCommand(insertSql, connection))
                        {
                            int key = insertCommand.ExecuteNonQuery();
                            Console.WriteLine($"New record with ID {key} inserted successfully.");
                        }
                        // Execute the SELECT @@IDENTITY command to get the new ID
                        //using (SqliteCommand cmd = new SqliteCommand("SELECT @@IDENTITY", connection))
                        //    client.ClientID = (int)cmd.ExecuteScalar();
                    }
                }
                connection.Close();
            }
        }
        #region Agents
        public void SaveAgent(Agent agent)
        {
            using (SqliteConnection connection = new SqliteConnection(userConnectionString))
            {
                connection.Open();
                // 1. Check for record existence
                string checkSql = $"SELECT COUNT(*) FROM Agents WHERE Key = @Key";
                using (SqliteCommand checkCommand = new SqliteCommand(checkSql, connection))
                {
                    checkCommand.Parameters.AddWithValue("@AgentID", agent.Key);
                    int recordCount = (int)checkCommand.ExecuteScalar();

                    if (recordCount > 0)
                    {
                        // 2. Record exists, execute UPDATE
                        string updateSql = string.Format(@"UPDATE Agents SET AgentID = '{0:s}',LastName = '{1:s}',FirstName  = '{2:s}',MiddleName  = '{3:s}',
Title  = '{4:s}',Company  = '{5:s}',AgencyCode  = '{6:s}',Email = '{7:s}',Address1  = '{8:s}',Address2  = '{9:s}',City  = '{10:s}',
State  = '{11:d}',Zip  = '{12:s}',DialNumber = '{13:s}'", agent.AgentID, agent.LastName, agent.FirstName, agent.MiddleName, agent.Title, agent.AgencyCode,
                            agent.Email, agent.Address1, agent.Address2, agent.City, agent.State, agent.Zip, agent.DialNumber);
                        using (SqliteCommand updateCommand = new SqliteCommand(updateSql, connection))
                        {
                            updateCommand.ExecuteNonQuery();
                            Console.WriteLine($"Record with ID {agent.Key} updated successfully.");
                        }
                    }
                    else
                    {
                        // 2. Record does not exist, execute INSERT
                        string insertSql = @"INSERT INTO Agents 
(AgentID, LastName,FirstName, MiddleName,Title,Company,AgencyCode, Email, Address1,Address2, City,State,Zip,DialNumber) VALUES (@AgentID, @LastName,@FirstName, @MiddleName,@Title,@Company,@AgencyCode, @Email, @Address1,@Address2, @City,@State,@Zip,@DialNumber);
    SELECT last_insert_rowid();";
                        using (SqliteCommand insertCommand = new SqliteCommand(insertSql, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@AgentID", agent.AgentID);
                            insertCommand.Parameters.AddWithValue("@LastName", agent.LastName);
                            insertCommand.Parameters.AddWithValue("@FirstName", agent.FirstName);
                            insertCommand.Parameters.AddWithValue("@MiddleName", agent.MiddleName);
                            insertCommand.Parameters.AddWithValue("@Title", agent.Title);
                            insertCommand.Parameters.AddWithValue("@Company", agent.Company);
                            insertCommand.Parameters.AddWithValue("@AgencyCode", agent.AgencyCode);
                            insertCommand.Parameters.AddWithValue("@Email", agent.Email);
                            insertCommand.Parameters.AddWithValue("@Address1", agent.Address1);
                            insertCommand.Parameters.AddWithValue("@Address2", agent.Address2);
                            insertCommand.Parameters.AddWithValue("@City", agent.City);
                            insertCommand.Parameters.AddWithValue("@State", agent.State);
                            insertCommand.Parameters.AddWithValue("@Zip", agent.Zip);
                            insertCommand.Parameters.AddWithValue("@DialNumber", agent.DialNumber);
                            agent.Key = insertCommand.ExecuteNonQuery();
                            // 2. Execute the SELECT @@IDENTITY command to get the new ID
                            //using (SqliteCommand cmd = new SqliteCommand("SELECT @@IDENTITY", connection))
                            //    agent.Key = (int)cmd.ExecuteScalar();
                            Console.WriteLine($"New record with ID {agent.Key} inserted successfully.");
                        }

                    }
                }
                connection.Close();
            }
        }

        public Agent GetAgent(int agentKey)
        {
            Agent agent = null;

            string sql = string.Format("select * from Agents where Key = {0:d}", agentKey);
            using (var connection = GetUserConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            agent = new Agent();
                            agent.Key = Convert.ToInt32(dr["Key"]);
                            agent.AgentID = dr["AgentID"].ToString();
                            agent.LastName = dr["LastName"].ToString();
                            agent.FirstName = dr["FirstName"].ToString();
                            agent.MiddleName = dr["MiddleName"].ToString();
                            agent.Title = dr["Title"].ToString();
                            agent.Company = dr["Company"].ToString();
                            agent.AgencyCode = dr["AgencyCode"].ToString();
                            agent.Email = dr["Email"].ToString();
                            agent.Address1 = dr["Address1"].ToString();
                            agent.Address2 = dr["Address2"].ToString();
                            agent.City = dr["City"].ToString();
                            agent.State = Convert.ToInt32(dr["State"]);
                            agent.Zip = dr["Zip"].ToString();
                            agent.DialNumber = dr["DialNumber"].ToString();
                        }
                    }
                }
                connection.Close();
            }
            return agent;
            #endregion
        }

        #region Plans
        /// <summary>
        /// Return a collection of plans by plan type
        /// </summary>
        /// <param name="pType"></param>
        /// <returns></returns>
        public List<Plan> GetPlans(string pType)
        {
            List<Plan> plans = new List<Plan>();
            string sql = string.Format("Select * from PLANS where PType='{0:s}' order by PlanOrder ", pType);
            using (SqliteConnection connection = GetProductConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Plan plan = new Plan();
                            plan.Key = Convert.ToInt32(dr["Key"]);
                            plan.PlanID = Convert.ToInt32(dr["PlanID"]);
                            plan.PlanKey = dr["PlanKey"].ToString();
                            plan.Description = dr["Description"].ToString();
                            plan.PType = dr["PType"].ToString();
                            plan.Active = dr["Active"].ToString() == "TRUE";
                            plan.Available = dr["Available"].ToString() == "TRUE";
                            plan.PopUp = dr["PopUp"].ToString() == "TRUE";
                            plans.Add(plan);
                        }
                    }
                }
                connection.Close();
            }
            return plans;
        }
        /// <summary>
        /// Return a plan by actual key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Plan GetPlan(int key)
        {
            Plan plan = null;
            string sql = string.Format("Select * from PLANS where Key={0:d}", key);
            using (SqliteConnection connection = GetProductConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            plan = new Plan();
                            plan.Key = Convert.ToInt32(dr["Key"]);
                            plan.PlanKey = dr["PlanID"].ToString();
                            plan.Description = dr["Description"].ToString();
                            plan.PType = dr["PType"].ToString();
                            plan.Active = dr["Active"].ToString() == "TRUE";
                            plan.Available = dr["Available"].ToString() == "TRUE";
                            plan.PopUp = dr["PopUp"].ToString() == "TRUE";
                        }
                    }
                }
                connection.Close();
            }
            return plan;
        }

        public Plan GetPlan(string planKey)
        {
            Plan plan = null;
            string sql = string.Format("Select * from Plans where PlanKey ='{0:s}'", planKey);
            using (SqliteConnection connection = GetProductConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            plan = new Plan();
                            plan.Key = Convert.ToInt32(dr["Key"]);
                            plan.PlanKey = dr["PlanID"].ToString();
                            plan.Description = dr["Description"].ToString();
                            plan.PType = dr["PType"].ToString();
                            plan.Active = dr["Active"].ToString() == "TRUE";
                            plan.Available = dr["Available"].ToString() == "TRUE";
                            plan.PopUp = dr["PopUp"].ToString() == "TRUE";
                        }
                    }
                }
                connection.Close();
            }
            return plan;
        }


        #endregion

        #region Products
        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns></returns>
        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            string sql = "Select * from Products order by ProdOrder ";
            using (SqliteConnection connection = GetProductConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Product p = new Product();
                            p.Key = Convert.ToInt32(dr["Key"]);
                            p.ServerID = dr["ServerID"].ToString();
                            p.LOB= dr["LOB"].ToString();
                            p.PType= dr["PType"].ToString();
                            p.Description = dr["Description"].ToString();
                            p.ProdOrder = Convert.ToInt32(dr["ProdOrder"]);
                            p.Active = dr["Active"].ToString() == "TRUE";
                            p.Plans.AddRange(GetPlans(p.PType));
                            products.Add(p);
                        }
                    }
                }
                connection.Close();
            }
            return products;
        }
        /// <summary>
        /// Return a plan by actual key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Product GetProduct(int key)
        {
            Product product = null;
            string sql = string.Format("Select * from Products Where Key={0:d}", key);
            using (SqliteConnection connection = GetProductConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            product = new Product();
                            product.Key = Convert.ToInt32(dr["Key"]);
                            product.ServerID = dr["ServerID"].ToString();
                            product.LOB = dr["LOB"].ToString();
                            product.PType = dr["PType"].ToString();
                            product.Description = dr["Description"].ToString();
                            product.ProdOrder = Convert.ToInt32(dr["ProdOrder "]);
                            product.Active = dr["Active"].ToString() == "TRUE";
                            product.Plans = GetPlans(product.PType);
                        }
                    }
                }
                connection.Close();
            }
            return product;
        }

        #endregion
        #region Reports
        public List<Report> GetReports(int planKey)
        {
            List<Report> reports = new List<Report>();
            string sql = string.Format("select * from Reports where PlanKey=={0:d} " ,planKey);
            using (SqliteConnection connection = GetProductConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Report r = new Report();
                            r.Key = Convert.ToInt32(dr["Key"]);
                            r.PlanKey = Convert.ToInt32(dr["PlanKey"]);
                            r.ReportName = dr["Report"].ToString();
                            r.Name = dr["Name"].ToString();
                            r.Active = dr["Active"].ToString() == "TRUE";
                            r.Rtype = Convert.ToInt32(dr["Rtype"]);
                            r.Topic = dr["Topic"].ToString();
                            r.Value = Convert.ToInt32(dr["Value"]);
                            reports.Add(r);
                        }
                    }
                }
                connection.Close();
            }
            return reports;
        }

        #endregion
        #region States
        public List<State> GetStates()
        {
            List<State> states = new List<State>();
            string sql = "select * from States";
            using (SqliteConnection connection = GetProductConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            State s = new State();
                            s.Key = Convert.ToInt32(dr["Key"]);
                            s.Abbreviation = dr["Abbreviation"].ToString();
                            s.Name = dr["Name"].ToString();
                            states.Add(s);
                        }
                    }
                }
                connection.Close();
            }
            return states;
        }
        #endregion

        #region Views

        private int CountViews(string query)
        {
            int count = 0;
            using (SqliteConnection connection = GetUserConnection())
            {
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    // ExecuteScalar retrieves a single value (the count) and returns it as an object
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        // Convert the result to an integer
                        count = Convert.ToInt32(result);
                        Console.WriteLine($"The table YourTable has {count} rows.");
                    }
                    else
                    {
                        Console.WriteLine("Could not retrieve row count.");
                    }
                }
                connection.Close();
            }
            return count;
        }
        public List<View> GetViewList(string plantype, int planCode, int userKey)
        {
            List<View> views = new List<View>();
            string sql = string.Format("SELECT * From Views WHERE Views.UserKey= {0:d} AND Views.Product='{1:s}' AND Views.PlanCode={2:d}", userKey, plantype, planCode);
            int count = CountViews(sql);
            if (count == 0)
            {
                sql = "SELECT * From Views WHERE Views.Product='" + plantype + "' AND Views.PlanCode=" + planCode;
                count = CountViews(sql);
                if (count == 0) {
                    return GetStandardViews(plantype);
                }
            }
            using (SqliteConnection connection = GetUserConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            View v = new View();
                            v.Key = Convert.ToInt32(dr["Key"]);
                            v.UserKey = Convert.ToInt32(dr["UserKey"]);
                            v.Product = dr["Product"].ToString();
                            v.PlanCode = Convert.ToInt32(dr["PlanCode"]);
                            v.ViewName = dr["ViewName"].ToString();
                            v.Columns = dr["Columns"].ToString();
                            v.DeleteOK = dr["DeleteOK"].ToString() == "TRUE";
                            views.Add(v);
                        }
                    }
                }
                connection.Close();
            }
            return views;
        }

        public List<View> GetStandardViews(string planType)
        {
            List<View> views = new List<View>();
            string sql = "SELECT * From Views WHERE Product ='{0:s}' and PlanCode = -1 AND DeleteOK = 'FALSE'" +
                         " order by Views.DeleteOK desc";
            using (SqliteConnection connection = GetUserConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = string.Format(sql, planType);
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            View v = new View();
                            v.Key = Convert.ToInt32(dr["Key"]);
                            v.UserKey = Convert.ToInt32(dr["UserKey"]);
                            v.Product = dr["Product"].ToString();
                            v.PlanCode = Convert.ToInt32(dr["PlanCode"]);
                            v.ViewName = dr["ViewName"].ToString();
                            v.Columns = dr["Columns"].ToString();
                            v.DeleteOK = dr["DeleteOK"].ToString() == "TRUE";
                            views.Add(v);
                        }
                    }
                }
                connection.Close();
            }

            return views;
        }

        public void SaveView(View view)
        {
            using (SqliteConnection connection = new SqliteConnection(userConnectionString))
            {
                connection.Open();
                // 1. Check for record existence
                string checkSql = $"SELECT COUNT(*) FROM Views WHERE Key = @Key";
                using (SqliteCommand checkCommand = new SqliteCommand(checkSql, connection))
                {
                    checkCommand.Parameters.AddWithValue("@Key", view.Key);
                    int recordCount = (int)checkCommand.ExecuteScalar();
                    if (recordCount > 0)
                    {
                        // 2. Record exists, execute UPDATE
                        string updateSql = string.Format(@"UPDATE Views SET 
Key={0:d}, UserKey = {1:d}, ViewName= '{2:s}', Columns='{3:s}',DeleteOK='{4:s}',Product='{5:s}', PlanCode={6:d}",
        view.Key, view.UserKey, view.ViewName, view.Columns, view.DeleteOK ? "TRUE" : "FALSE", view.Product, view.PlanCode);
                        using (SqliteCommand updateCommand = new SqliteCommand(updateSql, connection))
                        {
                            updateCommand.ExecuteNonQuery();
                            Console.WriteLine($"Record with ID {view.Key} updated successfully.");
                        }
                    }
                    else
                    {   // 2. Record does not exist, execute INSERT
                        string insertSql = @"INSERT INTO Views 
( UserKey, ViewName, Columns,DeleteOK,Product, PlanCode) VALUES (@Key, @UserKey,@ViewName, @Columns,@DeleteOK,@Product,@PlanCode);
    SELECT last_insert_rowid();";
                        using (SqliteCommand insertCommand = new SqliteCommand(insertSql, connection))
                        {
                            // insertCommand.Parameters.AddWithValue("@Key", view.Key);
                            insertCommand.Parameters.AddWithValue("@UserKey", view.ViewName);
                            insertCommand.Parameters.AddWithValue("@ViewName", view.ViewName);
                            insertCommand.Parameters.AddWithValue("@Columns", view.Columns);
                            insertCommand.Parameters.AddWithValue("@DeleteOK", view.DeleteOK ? "TRUE" : "FALSE");
                            insertCommand.Parameters.AddWithValue("@Product", view.Product);
                            insertCommand.Parameters.AddWithValue("@PlanCode", view.PlanCode);
                            view.Key = insertCommand.ExecuteNonQuery();
                            Console.WriteLine($"New record with ID {view.Key} inserted successfully.");
                        }
                    }
                    connection.Close();
                }
            }
        }
        
        public void DeleteView(int key)
        {
            using (SqliteConnection connection = GetUserConnection())
            {
                string deleteQuery = "DELETE from Views where Views.DeleteOK='TRUE' AND Key= @key";

                using (SqliteCommand command = new SqliteCommand(deleteQuery, connection))
                {
                    // Bind the parameter value
                    command.Parameters.AddWithValue("@key", key);
                    // Execute the command
                    int rowsDeleted = command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteAllView()
        {
            using (SqliteConnection connection = GetUserConnection())
            {
                string deleteQuery = "DELETE from Views";

                using (SqliteCommand command = new SqliteCommand(deleteQuery, connection))
                {
                    // Execute the command
                    int rowsDeleted = command.ExecuteNonQuery();
                }
            }
        }
        #endregion
    }
}
