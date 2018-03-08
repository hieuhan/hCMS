using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibDatabase;
using LibUtils;

namespace CMSLib
{
    public class Status
    {
        private byte _StatusID;
        private string _StatusName;
        private string _StatusDesc;
        private DBAccess db;
        //-----------------------------------------------------------------
        public Status()
        {
            db = new DBAccess(DbConstants.CONNECTION_STRING);
        }
        //-----------------------------------------------------------------        
        public Status(string constr)
        {
            db = new DBAccess((constr == "") ? DbConstants.CONNECTION_STRING : constr);
        }
        //-----------------------------------------------------------------
        ~Status()
        {

        }
        //-----------------------------------------------------------------
        public virtual void Dispose()
        {

        }
        //-----------------------------------------------------------------    
        public byte StatusID
        {
            get { return _StatusID; }
            set { _StatusID = value; }
        }
        //-----------------------------------------------------------------
        public string StatusName
        {
            get { return _StatusName; }
            set { _StatusName = value; }
        }
        //-----------------------------------------------------------------
        public string StatusDesc
        {
            get { return _StatusDesc; }
            set { _StatusDesc = value; }
        }
        //-----------------------------------------------------------------

        private List<Status> Init(SqlCommand cmd)
        {
            SqlConnection con = db.GetConnection();
            cmd.Connection = con;
            List<Status> l_Status = new List<Status>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    Status m_Status = new Status(db.ConnectionString);
                    m_Status.StatusID = smartReader.GetByte("StatusID");
                    m_Status.StatusName = smartReader.GetString("StatusName");
                    m_Status.StatusDesc = smartReader.GetString("StatusDesc");

                    l_Status.Add(m_Status);
                }
                reader.Close();
                return l_Status;
            }
            catch (SqlException err)
            {
                throw new ApplicationException("Data error: " + err.Message);
            }
            finally
            {
                db.closeConnection(con);
            }
        }
        //-----------------------------------------------------------
        public byte Insert(byte Replicated, int ActUserId, ref short SysMessageId)
        {
            byte RetVal = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("Status_Insert");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@StatusName", this.StatusName));
                cmd.Parameters.Add(new SqlParameter("@StatusDesc", this.StatusDesc));
                cmd.Parameters.Add("@StatusID", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                db.ExecuteSQL(cmd);
                this.StatusID = Convert.ToByte((cmd.Parameters["@StatusID"].Value == null) ? 0 : (cmd.Parameters["@StatusID"].Value));
                SysMessageId = Convert.ToInt16((cmd.Parameters["@SysMessageId"].Value == null) ? "0" : cmd.Parameters["@SysMessageId"].Value);
                RetVal = Convert.ToByte((cmd.Parameters["@SysMessageTypeId"].Value == null) ? "0" : cmd.Parameters["@SysMessageTypeId"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //--------------------------------------------------------------
        public byte Update(byte Replicated, int ActUserId, ref short SysMessageId)
        {
            byte RetVal = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("Status_Update");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@StatusName", this.StatusName));
                cmd.Parameters.Add(new SqlParameter("@StatusDesc", this.StatusDesc));
                cmd.Parameters.Add(new SqlParameter("@StatusID", this.StatusID));
                cmd.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                db.ExecuteSQL(cmd);
                SysMessageId = Convert.ToInt16((cmd.Parameters["@SysMessageId"].Value == null) ? "0" : cmd.Parameters["@SysMessageId"].Value);
                RetVal = Convert.ToByte((cmd.Parameters["@SysMessageTypeId"].Value == null) ? "0" : cmd.Parameters["@SysMessageTypeId"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //--------------------------------------------------------------            
        public byte Delete(byte Replicated, int ActUserId, ref short SysMessageId)
        {
            byte RetVal = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("Status_Delete");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@StatusID", this.StatusID));
                cmd.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                db.ExecuteSQL(cmd);
                SysMessageId = Convert.ToInt16((cmd.Parameters["@SysMessageId"].Value == null) ? "0" : cmd.Parameters["@SysMessageId"].Value);
                RetVal = Convert.ToByte((cmd.Parameters["@SysMessageTypeId"].Value == null) ? "0" : cmd.Parameters["@SysMessageTypeId"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public List<Status> GetList()
        {
            List<Status> RetVal = new List<Status>();
            try
            {
                string sql = "SELECT * FROM Status";
                SqlCommand cmd = new SqlCommand(sql);
                RetVal = Init(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //--------------------------------------------------------------
        public static List<Status> Static_GetList(string ConStr)
        {
            List<Status> RetVal = new List<Status>();
            try
            {
                Status m_Status = new Status(ConStr);
                RetVal = m_Status.GetList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //--------------------------------------------------------------     
        public static List<Status> Static_GetList()
        {
            return Static_GetList("");
        }
        //--------------------------------------------------------------    
        public List<Status> GetListOrderBy(string OrderBy)
        {
            List<Status> RetVal = new List<Status>();
            try
            {
                OrderBy = StringUtil.InjectionString(OrderBy).Trim();
                string sql = "SELECT * FROM Status ";
                if (OrderBy.Length > 0)
                {
                    sql += "ORDER BY " + OrderBy;
                }
                SqlCommand cmd = new SqlCommand(sql);
                RetVal = Init(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public static List<Status> Static_GetListOrderBy(string ConStr, string OrderBy)
        {
            Status m_Status = new Status(ConStr);
            return m_Status.GetListOrderBy(OrderBy);
        }
        //--------------------------------------------------------------    
        public static List<Status> Static_GetListOrderBy(string OrderBy)
        {
            return Static_GetListOrderBy("", OrderBy);
        }
        //--------------------------------------------------------------    
        public static List<Status> Static_GetListAllOrderBy(string ConStr, string OrderBy, string TextOptionAll = "...")
        {
            List<Status> RetVal = new List<Status>();
            Status m_Status = new Status(ConStr);
            try
            {
                OrderBy = StringUtil.InjectionString(OrderBy).Trim();
                if (OrderBy.Length > 0)
                {
                    RetVal = m_Status.GetListOrderBy(OrderBy);
                }
                TextOptionAll = (TextOptionAll == null) ? "" : TextOptionAll.Trim();
                if (TextOptionAll.Length > 0)
                {
                    m_Status.StatusName = TextOptionAll;
                    m_Status.StatusDesc = TextOptionAll;
                    RetVal.Insert(0, m_Status);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public static List<Status> Static_GetListAllOrderBy(string OrderBy, string TextOptionAll = "...")
        {
            return Static_GetListAllOrderBy("", OrderBy, TextOptionAll);
        }
        //--------------------------------------------------------------  
        public List<Status> GetListByStatusID(byte StatusID)
        {
            List<Status> RetVal = new List<Status>();
            try
            {
                if (StatusID > 0)
                {
                    string sql = "SELECT * FROM Status WHERE (StatusID=" + StatusID.ToString() + ")";
                    SqlCommand cmd = new SqlCommand(sql);
                    RetVal = Init(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }

        //-------------------------------------------------------------- 
        public Status Get(byte StatusID)
        {
            Status RetVal = new Status(db.ConnectionString);
            try
            {
                List<Status> list = GetListByStatusID(StatusID);
                if (list.Count > 0)
                {
                    RetVal = (Status)list[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public Status Get()
        {
            return Get(this.StatusID);
        }
        //-------------------------------------------------------------- 
        public static Status Static_Get(byte StatusID)
        {
            return Static_Get(StatusID);
        }
        //-----------------------------------------------------------------------------
        public static Status Static_Get(byte StatusID, List<Status> lList)
        {
            Status RetVal = new Status();
            if (StatusID > 0 && lList.Count > 0)
            {
                RetVal = lList.Find(i => i.StatusID == StatusID) ?? new Status();
            }
            return RetVal;
        }
        //--------------------------------------------------------------
    }
}
