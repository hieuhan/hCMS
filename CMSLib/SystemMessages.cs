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
    public class SystemMessages
    {
        private short _SystemMessageId;

        private short _SystemMessageTypeId;

        private string _SystemMessageName;

        private string _SystemMessageDesc;

        private DBAccess db;

        public short SystemMessageId
        {
            get
            {
                return this._SystemMessageId;
            }
            set
            {
                this._SystemMessageId = value;
            }
        }

        public short SystemMessageTypeId
        {
            get
            {
                return this._SystemMessageTypeId;
            }
            set
            {
                this._SystemMessageTypeId = value;
            }
        }

        public string SystemMessageName
        {
            get
            {
                return this._SystemMessageName;
            }
            set
            {
                this._SystemMessageName = value;
            }
        }

        public string SystemMessageDesc
        {
            get
            {
                return this._SystemMessageDesc;
            }
            set
            {
                this._SystemMessageDesc = value;
            }
        }

        public SystemMessages()
        {
            this.db = new DBAccess();
        }

        public SystemMessages(string constr)
        {
            this.db = ((constr.Length > 0) ? new DBAccess(constr) : new DBAccess());
        }

        ~SystemMessages()
        {
        }

        public virtual void Dispose()
        {
        }

        public SystemMessages Get(short systemMessageId)
        {
            SystemMessages result = new SystemMessages(this.db.ConnectionString);
            try
            {
                if (systemMessageId > 0)
                {
                    string cmdText = "SELECT * FROM SystemMessages WHERE (SystemMessageId=" + systemMessageId + ")";
                    List<SystemMessages> list = this.Init(new SqlCommand(cmdText)
                    {
                        CommandType = CommandType.Text
                    });
                    if (list.Count > 0)
                    {
                        result = list[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public SystemMessages Get()
        {
            return this.Get(this.SystemMessageId);
        }

        private List<SystemMessages> Init(SqlCommand cmd)
        {
            SqlConnection connection = this.db.GetConnection();
            cmd.Connection = connection;
            List<SystemMessages> list = new List<SystemMessages>();
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartDataReader = new SmartDataReader(reader);
                while (smartDataReader.Read())
                {
                    list.Add(new SystemMessages
                    {
                        SystemMessageId = smartDataReader.GetInt16("SystemMessageId"),
                        SystemMessageTypeId = (short)smartDataReader.GetByte("SystemMessageTypeId"),
                        SystemMessageName = smartDataReader.GetString("SystemMessageName"),
                        SystemMessageDesc = smartDataReader.GetString("SystemMessageDesc")
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
            return list;
        }

        public static string Static_GetDesc(string constr, short systemMessageId)
        {
            string result = "";
            try
            {
                SystemMessages systemMessages = new SystemMessages(constr);
                systemMessages = systemMessages.Get(systemMessageId);
                if (systemMessages.SystemMessageDesc != null)
                {
                    result = systemMessages.SystemMessageDesc.Trim();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static string Static_GetDesc(short systemMessageId)
        {
            return SystemMessages.Static_GetDesc("", systemMessageId);
        }
    }
}
