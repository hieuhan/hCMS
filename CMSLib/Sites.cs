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
    public class Sites
    {
        private short _SiteId;
        private string _SiteName = "";
        private string _SiteDesc = "";
        private string _MetaTitle;
        private string _MetaDesc;
        private string _MetaKeyword;
        private string _MetaTagAll;
        private string _CanonicalTag;
        private string _H1Tag;
        private string _SeoFooter;
        private int _CrUserId;
        private DateTime _CrDateTime;
        private DBAccess db;
        //-----------------------------------------------------------------
        public Sites()
        {
            db = new DBAccess(DbConstants.CONNECTION_STRING);
        }
        //-----------------------------------------------------------------        
        public Sites(string constr)
        {
            db = new DBAccess((constr == "") ? DbConstants.CONNECTION_STRING : constr);
        }
        //-----------------------------------------------------------------
        ~Sites()
        {

        }
        //-----------------------------------------------------------------
        public virtual void Dispose()
        {

        }
        //-----------------------------------------------------------------    
        public short SiteId
        {
            get { return _SiteId; }
            set { _SiteId = value; }
        }
        //-----------------------------------------------------------------
        public string SiteName
        {
            get { return _SiteName; }
            set { _SiteName = value; }
        }
        //-----------------------------------------------------------------
        public string SiteDesc
        {
            get { return _SiteDesc; }
            set { _SiteDesc = value; }
        }
        //-----------------------------------------------------------------
        public string MetaTitle
        {
            get { return _MetaTitle; }
            set { _MetaTitle = value; }
        }
        //-----------------------------------------------------------------
        public string MetaDesc
        {
            get { return _MetaDesc; }
            set { _MetaDesc = value; }
        }
        //-----------------------------------------------------------------
        public string MetaKeyword
        {
            get { return _MetaKeyword; }
            set { _MetaKeyword = value; }
        }
        //-----------------------------------------------------------------
        public string MetaTagAll
        {
            get { return _MetaTagAll; }
            set { _MetaTagAll = value; }
        }
        //-----------------------------------------------------------------
        public string CanonicalTag
        {
            get { return _CanonicalTag; }
            set { _CanonicalTag = value; }
        }
        //-----------------------------------------------------------------
        public string H1Tag
        {
            get { return _H1Tag; }
            set { _H1Tag = value; }
        }
        //-----------------------------------------------------------------
        public string SeoFooter
        {
            get { return _SeoFooter; }
            set { _SeoFooter = value; }
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

        private List<Sites> Init(SqlCommand cmd)
        {
            SqlConnection con = db.GetConnection();
            cmd.Connection = con;
            List<Sites> l_Sites = new List<Sites>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    Sites m_Sites = new Sites(db.ConnectionString);
                    m_Sites.SiteId = smartReader.GetInt16("SiteId");
                    m_Sites.SiteName = smartReader.GetString("SiteName");
                    m_Sites.SiteDesc = smartReader.GetString("SiteDesc");
                    m_Sites.MetaTitle = smartReader.GetString("MetaTitle");
                    m_Sites.MetaDesc = smartReader.GetString("MetaDesc");
                    m_Sites.MetaKeyword = smartReader.GetString("MetaKeyword");
                    m_Sites.MetaTagAll = smartReader.GetString("MetaTagAll");
                    m_Sites.CanonicalTag = smartReader.GetString("CanonicalTag");
                    m_Sites.H1Tag = smartReader.GetString("H1Tag");
                    m_Sites.SeoFooter = smartReader.GetString("SeoFooter");
                    m_Sites.CrUserId = smartReader.GetInt32("CrUserId");
                    m_Sites.CrDateTime = smartReader.GetDateTime("CrDateTime");

                    l_Sites.Add(m_Sites);
                }
                reader.Close();
                return l_Sites;
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
                SqlCommand cmd = new SqlCommand("Sites_Insert");
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@SiteName", this.SiteName));
                cmd.Parameters.Add(new SqlParameter("@SiteDesc", this.SiteDesc));
                cmd.Parameters.Add(new SqlParameter("@MetaTitle", this.MetaTitle));
                cmd.Parameters.Add(new SqlParameter("@MetaDesc", this.MetaDesc));
                cmd.Parameters.Add(new SqlParameter("@MetaKeyword", this.MetaKeyword));
                cmd.Parameters.Add(new SqlParameter("@MetaTagAll", this.MetaTagAll));
                cmd.Parameters.Add(new SqlParameter("@CanonicalTag", this.CanonicalTag));
                cmd.Parameters.Add(new SqlParameter("@H1Tag", this.H1Tag));
                cmd.Parameters.Add(new SqlParameter("@SeoFooter", this.SeoFooter));
                cmd.Parameters.Add("@SiteId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                db.ExecuteSQL(cmd);
                this.SiteId = (cmd.Parameters["@SiteId"].Value == null) ? (short)0 : Convert.ToInt16(cmd.Parameters["@SiteId"].Value);
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
                SqlCommand cmd = new SqlCommand("Sites_Update");
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@SiteName", this.SiteName));
                cmd.Parameters.Add(new SqlParameter("@SiteDesc", this.SiteDesc));
                cmd.Parameters.Add(new SqlParameter("@MetaTitle", this.MetaTitle));
                cmd.Parameters.Add(new SqlParameter("@MetaDesc", this.MetaDesc));
                cmd.Parameters.Add(new SqlParameter("@MetaKeyword", this.MetaKeyword));
                cmd.Parameters.Add(new SqlParameter("@MetaTagAll", this.MetaTagAll));
                cmd.Parameters.Add(new SqlParameter("@CanonicalTag", this.CanonicalTag));
                cmd.Parameters.Add(new SqlParameter("@H1Tag", this.H1Tag));
                cmd.Parameters.Add(new SqlParameter("@SeoFooter", this.SeoFooter));
                cmd.Parameters.Add(new SqlParameter("@SiteId", this.SiteId));
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
        //-----------------------------------------------------------
        public byte InsertOrUpdate(byte Replicated, int ActUserId, ref short SysMessageId)
        {
            byte RetVal = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("Sites_InsertOrUpdate");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@SiteName", this.SiteName));
                cmd.Parameters.Add(new SqlParameter("@SiteDesc", this.SiteDesc));
                cmd.Parameters.Add(new SqlParameter("@MetaTitle", this.MetaTitle));
                cmd.Parameters.Add(new SqlParameter("@MetaDesc", this.MetaDesc));
                cmd.Parameters.Add(new SqlParameter("@MetaKeyword", this.MetaKeyword));
                cmd.Parameters.Add(new SqlParameter("@MetaTagAll", this.MetaTagAll));
                cmd.Parameters.Add(new SqlParameter("@CanonicalTag", this.CanonicalTag));
                cmd.Parameters.Add(new SqlParameter("@H1Tag", this.H1Tag));
                cmd.Parameters.Add(new SqlParameter("@SeoFooter", this.SeoFooter));
                cmd.Parameters.Add(new SqlParameter("@SiteId", this.SiteId).Direction = ParameterDirection.InputOutput);
                cmd.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                db.ExecuteSQL(cmd);
                this.SiteId = (cmd.Parameters["@SiteId"].Value == null) ? (short)0 : Convert.ToInt16(cmd.Parameters["@SiteId"].Value);
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
                SqlCommand cmd = new SqlCommand("Sites_Delete");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@SiteId", this.SiteId));
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
        public void GenUrl(short SiteId, byte ResetAll)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sites_GenUrl");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@SiteId", SiteId));
                cmd.Parameters.Add(new SqlParameter("@ResetAll", ResetAll));
                db.ExecuteSQL(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //-------------------------------------------------------------- 
        public List<Sites> GetList(int ActUserId)
        {
            List<Sites> RetVal = new List<Sites>();
            try
            {
                int RowAmount = 100;
                int PageIndex = 0;
                string OrderBy = "DisplayOrder, SiteName";
                string DateFrom = "";
                string DateTo = "";
                int RowCount = 0;
                RetVal = GetPage(ActUserId, RowAmount, PageIndex, OrderBy, DateFrom, DateTo, ref RowCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }

        //--------------------------------------------------------------  
        public List<Sites> GetListBySiteId(int ActUserId, short SiteId)
        {
            List<Sites> RetVal = new List<Sites>();
            try
            {
                if (SiteId > 0)
                {
                    int RowAmount = 1;
                    int PageIndex = 0;
                    string OrderBy = "";
                    string DateFrom = "";
                    string DateTo = "";
                    int RowCount = 0;
                    this.SiteId = SiteId;
                    RetVal = GetPage(ActUserId, RowAmount, PageIndex, OrderBy, DateFrom, DateTo, ref RowCount);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }

        //-------------------------------------------------------------- 
        public Sites Get(int ActUserId, short SiteId)
        {
            Sites RetVal = new Sites();
            try
            {
                List<Sites> list = GetListBySiteId(ActUserId, SiteId);
                if (list.Count > 0)
                {
                    RetVal = (Sites)list[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public Sites Get(short SiteId, List<Sites> list)
        {
            Sites RetVal = new Sites();
            try
            {
                RetVal = list.Find(i => i.SiteId == SiteId);
                if (RetVal == null) RetVal = new Sites();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public Sites Get(int ActUserId)
        {
            return Get(ActUserId, this.SiteId);
        }
        //-------------------------------------------------------------- 
        public static Sites Static_Get(int ActUserId, short SiteId)
        {
            return new Sites().Get(ActUserId, SiteId);
        }
        //--------------------------------------------------------------     
        public static List<Sites> Static_GetList(int ActUserId)
        {
            return new Sites().GetList(ActUserId);
        }
        //-----------------------------------------------------------------------------
        public static Sites Static_Get(short SiteId, List<Sites> lList)
        {
            Sites RetVal = new Sites();
            if (SiteId > 0 && lList.Count > 0)
            {
                RetVal = lList.Find(i => i.SiteId == SiteId);
                if (RetVal == null) RetVal = new Sites();
            }
            return RetVal;
        }
        //--------------------------------------------------------------     
        public List<Sites> GetPage(int ActUserId, int RowAmount, int PageIndex, string OrderBy, string DateFrom, string DateTo, ref int RowCount)
        {
            List<Sites> RetVal = new List<Sites>();
            try
            {
                SqlCommand cmd = new SqlCommand("Sites_GetPage");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@RowAmount", RowAmount));
                cmd.Parameters.Add(new SqlParameter("@PageIndex", PageIndex));
                cmd.Parameters.Add(new SqlParameter("@OrderBy", OrderBy));
                cmd.Parameters.Add(new SqlParameter("@SiteId", this.SiteId));
                cmd.Parameters.Add(new SqlParameter("@SiteName", this.SiteName));
                cmd.Parameters.Add(new SqlParameter("@SiteDesc", this.SiteDesc));
                cmd.Parameters.Add(new SqlParameter("@CrUserId", this.CrUserId));
                if (DateFrom != "") cmd.Parameters.Add(new SqlParameter("@DateFrom", DateTime.Parse(DateFrom, System.Globalization.CultureInfo.CreateSpecificCulture("en-CA"))));
                if (DateTo != "") cmd.Parameters.Add(new SqlParameter("@DateTo", DateTime.Parse(DateTo, System.Globalization.CultureInfo.CreateSpecificCulture("en-CA"))));
                cmd.Parameters.Add("@RowCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                RetVal = Init(cmd);
                RowCount = Convert.ToInt32(cmd.Parameters["@RowCount"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
    }
}
