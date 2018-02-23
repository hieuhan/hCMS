using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibDatabase;

namespace CMSLib
{
    public class RoleActions
    {
        private int _RoleActionId;

        private short _RoleId;

        private short _ActionId;

        private DBAccess db;

        public int RoleActionId
        {
            get
            {
                return this._RoleActionId;
            }
            set
            {
                this._RoleActionId = value;
            }
        }

        public short RoleId
        {
            get
            {
                return this._RoleId;
            }
            set
            {
                this._RoleId = value;
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

        public RoleActions()
        {
            this.db = new DBAccess(DbConstants.CONNECTION_STRING);
        }

        ~RoleActions()
        {
        }

        public virtual void Dispose()
        {
        }

        private List<RoleActions> Init(SqlCommand cmd)
        {
            List<RoleActions> list = new List<RoleActions>();
            try
            {
                SqlConnection sqlConnection = cmd.Connection = this.db.GetConnection();
                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartDataReader = new SmartDataReader(reader);
                while (smartDataReader.Read())
                {
                    RoleActions roleActions = new RoleActions();
                    roleActions.RoleActionId = smartDataReader.GetInt32("RoleActionId");
                    roleActions.ActionId = smartDataReader.GetInt16("ActionId");
                    roleActions.RoleId = smartDataReader.GetInt16("RoleId");
                    list.Add(roleActions);
                }
                smartDataReader.disposeReader(reader);
                this.db.closeConnection(sqlConnection);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return list;
        }

        public byte Insert(int actUserId, ref short sysMessageId)
        {
            byte b = 0;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("RoleActions_Insert");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@CrUserId", actUserId));
                sqlCommand.Parameters.Add(new SqlParameter("@RoleId", this.RoleId));
                sqlCommand.Parameters.Add(new SqlParameter("@ActionId", this.ActionId));
                sqlCommand.Parameters.Add("@RoleActionId", SqlDbType.Int).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                this.db.ExecuteSQL(sqlCommand);
                this.RoleActionId = Convert.ToInt32(sqlCommand.Parameters["@RoleActionId"].Value);
                sysMessageId = Convert.ToInt16(sqlCommand.Parameters["@SysMessageId"].Value ?? "0");
                return Convert.ToByte(sqlCommand.Parameters["@SysMessageTypeId"].Value ?? "0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertQuick(int actUserId)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand("RoleActions_Insert_Quick");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@CrUserId", actUserId));
                sqlCommand.Parameters.Add(new SqlParameter("@RoleId", this.RoleId));
                sqlCommand.Parameters.Add(new SqlParameter("@ActionId", this.ActionId));
                sqlCommand.Parameters.Add("@RoleActionId", SqlDbType.Int).Direction = ParameterDirection.Output;
                this.db.ExecuteSQL(sqlCommand);
                this.RoleActionId = Convert.ToInt32(sqlCommand.Parameters["@RoleActionId"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public short Update(int actUserId, ref short sysMessageId)
        {
            byte b = 0;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("RoleActions_Update");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@CrUserId", actUserId));
                sqlCommand.Parameters.Add(new SqlParameter("@RoleId", this.RoleId));
                sqlCommand.Parameters.Add(new SqlParameter("@ActionId", this.ActionId));
                sqlCommand.Parameters.Add(new SqlParameter("@RoleActionId", this.RoleActionId));
                sqlCommand.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                this.db.ExecuteSQL(sqlCommand);
                sysMessageId = Convert.ToInt16(sqlCommand.Parameters["@SysMessageId"].Value ?? "0");
                return Convert.ToByte(sqlCommand.Parameters["@SysMessageTypeId"].Value ?? "0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public byte Delete(int actUserId, ref short sysMessageId)
        {
            byte b = 0;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("RoleActions_Delete");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@CrUserId", actUserId));
                sqlCommand.Parameters.Add(new SqlParameter("@RoleActionId", this.RoleActionId));
                sqlCommand.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                this.db.ExecuteSQL(sqlCommand);
                sysMessageId = Convert.ToInt16(sqlCommand.Parameters["@SysMessageId"].Value ?? "0");
                return Convert.ToByte(sqlCommand.Parameters["@SysMessageTypeId"].Value ?? "0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteQuickBy(int actUserId)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand("RoleActions_Delete_QuickBy");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@CrUserId", actUserId));
                sqlCommand.Parameters.Add(new SqlParameter("@RoleId", this.RoleId));
                sqlCommand.Parameters.Add(new SqlParameter("@ActionId", this.ActionId));
                this.db.ExecuteSQL(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RoleActions Get(int roleActionId)
        {
            string cmdText = "SELECT * FROM RoleActions WHERE (RoleActionId = " + roleActionId + ")";
            SqlCommand cmd = new SqlCommand(cmdText);
            List<RoleActions> list = this.Init(cmd);
            if (list.Count == 1)
            {
                return list[0];
            }
            return new RoleActions();
        }

        public List<RoleActions> GetByRole(short roleId)
        {
            string cmdText = "SELECT * FROM RoleActions WHERE (RoleId = " + roleId + ")";
            SqlCommand sqlCommand = new SqlCommand(cmdText);
            sqlCommand.CommandType = CommandType.Text;
            return this.Init(sqlCommand);
        }

        public List<RoleActions> GetByActionId(short actionId)
        {
            string cmdText = "SELECT * FROM RoleActions WHERE (ActionId = " + actionId + ")";
            return this.Init(new SqlCommand(cmdText)
            {
                CommandType = CommandType.Text
            });
        }

        public List<RoleActions> GetAll()
        {
            string cmdText = "SELECT * FROM RoleActions";
            SqlCommand cmd = new SqlCommand(cmdText);
            return this.Init(cmd);
        }
    }

}
