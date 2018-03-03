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
    public class Roles
    {
        private short _RoleId;

        private string _RoleName;

        private string _RoleDesc;

        private byte _BuildIn;

        private DBAccess db;

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

        public string RoleName
        {
            get
            {
                return this._RoleName;
            }
            set
            {
                this._RoleName = value;
            }
        }

        public string RoleDesc
        {
            get
            {
                return this._RoleDesc;
            }
            set
            {
                this._RoleDesc = value;
            }
        }

        public byte BuildIn
        {
            get
            {
                return this._BuildIn;
            }
            set
            {
                this._BuildIn = value;
            }
        }

        public Roles()
        {
            this.db = new DBAccess(DbConstants.CONNECTION_STRING);
        }

        public Roles(string constr)
        {
            this.db = constr.Length > 0 ? new DBAccess(constr) : new DBAccess(DbConstants.CONNECTION_STRING);
        }

        ~Roles()
        {
        }

        public virtual void Dispose()
        {
        }

        private List<Roles> Init(SqlCommand cmd)
        {
            SqlConnection sqlConnection = cmd.Connection = this.db.GetConnection();
            List<Roles> list = new List<Roles>();
            try
            {
                sqlConnection.Open();
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                SmartDataReader smartDataReader = new SmartDataReader(sqlDataReader);
                while (smartDataReader.Read())
                {
                    var roles = new Roles
                    {
                        RoleId = smartDataReader.GetInt16("RoleId"),
                        RoleName = smartDataReader.GetString("RoleName"),
                        RoleDesc = smartDataReader.GetString("RoleDesc"),
                        BuildIn = smartDataReader.GetByte("BuildIn")
                    };
                    list.Add(roles);
                }
                sqlDataReader.Close();
                return list;
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Data error: " + ex.Message);
            }
            finally
            {
                this.db.closeConnection(sqlConnection);
            }
        }

        public byte Insert(int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("Roles_Insert") { CommandType = CommandType.StoredProcedure };
                //sqlCommand.Parameters.Add(new SqlParameter("@CrUserId", actUserId));
                sqlCommand.Parameters.Add(new SqlParameter("@RoleName", this.RoleName));
                sqlCommand.Parameters.Add(new SqlParameter("@RoleDesc", this.RoleDesc));
                sqlCommand.Parameters.Add("@RoleId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                this.db.ExecuteSQL(sqlCommand);
                this.RoleId = Convert.ToInt16(sqlCommand.Parameters["@RoleId"].Value ?? "0");
                sysMessageId = Convert.ToInt16(sqlCommand.Parameters["@SysMessageId"].Value ?? "0");
                retVal = Convert.ToByte(sqlCommand.Parameters["@SysMessageTypeId"].Value ?? "0");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retVal;
        }

        public byte Update(int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("Roles_Update") { CommandType = CommandType.StoredProcedure };
                //sqlCommand.Parameters.Add(new SqlParameter("@CrUserId", actUserId));
                sqlCommand.Parameters.Add(new SqlParameter("@RoleName", this.RoleName));
                sqlCommand.Parameters.Add(new SqlParameter("@RoleDesc", this.RoleDesc));
                sqlCommand.Parameters.Add(new SqlParameter("@RoleId", this.RoleId));
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
                SqlCommand sqlCommand = new SqlCommand("Roles_Delete") { CommandType = CommandType.StoredProcedure };
                //sqlCommand.Parameters.Add(new SqlParameter("@CrUserId", actUserId));
                sqlCommand.Parameters.Add(new SqlParameter("@RoleId", this.RoleId));
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

        public Roles Get(short roleId)
        {
            string cmdText = "SELECT * FROM Roles WHERE (RoleId =" + roleId + ")";
            try
            {
                SqlCommand sqlCommand = new SqlCommand(cmdText) { CommandType = CommandType.Text };
                List<Roles> list = this.Init(sqlCommand);
                if (list.Count >= 1)
                {
                    return list[0];
                }
                return new Roles();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Roles> GetListOrderByName()
        {
            string cmdText = "SELECT * FROM V$Roles ORDER BY RoleName";
            SqlCommand sqlCommand = new SqlCommand(cmdText) { CommandType = CommandType.Text };
            try
            {
                return this.Init(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Roles> GetAll()
        {
            string cmdText = "SELECT * FROM Roles";
            SqlCommand sqlCommand = new SqlCommand(cmdText) { CommandType = CommandType.Text };
            try
            {
                return this.Init(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Roles> GetRoleByUserId(int userId)
        {
            List<Roles> list = new List<Roles>();
            try
            {
                SqlCommand sqlCommand = new SqlCommand("Roles_GetByUserId") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", userId));
                list = this.Init(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }
    }
}
