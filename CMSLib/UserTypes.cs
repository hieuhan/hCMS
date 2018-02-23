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
    public class UserTypes
    {
        private byte _UserTypeId;

        private string _UserTypeName;

        private string _UserTypeDesc;

        private DBAccess db;

        public byte UserTypeId
        {
            get
            {
                return this._UserTypeId;
            }
            set
            {
                this._UserTypeId = value;
            }
        }

        public string UserTypeName
        {
            get
            {
                return this._UserTypeName;
            }
            set
            {
                this._UserTypeName = value;
            }
        }

        public string UserTypeDesc
        {
            get
            {
                return this._UserTypeDesc;
            }
            set
            {
                this._UserTypeDesc = value;
            }
        }

        public UserTypes()
        {
            this.db = new DBAccess();
        }

        public UserTypes(string constr)
        {
            this.db = ((constr.Length > 0) ? new DBAccess(constr) : new DBAccess());
        }

        ~UserTypes()
        {
        }

        private List<UserTypes> Init(SqlCommand cmd)
        {
            SqlConnection connection = this.db.GetConnection();
            cmd.Connection = connection;
            List<UserTypes> result = new List<UserTypes>();
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartDataReader = new SmartDataReader(reader);
                while (smartDataReader.Read())
                {
                    result.Add(new UserTypes
                    {
                        UserTypeId = smartDataReader.GetByte("UserTypeId"),
                        UserTypeName = smartDataReader.GetString("UserTypeName"),
                        UserTypeDesc = smartDataReader.GetString("UserTypeDesc")
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

        public List<UserTypes> GetAll()
        {
            List<UserTypes> result;
            try
            {
                string cmdText = "SELECT * FROM UserTypes";
                SqlCommand cmd = new SqlCommand(cmdText);
                result = this.Init(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static List<UserTypes> Static_GetList()
        {
            return UserTypes.Static_GetList("");
        }

        public static List<UserTypes> Static_GetList(string constr)
        {
            List<UserTypes> result = new List<UserTypes>();
            try
            {
                UserTypes userTypes = new UserTypes(constr);
                result = userTypes.GetList();
            }
            catch
            {
                result = new List<UserTypes>();
            }
            return result;
        }

        public List<UserTypes> GetList()
        {
            List<UserTypes> result;
            try
            {
                string cmdText = "SELECT * FROM UserTypes";
                SqlCommand cmd = new SqlCommand(cmdText);
                result = this.Init(cmd);
            }
            catch
            {
                result = new List<UserTypes>();
            }
            return result;
        }

        public UserTypes Get(byte id)
        {
            UserTypes result = new UserTypes();
            try
            {
                if (id > 0)
                {
                    string cmdText = "SELECT * FROM UserTypes WHERE (UserTypeId=" + id + ")";
                    List<UserTypes> list = this.Init(new SqlCommand(cmdText)
                    {
                        CommandType = CommandType.Text
                    });
                    if (list.Count >= 1)
                    {
                        result = list[0];
                    }
                }
            }
            catch
            {
                result = new UserTypes();
            }
            return result;
        }
    }
}
