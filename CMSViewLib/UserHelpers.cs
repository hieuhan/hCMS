using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMSLib;
using LibDatabase;
using LibUtils;

namespace CMSViewLib
{
    public class UserHelpers
    {
        private static DBAccess db;
        //-----------------------------------------------------------
        public static List<Users> Init(SmartDataReader smartReader)
        {
            List<Users> listUsers = new List<Users>();
            try
            {
                while (smartReader.Read())
                {
                    Users user = new Users
                    {
                        UserId = smartReader.GetInt32("UserId"),
                        UserName = smartReader.GetString("UserName"),
                        Password = smartReader.GetString("Password"),
                        Fullname = smartReader.GetString("Fullname"),
                        Address = smartReader.GetString("Address"),
                        GenderId = smartReader.GetByte("GenderId"),
                        UserStatusId = smartReader.GetByte("UserStatusId"),
                        DefaultActionId = smartReader.GetInt16("DefaultActionId"),
                        CrDateTime = smartReader.GetDateTime("CrDateTime")
                    };
                    listUsers.Add(user);
                }
                return listUsers;
            }
            catch (Exception err)
            {
                throw new ApplicationException("Data error: " + err.Message);
            }
        }
        //-----------------------------------------------------------
        public static UserLoginResult Login(string userName, string password)
        {
            UserLoginResult retVal = new UserLoginResult();
            try
            {
                SqlCommand cmd = new SqlCommand("User_Login") { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Add(new SqlParameter("@UserName", StringUtil.InjectionString(userName)));
                cmd.Parameters.Add(new SqlParameter("@Password", StringUtil.InjectionString(password)));
                cmd.Parameters.Add("@LoginStatus", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@LoginMessage", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;

                db = new DBAccess(DbConstants.CONNECTION_STRING);
                SqlConnection con = db.GetConnection();
                cmd.Connection = con;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                List<Users> listUsers = Init(smartReader);
                retVal.User = listUsers.Count == 0 ? new Users() : listUsers[0];
                reader.NextResult();
                reader.Close();
                if (cmd.Parameters["@LoginStatus"].Value != DBNull.Value) retVal.ActionStatus = cmd.Parameters["@LoginStatus"].Value.ToString();
                if (cmd.Parameters["@LoginMessage"].Value != DBNull.Value) retVal.ActionMessage = cmd.Parameters["@LoginMessage"].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                db.closeConnection(db.GetConnection());
            }
            return retVal;
        }
    }
    //-----------------------------------------------------------
    public class UserLoginResult
    {
        public Users User;
        public string ActionStatus = string.Empty;
        public string ActionMessage = string.Empty;
    }
    //-----------------------------------------------------------
}
