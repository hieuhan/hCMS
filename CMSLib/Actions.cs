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
    public class Actions
    {
        private short _ActionId;

        private string _ActionName;

        private string _ActionDesc;

        private short _ParentActionId;

        private string _Url;

        private byte _ActionStatusId;

        private byte _Levels;

        private byte _Display;

        private short _ActionOrder;

        private short _LevelId;

        private DBAccess db;

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

        public string ActionName
        {
            get
            {
                return this._ActionName;
            }
            set
            {
                this._ActionName = value;
            }
        }

        public byte ActionStatusId
        {
            get
            {
                return this._ActionStatusId;
            }
            set
            {
                this._ActionStatusId = value;
            }
        }

        public string ActionDesc
        {
            get
            {
                return this._ActionDesc;
            }
            set
            {
                this._ActionDesc = value;
            }
        }

        public byte Display
        {
            get
            {
                return this._Display;
            }
            set
            {
                this._Display = value;
            }
        }

        public short ActionOrder
        {
            get
            {
                return this._ActionOrder;
            }
            set
            {
                this._ActionOrder = value;
            }
        }

        public string Url
        {
            get
            {
                return this._Url;
            }
            set
            {
                this._Url = value;
            }
        }

        public short ParentActionId
        {
            get
            {
                return this._ParentActionId;
            }
            set
            {
                this._ParentActionId = value;
            }
        }

        public short LevelId
        {
            get
            {
                return this._LevelId;
            }
            set
            {
                this._LevelId = value;
            }
        }

        public byte Levels
        {
            get
            {
                return this._Levels;
            }
            set
            {
                this._Levels = value;
            }
        }

        public Actions()
        {
            this.db = new DBAccess(DbConstants.CONNECTION_STRING);
        }

        public Actions(string constr)
        {
            this.db = new DBAccess((constr == "") ? DbConstants.CONNECTION_STRING : constr);
        }

        ~Actions()
        {
        }

        public virtual void Dispose()
        {
        }

        private List<Actions> Init(SqlCommand cmd)
        {
            SqlConnection sqlConnection = cmd.Connection = this.db.GetConnection();
            List<Actions> listActions = new List<Actions>();
            try
            {
                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartDataReader = new SmartDataReader(reader);
                while (smartDataReader.Read())
                {
                    Actions actions = new Actions
                    {
                        ActionId = smartDataReader.GetInt16("ActionId"),
                        ActionName = smartDataReader.GetString("ActionName"),
                        ActionDesc = smartDataReader.GetString("ActionDesc"),
                        Url = smartDataReader.GetString("Url"),
                        ActionStatusId = smartDataReader.GetByte("ActionStatusId"),
                        ParentActionId = smartDataReader.GetInt16("ParentActionId"),
                        Levels = smartDataReader.GetByte("Levels"),
                        Display = smartDataReader.GetByte("Display"),
                        LevelId = smartDataReader.GetInt16("LevelId"),
                        ActionOrder = smartDataReader.GetInt16("ActionOrder")
                    };
                    listActions.Add(actions);
                }
                smartDataReader.disposeReader(reader);
                this.db.closeConnection(sqlConnection);
                return listActions;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public byte Insert(byte replicated, int actUserId, ref short sysMessageId)
        {
            byte b = 0;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("Actions_Insert") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@ActionName", this.ActionName));
                sqlCommand.Parameters.Add(new SqlParameter("@Url", this.Url));
                sqlCommand.Parameters.Add(new SqlParameter("@ActionDesc", this.ActionDesc));
                sqlCommand.Parameters.Add(new SqlParameter("@ParentActionId", this.ParentActionId));
                sqlCommand.Parameters.Add(new SqlParameter("@ActionOrder", this.ActionOrder));
                sqlCommand.Parameters.Add(new SqlParameter("@Display", this.Display));
                sqlCommand.Parameters.Add(new SqlParameter("@LevelId", this.LevelId));
                sqlCommand.Parameters.Add(new SqlParameter("@ActionStatusId", this.ActionStatusId));
                sqlCommand.Parameters.Add("@ActionId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                this.db.ExecuteSQL(sqlCommand);
                this.ActionId = Convert.ToInt16(sqlCommand.Parameters["@ActionId"].Value);
                sysMessageId = Convert.ToInt16(sqlCommand.Parameters["@SysMessageId"].Value ?? "0");
                return Convert.ToByte(sqlCommand.Parameters["@SysMessageTypeId"].Value ?? "0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public byte Update(byte replicated, int actUserId, ref short sysMessageId)
        {
            byte b = 0;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("Actions_Update") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@ActionName", this.ActionName));
                sqlCommand.Parameters.Add(new SqlParameter("@Url", this.Url));
                sqlCommand.Parameters.Add(new SqlParameter("@ActionDesc", this.ActionDesc));
                sqlCommand.Parameters.Add(new SqlParameter("@ParentActionId", this.ParentActionId));
                sqlCommand.Parameters.Add(new SqlParameter("@ActionOrder", this.ActionOrder));
                sqlCommand.Parameters.Add(new SqlParameter("@Display", this.Display));
                sqlCommand.Parameters.Add(new SqlParameter("@ActionStatusId", this.ActionStatusId));
                sqlCommand.Parameters.Add(new SqlParameter("@LevelId", this.LevelId));
                sqlCommand.Parameters.Add(new SqlParameter("@ActionId", this.ActionId));
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

        //-----------------------------------------------------------
        public byte InsertOrUpdate(byte replicated, int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("Actions_InsertOrUpdate") {CommandType = CommandType.StoredProcedure};
                cmd.Parameters.Add(new SqlParameter("@ActUserId", actUserId));
                cmd.Parameters.Add(new SqlParameter("@ActionName", this.ActionName));
                cmd.Parameters.Add(new SqlParameter("@ActionDesc", this.ActionDesc));
                cmd.Parameters.Add(new SqlParameter("@ParentActionId", this.ParentActionId));
                cmd.Parameters.Add(new SqlParameter("@LevelId", this.LevelId));
                cmd.Parameters.Add(new SqlParameter("@Url", this.Url));
                cmd.Parameters.Add(new SqlParameter("@ActionStatusId", this.ActionStatusId));
                cmd.Parameters.Add(new SqlParameter("@Display", this.Display));
                cmd.Parameters.Add(new SqlParameter("@ActionOrder", this.ActionOrder));
                cmd.Parameters.Add(new SqlParameter("@ActionId", this.ActionId)).Direction = ParameterDirection.InputOutput;
                cmd.Parameters.Add("@SysMessageId", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                db.ExecuteSQL(cmd);
                this.ActionId = short.Parse(cmd.Parameters["@ActionId"].Value.ToString());
                sysMessageId = Convert.ToInt16(cmd.Parameters["@SysMessageId"].Value ?? "0");
                retVal = Convert.ToByte(cmd.Parameters["@SysMessageTypeId"].Value ?? "0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        //--------------------------------------------------------------       

        public byte Delete(byte replicated, int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("Actions_Delete") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@ActionId", this.ActionId));
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

        public List<Actions> GetList()
        {
            string cmdText = "SELECT * FROM V$Actions";
            SqlCommand cmd = new SqlCommand(cmdText);
            return this.Init(cmd);
        }

        public List<Actions> GetListActiondesList()
        {
            string cmdText = "SELECT * FROM V$ActionList order by  [ParentActionId]";
            SqlCommand cmd = new SqlCommand(cmdText);
            return this.Init(cmd);
        }

        public List<Actions> GetRoots()
        {
            string cmdText = "SELECT * FROM V$ActionsRoots";
            SqlCommand cmd = new SqlCommand(cmdText);
            return this.Init(cmd);
        }

        public List<Actions> GetRootsNA()
        {
            string cmdText = "SELECT * FROM V$ActionsRootsNA";
            SqlCommand cmd = new SqlCommand(cmdText);
            return this.Init(cmd);
        }

        public List<Actions> GetListNA()
        {
            string cmdText = "SELECT * FROM V$ActionsNA";
            SqlCommand cmd = new SqlCommand(cmdText);
            return this.Init(cmd);
        }

        public List<Actions> GetAllHierachy()
        {
            try
            {
                SqlCommand sqlCommand =
                    new SqlCommand("Actions_GetAllHierachy") { CommandType = CommandType.StoredProcedure };
                return this.Init(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Actions> GetAllHierachy2(string paddingChar)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand("Actions_GetAllHierachy");
                if (!string.IsNullOrEmpty(paddingChar))
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@PaddingChar", paddingChar));
                }
                sqlCommand.CommandType = CommandType.StoredProcedure;
                return this.Init(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Actions GetDefaultForUser(int userId)
        {
            try
            {
                SqlCommand sqlCommand =
                    new SqlCommand("Actions_GetDefaultForUser") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", userId));
                List<Actions> listActions = this.Init(sqlCommand);
                if (listActions.Count >= 1)
                {
                    return listActions[0] as Actions;
                }
                return new Actions();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Actions Get(short id)
        {
            try
            {
                string cmdText = "SELECT * FROM Actions WHERE (ActionId =" + id + ")";
                SqlCommand sqlCommand = new SqlCommand(cmdText) { CommandType = CommandType.Text };
                List<Actions> listActions = this.Init(sqlCommand);
                if (listActions.Count >= 1)
                {
                    return listActions[0] as Actions;
                }
                return new Actions();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static short GetActionId(string url)
        {
            short result = 0;
            try
            {
                Actions actions = new Actions();
                actions = actions.Get(url);
                if (actions.ActionId > 0)
                {
                    result = actions.ActionId;
                }
            }
            catch
            {
            }
            return result;
        }

        public static string GetDesc(short actionId)
        {
            string result = "";
            try
            {
                if (actionId > 0)
                {
                    Actions actions = new Actions();
                    actions = actions.Get(actionId);
                    if (actions.ActionId == actionId)
                    {
                        result = actions.ActionDesc.Trim();
                    }
                }
            }
            catch
            {
            }
            return result;
        }

        public Actions Get(string url)
        {
            Actions result = new Actions();
            try
            {
                if (url.Length > 0)
                {
                    string cmdText = "SELECT * FROM V$Actions WHERE (CHARINDEX(Url,N'" + url + "')>0)";
                    SqlCommand sqlCommand = new SqlCommand(cmdText) { CommandType = CommandType.Text };
                    List<Actions> listActions = this.Init(sqlCommand);
                    if (listActions.Count >= 1)
                    {
                        result = (Actions)listActions[0];
                    }
                }
            }
            catch
            {
            }
            return result;
        }

        public Actions GetNA(short id)
        {
            try
            {
                string cmdText = "SELECT * FROM V$ActionsNA WHERE (ActionId =" + id + ")";
                SqlCommand sqlCommand = new SqlCommand(cmdText) { CommandType = CommandType.Text };
                List<Actions> listActions = this.Init(sqlCommand);
                if (listActions.Count >= 1)
                {
                    return listActions[0] as Actions;
                }
                return new Actions();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Actions GetNone(short id)
        {
            try
            {
                string cmdText = "SELECT * FROM V$ActionsNone WHERE (ActionId =" + id + ")";
                SqlCommand sqlCommand = new SqlCommand(cmdText) { CommandType = CommandType.Text };
                List<Actions> listActions = this.Init(sqlCommand);
                if (listActions.Count >= 1)
                {
                    return listActions[0] as Actions;
                }
                return new Actions();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Actions> GetAllReports()
        {
            string cmdText = "SELECT * FROM V$Actions ORDER BY ParentActionId, ActionId";
            SqlCommand cmd = new SqlCommand(cmdText);
            return this.Init(cmd);
        }

        public List<Actions> GetActionsByRole(short roleId)
        {
            string cmdText = "SELECT R.* FROM Actions R, RoleActions G WHERE R.ActionId=G.ActionId and G.RoleId=@RoleId";
            SqlCommand sqlCommand = new SqlCommand(cmdText);
            sqlCommand.Parameters.Add(new SqlParameter("@RoleId", roleId));
            return this.Init(sqlCommand);
        }

        public List<Actions> GetActionsByUser(int userId)
        {
            try
            {
                SqlCommand sqlCommand =
                    new SqlCommand("Actions_GetRootForUser") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", userId));
                return this.Init(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Actions> GetAll(int userId)
        {
            try
            {
                SqlCommand sqlCommand =
                    new SqlCommand("Actions_GetAllForUser") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", userId));
                return this.Init(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Actions> GetNotRootForUser(int userId)
        {
            try
            {
                SqlCommand sqlCommand =
                    new SqlCommand("Actions_GetNotRootForUser") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", userId));
                return this.Init(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Actions> GetChildActionByUser(int userId, short actionId)
        {
            try
            {
                SqlCommand sqlCommand =
                    new SqlCommand("Actions_GetChildByUser") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", userId));
                sqlCommand.Parameters.Add(new SqlParameter("@ActionId", actionId));
                return this.Init(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
