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
    public class UserSites
    {
        private int _UserSiteId;
        private int _UserId;
        private short _SiteId;
        private int _CrUserId;
        private DateTime _CrDateTime;
        private DBAccess db;
        //-----------------------------------------------------------------
        public UserSites()
        {
            db = new DBAccess(DbConstants.CONNECTION_STRING);
        }
        //-----------------------------------------------------------------        
        public UserSites(string constr)
        {
            db = new DBAccess((constr == "") ? DbConstants.CONNECTION_STRING : constr);
        }
        //-----------------------------------------------------------------
        ~UserSites()
        {

        }
        //-----------------------------------------------------------------
        public virtual void Dispose()
        {

        }
        //-----------------------------------------------------------------    
        public int UserSiteId
        {
            get { return _UserSiteId; }
            set { _UserSiteId = value; }
        }
        //-----------------------------------------------------------------
        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }
        //-----------------------------------------------------------------
        public short SiteId
        {
            get { return _SiteId; }
            set { _SiteId = value; }
        }
        //-----------------------------------------------------------------
        public int CrUserId
        {
            get { return _CrUserId; }
            set { _CrUserId = value; }
        }
        //-----------------------------------------------------------------
        public DateTime CrDateTime
        {
            get { return _CrDateTime; }
            set { _CrDateTime = value; }
        }
        //-----------------------------------------------------------------

        private List<UserSites> Init(SqlCommand cmd)
        {
            SqlConnection con = db.GetConnection();
            cmd.Connection = con;
            List<UserSites> l_UserSites = new List<UserSites>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    UserSites m_UserSites = new UserSites(db.ConnectionString);
                    m_UserSites.UserSiteId = smartReader.GetInt32("UserSiteId");
                    m_UserSites.UserId = smartReader.GetInt32("UserId");
                    m_UserSites.SiteId = smartReader.GetInt16("SiteId");
                    m_UserSites.CrUserId = smartReader.GetInt32("CrUserId");
                    m_UserSites.CrDateTime = smartReader.GetDateTime("CrDateTime");

                    l_UserSites.Add(m_UserSites);
                }
                reader.Close();
                return l_UserSites;
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
                SqlCommand cmd = new SqlCommand("UserSites_Insert");
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@UserId", this.UserId));
                cmd.Parameters.Add(new SqlParameter("@SiteId", this.SiteId));
                cmd.Parameters.Add("@UserSiteId", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                db.ExecuteSQL(cmd);
                this.UserSiteId = (cmd.Parameters["@UserSiteId"].Value == null) ? 0 : Convert.ToInt32(cmd.Parameters["@UserSiteId"].Value);
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
        public byte DeleteByUserId(byte Replicated, int ActUserId, ref short SysMessageId)
        {
            byte RetVal = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("UserSites_DeleteByUserId");
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                //cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@UserId", this.UserId));
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
        public List<UserSites> GetList(int UserId)
        {
            List<UserSites> RetVal = new List<UserSites>();
            try
            {
                string sql = "SELECT * FROM UserSites WHERE UserId=" + UserId.ToString();
                SqlCommand cmd = new SqlCommand(sql);
                RetVal = Init(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }

        public List<UserSites> GetByUser(int userId)
        {
            string cmdText = "SELECT * FROM UserSites WHERE (UserId = " + userId + ")";
            return this.Init(new SqlCommand(cmdText)
            {
                CommandType = CommandType.Text
            });
        }

    }
}
