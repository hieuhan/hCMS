using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibDatabase;

namespace CMSLib
{
    public class UserStatus
    {
        private byte _UserStatusId;

        private string _UserStatusName;

        private string _UserStatusDesc;

        private DBAccess db;

        public byte UserStatusId
        {
            get
            {
                return this._UserStatusId;
            }
            set
            {
                this._UserStatusId = value;
            }
        }

        public string UserStatusName
        {
            get
            {
                return this._UserStatusName;
            }
            set
            {
                this._UserStatusName = value;
            }
        }

        public string UserStatusDesc
        {
            get
            {
                return this._UserStatusDesc;
            }
            set
            {
                this._UserStatusDesc = value;
            }
        }

        public UserStatus()
        {
            this.db = new DBAccess();
        }

        public UserStatus(string constr)
        {
            this.db = ((constr.Length > 0) ? new DBAccess(constr) : new DBAccess());
        }

        ~UserStatus()
        {
        }

        public virtual void Dispose()
        {
        }

        private List<UserStatus> Init(SqlCommand cmd)
        {
            SqlConnection connection = this.db.GetConnection();
            cmd.Connection = connection;
            List<UserStatus> result = new List<UserStatus>();
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartDataReader = new SmartDataReader(reader);
                while (smartDataReader.Read())
                {
                    result.Add(new UserStatus
                    {
                        UserStatusDesc = smartDataReader.GetString("UserStatusDesc"),
                        UserStatusId = smartDataReader.GetByte("UserStatusId"),
                        UserStatusName = smartDataReader.GetString("UserStatusName")
                    });
                }
                smartDataReader.disposeReader(reader);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                this.db.closeConnection(connection);
            }
            return result;
        }

        public List<UserStatus> GetAll()
        {
            List<UserStatus> result;
            try
            {
                string cmdText = "SELECT * FROM UserStatus";
                SqlCommand cmd = new SqlCommand(cmdText);
                result = this.Init(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public UserStatus Get(byte id)
        {
            UserStatus result;
            try
            {
                string cmdText = "SELECT * FROM UserStatus WHERE (UserStatusId=" + id + ")";
                List<UserStatus> list = this.Init(new SqlCommand(cmdText)
                {
                    CommandType = CommandType.Text
                });
                result = list.Count == 1 ? list[0] : new UserStatus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static string Static_GetDesc(byte Id)
        {
            string result = "";
            try
            {
                UserStatus userStatus = new UserStatus();
                userStatus = userStatus.Get(Id);
                if (userStatus.UserStatusDesc != null)
                {
                    result = ((userStatus.UserStatusId == 1) ? ("<font color=blue>" + userStatus.UserStatusDesc.Trim() + "</font>") : userStatus.UserStatusDesc.Trim());
                }
            }
            catch
            {
            }
            return result;
        }

        public static UserStatus Static_Get(int userStausId, List<UserStatus> listUserStatus)
        {
            var retVal = listUserStatus.Find(x => x.UserStatusId == userStausId) ?? new UserStatus();
            return retVal;
        }
    }
}
