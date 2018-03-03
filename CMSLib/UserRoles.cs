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
    public class UserRoles
    {
        private int _UserRoleId;

        private short _RoleId;

        private int _UserId;

        private DBAccess db;

        public int UserRoleId
        {
            get
            {
                return this._UserRoleId;
            }
            set
            {
                this._UserRoleId = value;
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

        public UserRoles()
        {
            this.db = new DBAccess(DbConstants.CONNECTION_STRING);
        }

        public UserRoles(string constr)
        {
            this.db = ((constr.Length > 0) ? new DBAccess(constr) : new DBAccess(DbConstants.CONNECTION_STRING));
        }

        ~UserRoles()
        {
        }

        public virtual void Dispose()
        {
        }

        private List<UserRoles> Init(SqlCommand cmd)
        {
            SqlConnection sqlConnection = cmd.Connection = this.db.GetConnection();
            List<UserRoles> list = new List<UserRoles>();
            try
            {
                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartDataReader = new SmartDataReader(reader);
                while (smartDataReader.Read())
                {
                    UserRoles userRoles = new UserRoles
                    {
                        UserRoleId = smartDataReader.GetInt32("UserRoleId"),
                        RoleId = smartDataReader.GetInt16("RoleId"),
                        UserId = smartDataReader.GetInt32("UserId")
                    };
                    list.Add(userRoles);
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

        public short Insert(int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("UserRoles_Insert") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@CrUserId", actUserId));
                sqlCommand.Parameters.Add(new SqlParameter("@RoleId", this.RoleId));
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", this.UserId));
                sqlCommand.Parameters.Add("@UserRoleId", SqlDbType.Int).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                this.db.ExecuteSQL(sqlCommand);
                this.UserRoleId = Convert.ToInt32(sqlCommand.Parameters["@UserRoleId"].Value ?? "0");
                sysMessageId = Convert.ToInt16(sqlCommand.Parameters["@SysMessageId"].Value ?? "0");
                retVal = Convert.ToByte(sqlCommand.Parameters["@SysMessageTypeId"].Value ?? "0");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retVal;
        }

        public short Update(int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("UserRoles_Update") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@CrUserId", actUserId));
                sqlCommand.Parameters.Add(new SqlParameter("@UserRoleId", this.UserRoleId));
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", this.UserId));
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

        public short Delete(int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("UserRoles_Delete") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@CrUserId", actUserId));
                sqlCommand.Parameters.Add(new SqlParameter("@UserRoleId", this.UserRoleId));
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

        public void InsertQuick(int actUserId)
        {
            try
            {
                SqlCommand sqlCommand =
                    new SqlCommand("UserRoles_Insert_Quick") { CommandType = CommandType.StoredProcedure };
                //sqlCommand.Parameters.Add(new SqlParameter("@CrUserId", actUserId));
                sqlCommand.Parameters.Add(new SqlParameter("@RoleId", this.RoleId));
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", this.UserId));
                sqlCommand.Parameters.Add("@UserRoleId", SqlDbType.Int).Direction = ParameterDirection.Output;
                this.db.ExecuteSQL(sqlCommand);
                this.UserRoleId = Convert.ToInt32(sqlCommand.Parameters["@UserRoleId"].Value ?? "0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQuick(int actUserId)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand("UserRoles_Update_Quick");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@CrUserId", actUserId));
                sqlCommand.Parameters.Add(new SqlParameter("@UserRoleId", this.UserRoleId));
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", this.UserId));
                sqlCommand.Parameters.Add(new SqlParameter("@RoleId", this.RoleId));
                this.db.ExecuteSQL(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteQuick(int actUserId)
        {
            try
            {
                SqlCommand sqlCommand =
                    new SqlCommand("UserRoles_Delete_Quick") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@CrUserId", actUserId));
                sqlCommand.Parameters.Add(new SqlParameter("@UserRoleId", this.UserRoleId));
                this.db.ExecuteSQL(sqlCommand);
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
                SqlCommand sqlCommand =
                    new SqlCommand("UserRoles_Delete_QuickBy") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@CrUserId", actUserId));
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", this.UserId));
                sqlCommand.Parameters.Add(new SqlParameter("@RoleId", this.RoleId));
                this.db.ExecuteSQL(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public UserRoles Get(int userRoleId)
        {
            UserRoles result = new UserRoles();
            try
            {
                if (userRoleId > 0)
                {
                    string cmdText = "SELECT * FROM V$UserRoles WHERE (UserRoleId=" + userRoleId + ")";
                    SqlCommand sqlCommand = new SqlCommand(cmdText) { CommandType = CommandType.Text };
                    List<UserRoles> list = this.Init(sqlCommand);
                    result = list[0];
                }
            }
            catch
            {
                result = new UserRoles();
            }
            return result;
        }

        public List<UserRoles> GetListByUserId(int userId)
        {
            string cmdText = "SELECT * FROM UserRoles WHERE (UserId = " + userId + ")";
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

        public List<UserRoles> GetListByRoleId(short roleId)
        {
            string cmdText = "SELECT * FROM V$UserRoles WHERE (RoleId = " + roleId + ")";
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

        public List<UserRoles> GetAll()
        {
            string cmdText = "SELECT * FROM V$UserRoles";
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

        public List<UserRoles> GetAllOrderByUser()
        {
            string cmdText = "SELECT * FROM V$UserRoles ORDER BY UserId";
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

        public UserRoles Get(int userId, string roleName)
        {
            UserRoles userRoles = new UserRoles(this.db.ConnectionString);
            try
            {
                if (userId > 0 && roleName.Length > 0)
                {
                    string cmdText = "SELECT * FROM V$UserRoles WHERE (UserId=" + userId + ") AND (RoleId=[dbo].[Roles_GetIdByName] ('" + roleName + "'))";
                    SqlCommand sqlCommand = new SqlCommand(cmdText) { CommandType = CommandType.Text };
                    List<UserRoles> list = userRoles.Init(sqlCommand);
                    if (list.Count >= 1)
                    {
                        userRoles = list[0];
                    }
                }
            }
            catch
            {
            }
            return userRoles;
        }

        public UserRoles GetAdmin(int userId)
        {
            return this.GetAdmin(userId, "");
        }

        public UserRoles GetAdmin(int userId, string constr)
        {
            UserRoles userRoles = new UserRoles(constr);
            try
            {
                if (userId > 0)
                {
                    string cmdText = "SELECT * FROM V$UserRoles WHERE (UserId=" + userId + ") AND (RoleId=[dbo].[Roles_GetAdminRoleId]())";
                    SqlCommand sqlCommand = new SqlCommand(cmdText) { CommandType = CommandType.Text };
                    List<UserRoles> list = userRoles.Init(sqlCommand);
                    if (list.Count >= 1)
                    {
                        userRoles = list[0];
                    }
                }
            }
            catch
            {
                userRoles = new UserRoles(constr);
            }
            return userRoles;
        }

        public UserRoles GetCustomerCare(int userId)
        {
            return this.GetCustomerCare(userId, "");
        }

        public UserRoles GetCustomerCare(int userId, string constr)
        {
            UserRoles userRoles = new UserRoles(constr);
            try
            {
                if (userId > 0)
                {
                    string cmdText = "SELECT * FROM V$UserRoles WHERE (UserId=" + userId + ") AND (RoleId=[dbo].[Roles_GetCustomerCareRoleId]())";
                    SqlCommand sqlCommand = new SqlCommand(cmdText) { CommandType = CommandType.Text };
                    List<UserRoles> list = userRoles.Init(sqlCommand);
                    if (list.Count >= 1)
                    {
                        userRoles = list[0];
                    }
                }
            }
            catch
            {
                userRoles = new UserRoles(constr);
            }
            return userRoles;
        }

        public static bool IsAdmin(string constr, int userId)
        {
            bool result = false;
            try
            {
                UserRoles userRoles = new UserRoles(constr);
                userRoles = userRoles.Get(userId, DbConstants.ROLENAME_ADMIN);
                if (userRoles.UserId > 0)
                {
                    result = true;
                }
            }
            catch
            {
            }
            return result;
        }

        public static bool IsAdmin(int userId)
        {
            return UserRoles.IsAdmin("", userId);
        }
    }
}
