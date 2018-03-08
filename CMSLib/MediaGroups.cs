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
    public class MediaGroups
    {
        private short _MediaGroupId;
        private string _MediaGroupName;
        private string _MediaGroupDesc;
        private int _CrUserId;
        private DateTime _CrDateTime;
        private DBAccess db;
        public short SiteId { get; set; }
        public short ParentGroupId { get; set; }
        public byte LevelId { get; set; }
        //-----------------------------------------------------------------
        public MediaGroups()
        {
            db = new DBAccess(DbConstants.CONNECTION_STRING);
        }
        //-----------------------------------------------------------------        
        public MediaGroups(string constr)
        {
            db = new DBAccess((constr == "") ? DbConstants.CONNECTION_STRING : constr);
        }
        //-----------------------------------------------------------------
        ~MediaGroups()
        {

        }
        //-----------------------------------------------------------------
        public virtual void Dispose()
        {

        }
        //-----------------------------------------------------------------    
        public short MediaGroupId
        {
            get { return _MediaGroupId; }
            set { _MediaGroupId = value; }
        }
        //-----------------------------------------------------------------
        public string MediaGroupName
        {
            get { return _MediaGroupName; }
            set { _MediaGroupName = value; }
        }
        //-----------------------------------------------------------------
        public string MediaGroupDesc
        {
            get { return _MediaGroupDesc; }
            set { _MediaGroupDesc = value; }
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

        private List<MediaGroups> Init(SqlCommand cmd)
        {
            SqlConnection con = db.GetConnection();
            cmd.Connection = con;
            List<MediaGroups> l_MediaGroups = new List<MediaGroups>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    MediaGroups m_MediaGroups = new MediaGroups(db.ConnectionString);
                    m_MediaGroups.MediaGroupId = smartReader.GetInt16("MediaGroupId");
                    m_MediaGroups.MediaGroupName = smartReader.GetString("MediaGroupName");
                    m_MediaGroups.MediaGroupDesc = smartReader.GetString("MediaGroupDesc");
                    m_MediaGroups.CrUserId = smartReader.GetInt32("CrUserId");
                    m_MediaGroups.CrDateTime = smartReader.GetDateTime("CrDateTime");
                    m_MediaGroups.SiteId = smartReader.GetInt16("SiteId");
                    m_MediaGroups.ParentGroupId = smartReader.GetInt16("ParentGroupId");

                    l_MediaGroups.Add(m_MediaGroups);
                }
                reader.Close();
                return l_MediaGroups;
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
                SqlCommand cmd = new SqlCommand("MediaGroups_Insert");
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@MediaGroupName", this.MediaGroupName));
                cmd.Parameters.Add(new SqlParameter("@MediaGroupDesc", this.MediaGroupDesc));
                cmd.Parameters.Add(new SqlParameter("@SiteId", this.SiteId));
                cmd.Parameters.Add(new SqlParameter("@ParentGroupId", this.ParentGroupId));
                cmd.Parameters.Add("@MediaGroupId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                db.ExecuteSQL(cmd);
                this.MediaGroupId = Convert.ToInt16((cmd.Parameters["@MediaGroupId"].Value == null) ? 0 : (cmd.Parameters["@MediaGroupId"].Value));
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
                SqlCommand cmd = new SqlCommand("MediaGroups_Update");
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@MediaGroupName", this.MediaGroupName));
                cmd.Parameters.Add(new SqlParameter("@MediaGroupDesc", this.MediaGroupDesc));
                cmd.Parameters.Add(new SqlParameter("@SiteId", this.SiteId));
                cmd.Parameters.Add(new SqlParameter("@ParentGroupId", this.ParentGroupId));
                cmd.Parameters.Add(new SqlParameter("@MediaGroupId", this.MediaGroupId));
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
                SqlCommand cmd = new SqlCommand("MediaGroups_Delete");
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@MediaGroupId", this.MediaGroupId));
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
        public List<MediaGroups> GetList()
        {
            List<MediaGroups> RetVal = new List<MediaGroups>();
            try
            {
                string sql = "SELECT * FROM MediaGroups";
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
        public List<MediaGroups> GetAllHierachy(int ActUserId, short SiteId, short MediaGroupId, string PaddingChar)
        {
            List<MediaGroups> RetVal = new List<MediaGroups>();
            try
            {
                SqlCommand cmd = new SqlCommand("MediaGroups_GetAllHierachy");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@SiteId", SiteId));
                cmd.Parameters.Add(new SqlParameter("@MediaGroupId", MediaGroupId));
                cmd.Parameters.Add(new SqlParameter("@PaddingChar", PaddingChar));
                RetVal = Init(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public static List<MediaGroups> Static_GetAllHierachy(int ActUserId, short SiteId, short MediaGroupId, string PaddingChar)
        {
            return new MediaGroups().GetAllHierachy(ActUserId, SiteId, MediaGroupId, PaddingChar);
        }
        //--------------------------------------------------------------
        public static List<MediaGroups> Static_GetList(string ConStr)
        {
            List<MediaGroups> RetVal = new List<MediaGroups>();
            try
            {
                MediaGroups m_MediaGroups = new MediaGroups(ConStr);
                RetVal = m_MediaGroups.GetList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //--------------------------------------------------------------     
        public static List<MediaGroups> Static_GetList()
        {
            return Static_GetList("");
        }
        //--------------------------------------------------------------   
        public static List<MediaGroups> Static_GetListAll(string ConStr, string TextOptionAll = "...")
        {
            MediaGroups m_MediaGroups = new MediaGroups(ConStr);
            List<MediaGroups> RetVal = m_MediaGroups.GetList();
            TextOptionAll = (TextOptionAll == null) ? "" : TextOptionAll.Trim();
            if (TextOptionAll.Length > 0)
            {
                m_MediaGroups.MediaGroupName = TextOptionAll;
                m_MediaGroups.MediaGroupDesc = TextOptionAll;
                RetVal.Insert(0, m_MediaGroups);
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public static List<MediaGroups> Static_GetListAll(string TextOptionAll = "...")
        {
            return Static_GetListAll("", TextOptionAll);
        }
        //--------------------------------------------------------------    
        public List<MediaGroups> GetListOrderBy(string OrderBy)
        {
            List<MediaGroups> RetVal = new List<MediaGroups>();
            try
            {
                OrderBy = StringUtil.InjectionString(OrderBy).Trim();
                string sql = "SELECT * FROM MediaGroups ";
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
        public static List<MediaGroups> Static_GetListOrderBy(string ConStr, string OrderBy)
        {
            MediaGroups m_MediaGroups = new MediaGroups(ConStr);
            return m_MediaGroups.GetListOrderBy(OrderBy);
        }
        //--------------------------------------------------------------    
        public static List<MediaGroups> Static_GetListOrderBy(string OrderBy)
        {
            return Static_GetListOrderBy("", OrderBy);
        }
        //--------------------------------------------------------------    
        public static List<MediaGroups> Static_GetListAllOrderBy(string ConStr, string OrderBy, string TextOptionAll = "...")
        {
            List<MediaGroups> RetVal = new List<MediaGroups>();
            MediaGroups m_MediaGroups = new MediaGroups(ConStr);
            try
            {
                OrderBy = StringUtil.InjectionString(OrderBy).Trim();
                if (OrderBy.Length > 0)
                {
                    RetVal = m_MediaGroups.GetListOrderBy(OrderBy);
                }
                TextOptionAll = (TextOptionAll == null) ? "" : TextOptionAll.Trim();
                if (TextOptionAll.Length > 0)
                {
                    m_MediaGroups.MediaGroupName = TextOptionAll;
                    m_MediaGroups.MediaGroupDesc = TextOptionAll;
                    RetVal.Insert(0, m_MediaGroups);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public static List<MediaGroups> Static_GetListAllOrderBy(string OrderBy, string TextOptionAll = "...")
        {
            return Static_GetListAllOrderBy("", OrderBy, TextOptionAll);
        }
        //--------------------------------------------------------------  
        public List<MediaGroups> GetListByMediaGroupId(short MediaGroupId)
        {
            List<MediaGroups> RetVal = new List<MediaGroups>();
            try
            {
                if (MediaGroupId > 0)
                {
                    string sql = "SELECT * FROM MediaGroups WHERE (MediaGroupId=" + MediaGroupId.ToString() + ")";
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
        public MediaGroups Get(short MediaGroupId)
        {
            MediaGroups RetVal = new MediaGroups(db.ConnectionString);
            try
            {
                List<MediaGroups> list = GetListByMediaGroupId(MediaGroupId);
                if (list.Count > 0)
                {
                    RetVal = (MediaGroups)list[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public MediaGroups Get()
        {
            return Get(this.MediaGroupId);
        }
        //-------------------------------------------------------------- 
        public static MediaGroups Static_Get(short MediaGroupId)
        {
            return Static_Get(MediaGroupId);
        }
        //--------------------------------------------------------------
    }
}
