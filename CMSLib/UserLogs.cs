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
    public class UserLogs
    {
        private int _logUserId;

        private string _Username;

        private string _Password;

        private string _IPAddress;

        private int _statusId;

        private DateTime _CrDateTime;

        private DBAccess db;
        private string _userAgent;

        public int logUserId
        {
            get
            {
                return this._logUserId;
            }
            set
            {
                this._logUserId = value;
            }
        }

        public string UserName
        {
            get
            {
                return this._Username;
            }
            set
            {
                this._Username = value;
            }
        }

        public string Password
        {
            get
            {
                return this._Password;
            }
            set
            {
                this._Password = value;
            }
        }

        public string IPAddress
        {
            get
            {
                return this._IPAddress;
            }
            set
            {
                this._IPAddress = value;
            }
        }

        public int StatusId
        {
            get
            {
                return this._statusId;
            }
            set
            {
                this._statusId = value;
            }
        }

        public DateTime CrDateTime
        {
            get
            {
                return this._CrDateTime;
            }
            set
            {
                this._CrDateTime = value;
            }
        }

        public string UserAgent
        {
            get => _userAgent;
            set => _userAgent = value;
        }

        public UserLogs()
        {
            this.db = new DBAccess(DbConstants.CONNECTION_STRING);
        }

        public UserLogs(string constr)
        {
            this.db = ((constr.Length > 0) ? new DBAccess(constr) : new DBAccess());
        }

        ~UserLogs()
        {
        }

        public virtual void Dispose()
        {
        }

        private List<UserLogs> Init(SqlCommand cmd)
        {
            SqlConnection sqlConnection = cmd.Connection = this.db.GetConnection();
            List<UserLogs> list = new List<UserLogs>();
            try
            {
                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartDataReader = new SmartDataReader(reader);
                while (smartDataReader.Read())
                {
                    UserLogs userLogs = new UserLogs
                    {
                        logUserId = smartDataReader.GetInt32("logUserId"),
                        UserName = smartDataReader.GetString("Username"),
                        Password = smartDataReader.GetString("Password"),
                        IPAddress = smartDataReader.GetString("IPAddress"),
                        UserAgent = smartDataReader.GetString("UserAgent"),
                        CrDateTime = smartDataReader.GetDateTime("CrDateTime"),
                        StatusId = smartDataReader.GetInt32("Status")
                    };
                    list.Add(userLogs);
                }
                smartDataReader.disposeReader(reader);
                return list;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                this.db.closeConnection(sqlConnection);
            }
        }

        public void Add()
        {
            SqlCommand sqlCommand = new SqlCommand("UserLogs_Insert");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.Add(new SqlParameter("@Username", this.UserName));
            sqlCommand.Parameters.Add(new SqlParameter("@Password", this.Password));
            sqlCommand.Parameters.Add(new SqlParameter("@CrDateTime", this.CrDateTime));
            sqlCommand.Parameters.Add(new SqlParameter("@IPAddress", this.IPAddress));
            sqlCommand.Parameters.Add(new SqlParameter("@UserAgent", this.UserAgent));
            sqlCommand.Parameters.Add(new SqlParameter("@Status", this.StatusId));
            sqlCommand.Parameters.Add("@logUserId", SqlDbType.Int).Direction = ParameterDirection.Output;
            sqlCommand.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
            sqlCommand.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
            this.db.ExecuteSQL(sqlCommand);
        }

        public void Edit()
        {
            SqlCommand sqlCommand = new SqlCommand("UserLogs_Update");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.Add(new SqlParameter("@logUserId", this.logUserId));
            sqlCommand.Parameters.Add(new SqlParameter("@Username", this.UserName));
            sqlCommand.Parameters.Add(new SqlParameter("@Password", this.Password));
            sqlCommand.Parameters.Add(new SqlParameter("@CrDateTime", this.CrDateTime));
            sqlCommand.Parameters.Add(new SqlParameter("@IPAddress", this.IPAddress));
            sqlCommand.Parameters.Add(new SqlParameter("@UserAgent", this.UserAgent));
            sqlCommand.Parameters.Add(new SqlParameter("@Status", this.StatusId));
            sqlCommand.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
            sqlCommand.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
            this.db.ExecuteSQL(sqlCommand);
        }

        public void Delete()
        {
            SqlCommand sqlCommand = new SqlCommand("UserLogs_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.Add(new SqlParameter("@logUserId", this.logUserId));
            sqlCommand.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
            sqlCommand.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
            this.db.ExecuteSQL(sqlCommand);
        }

        public int GetlogUserId()
        {
            return this.logUserId;
        }

        public List<UserLogs> GetAllUserLogs()
        {
            string cmdText = "SELECT * FROM UserLogs ORDER BY  CrDateTime DESC";
            SqlCommand cmd = new SqlCommand(cmdText);
            return this.Init(cmd);
        }

        public List<UserLogs> GetByUsername(string username)
        {
            try
            {
                string cmdText = "SELECT * FROM UserLogs WHERE Username = '" + username + "'";
                SqlCommand cmd = new SqlCommand(cmdText);
                return this.Init(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UserLogs> GetByHour(string dateFrom, string dateTo, string username)
        {
            try
            {
                string cmdText = "SELECT * FROM UserLogs WHERE Username = '" + username + "'   AND CrDateTime  >'" + dateFrom + "' and  CrDateTime <= '" + dateTo + "' ORDER BY CrDateTime DESC";
                SqlCommand cmd = new SqlCommand(cmdText);
                return this.Init(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserLogs Get(int id)
        {
            try
            {
                string cmdText = "SELECT * FROM UserLogs WHERE (logUserId=" + id + ")";
                SqlCommand sqlCommand = new SqlCommand(cmdText);
                sqlCommand.CommandType = CommandType.Text;
                List<UserLogs> list = this.Init(sqlCommand);
                if (list.Count == 1)
                {
                    return list[0];
                }
                return new UserLogs();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertQuick()
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand("UserLogs_Insert_Quick");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@UserName", this.UserName));
                sqlCommand.Parameters.Add(new SqlParameter("@IPAddress", this.IPAddress));
                sqlCommand.Parameters.Add(new SqlParameter("@UserAgent", this.UserAgent));
                sqlCommand.Parameters.Add(new SqlParameter("@StatusId", this.StatusId));
                this.db.ExecuteSQL(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
