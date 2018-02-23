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
    public class ActionStatus
    {
        private byte _ActionStatusId;

        private string _ActionStatusName;

        private string _ActionStatusDesc;

        private DBAccess db;

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

        public string ActionStatusName
        {
            get
            {
                return this._ActionStatusName;
            }
            set
            {
                this._ActionStatusName = value;
            }
        }

        public string ActionStatusDesc
        {
            get
            {
                return this._ActionStatusDesc;
            }
            set
            {
                this._ActionStatusDesc = value;
            }
        }

        public ActionStatus()
        {
            this.db = new DBAccess(DbConstants.CONNECTION_STRING);
        }

        public ActionStatus(string constr)
        {
            this.db = new DBAccess((constr == "") ? DbConstants.CONNECTION_STRING : constr);
        }

        ~ActionStatus()
        {
        }

        public virtual void Dispose()
        {
        }

        private List<ActionStatus> Init(SqlCommand cmd)
        {
            SqlConnection sqlConnection = cmd.Connection = this.db.GetConnection();
            List<ActionStatus> list = new List<ActionStatus>();
            try
            {
                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartDataReader = new SmartDataReader(reader);
                while (smartDataReader.Read())
                {
                    ActionStatus actionStatus = new ActionStatus(this.db.ConnectionString);
                    actionStatus.ActionStatusId = smartDataReader.GetByte("ActionStatusId");
                    actionStatus.ActionStatusName = smartDataReader.GetString("ActionStatusName");
                    actionStatus.ActionStatusDesc = smartDataReader.GetString("ActionStatusDesc");
                    list.Add(actionStatus);
                }
                smartDataReader.disposeReader(reader);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                this.db.closeConnection(sqlConnection);
            }
            return list;
        }

        public List<ActionStatus> GetList()
        {
            List<ActionStatus> result = new List<ActionStatus>();
            try
            {
                string cmdText = "SELECT * FROM ActionStatus";
                SqlCommand cmd = new SqlCommand(cmdText);
                result = this.Init(cmd);
            }
            catch
            {
            }
            return result;
        }

        public static List<ActionStatus> Static_GetList(string constr)
        {
            List<ActionStatus> list = new List<ActionStatus>();
            try
            {
                ActionStatus actionStatus = new ActionStatus(constr);
                return actionStatus.GetList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ActionStatus> Static_GetList()
        {
            return ActionStatus.Static_GetList("");
        }

        public List<ActionStatus> GetListByActionStatusId(byte actionStatusId)
        {
            List<ActionStatus> result = new List<ActionStatus>();
            try
            {
                if (actionStatusId > 0)
                {
                    string cmdText = "SELECT * FROM V$ActionStatus WHERE (ActionStatusId=" + actionStatusId + ")";
                    SqlCommand sqlCommand = new SqlCommand(cmdText);
                    sqlCommand.CommandType = CommandType.Text;
                    result = this.Init(sqlCommand);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ActionStatus Get(byte actionStatusId)
        {
            ActionStatus actionStatus = new ActionStatus(this.db.ConnectionString);
            try
            {
                List<ActionStatus> listByActionStatusId = actionStatus.GetListByActionStatusId(actionStatusId);
                if (listByActionStatusId.Count > 0)
                {
                    actionStatus = listByActionStatusId[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return actionStatus;
        }

        public static string Static_GetDesc(string constr, byte actionStatusId)
        {
            string result = "";
            try
            {
                ActionStatus actionStatus = new ActionStatus(constr);
                actionStatus = actionStatus.Get(actionStatusId);
                if (actionStatus.ActionStatusDesc != null)
                {
                    result = ((actionStatus.ActionStatusId == 1) ? ("<font color=blue>" + actionStatus.ActionStatusDesc.Trim() + "</font>") : actionStatus.ActionStatusDesc.Trim());
                }
            }
            catch
            {
            }
            return result;
        }

        public static string Static_GetDesc(byte actionStatusId)
        {
            return ActionStatus.Static_GetDesc("", actionStatusId);
        }

        public static ActionStatus Static_Get(int actionStatusId, List<ActionStatus> listActionStatus)
        {
            var retVal = listActionStatus.Find(x => x.ActionStatusId == actionStatusId) ?? new ActionStatus();
            return retVal;
        }
    }
}
