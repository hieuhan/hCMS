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
    public class UserActions
    {
        private int _UserActionId;

        private int _UserId;

        private short _ActionId;

        private DBAccess db;

        public int UserActionId
        {
            get
            {
                return this._UserActionId;
            }
            set
            {
                this._UserActionId = value;
            }
        }

        public int UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                this._UserId = value;
            }
        }

        public short ActionId
        {
            get
            {
                return this._ActionId;
            }
            set
            {
                this._ActionId = value;
            }
        }

        public UserActions()
        {
            this.db = new DBAccess(DbConstants.CONNECTION_STRING);
        }

        ~UserActions()
        {
        }

        public virtual void Dispose()
        {
        }

        private List<UserActions> Init(SqlCommand cmd)
        {
            SqlConnection sqlConnection = cmd.Connection = this.db.GetConnection();
            List<UserActions> list = new List<UserActions>();
            try
            {
                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartDataReader = new SmartDataReader(reader);
                while (smartDataReader.Read())
                {
                    UserActions userActions = new UserActions
                    {
                        UserActionId = smartDataReader.GetInt32("UserActionId"),
                        ActionId = smartDataReader.GetInt16("ActionId"),
                        UserId = smartDataReader.GetInt32("UserId")
                    };
                    list.Add(userActions);
                }
                smartDataReader.disposeReader(reader);
                this.db.closeConnection(sqlConnection);
                return list;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public short Insert(int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                SqlCommand sqlCommand =
                    new SqlCommand("UserActions_Insert") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@CrUserId", actUserId));
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", this.UserId));
                sqlCommand.Parameters.Add(new SqlParameter("@ActionId", this.ActionId));
                sqlCommand.Parameters.Add("@UserActionId", SqlDbType.Int).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                this.db.ExecuteSQL(sqlCommand);
                this.UserActionId = Convert.ToInt32(sqlCommand.Parameters["@UserActionId"].Value ?? "0");
                sysMessageId = Convert.ToInt16(sqlCommand.Parameters["@SysMessageId"].Value ?? "0");
                retVal = Convert.ToByte(sqlCommand.Parameters["@SysMessageTypeId"].Value ?? "0");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retVal;
        }

        public void InsertQuick()
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand("UserActions_Insert_Quick");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", this.UserId));
                sqlCommand.Parameters.Add(new SqlParameter("@ActionId", this.ActionId));
                sqlCommand.Parameters.Add("@UserActionId", SqlDbType.Int).Direction = ParameterDirection.Output;
                this.db.ExecuteSQL(sqlCommand);
                this.UserActionId = Convert.ToInt32(sqlCommand.Parameters["@UserActionId"].Value ?? "0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public byte Update(int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                SqlCommand sqlCommand =
                    new SqlCommand("UserActions_Update") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@CrUserId", actUserId));
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", this.UserId));
                sqlCommand.Parameters.Add(new SqlParameter("@ActionId", this.ActionId));
                sqlCommand.Parameters.Add(new SqlParameter("@UserActionId", this.UserActionId));
                sqlCommand.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                this.db.ExecuteSQL(sqlCommand);
                sysMessageId = Convert.ToInt16(sqlCommand.Parameters["@SysMessageId"].Value ?? "0");
                retVal = Convert.ToByte(sqlCommand.Parameters["@SysMessageTypeId"].Value ?? "0");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retVal;
        }

        public byte Delete(int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                SqlCommand sqlCommand =
                    new SqlCommand("UserActions_Delete") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@CrUserId", actUserId));
                sqlCommand.Parameters.Add(new SqlParameter("@UserActionId", this.UserActionId));
                sqlCommand.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                this.db.ExecuteSQL(sqlCommand);
                sysMessageId = Convert.ToInt16(sqlCommand.Parameters["@SysMessageId"].Value ?? "0");
                retVal = Convert.ToByte(sqlCommand.Parameters["@SysMessageTypeId"].Value ?? "0");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retVal;
        }

        public void DeleteQuickBy()
        {
            try
            {
                SqlCommand sqlCommand =
                    new SqlCommand("UserActions_Delete_QuickBy") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", this.UserId));
                sqlCommand.Parameters.Add(new SqlParameter("@ActionId", this.ActionId));
                this.db.ExecuteSQL(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserActions Get(int userActionId)
        {
            string cmdText = "SELECT * FROM V$UserActions WHERE (UserActionId =" + userActionId + ")";
            SqlCommand cmd = new SqlCommand(cmdText);
            List<UserActions> list = this.Init(cmd);
            if (list.Count == 1)
            {
                return list[0];
            }
            return new UserActions();
        }

        public List<UserActions> GetByUser(int userId)
        {
            string cmdText = "SELECT * FROM UserActions WHERE (UserId = " + userId + ")";
            SqlCommand sqlCommand = new SqlCommand(cmdText) { CommandType = CommandType.Text };
            return this.Init(sqlCommand);
        }

        public List<UserActions> GetAll()
        {
            string cmdText = "SELECT * FROM V$UserActions";
            SqlCommand cmd = new SqlCommand(cmdText);
            return this.Init(cmd);
        }
    }
}
