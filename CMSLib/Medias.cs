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
    public class Medias
    {
        private int _MediaId;
        private byte _MediaTypeId;
        private short _MediaGroupId;
        private string _MediaName;
        private string _MediaDesc;
        private string _FilePath;
        private int _FileSize;
        private int _CrUserId;
        private DateTime _CrDateTime;
        private DBAccess db;
        //-----------------------------------------------------------------
        public Medias()
        {
            db = new DBAccess(DbConstants.CONNECTION_STRING);
        }
        //-----------------------------------------------------------------        
        public Medias(string constr)
        {
            db = new DBAccess((constr == "") ? DbConstants.CONNECTION_STRING : constr);
        }
        //-----------------------------------------------------------------
        ~Medias()
        {

        }
        //-----------------------------------------------------------------
        public virtual void Dispose()
        {

        }
        //-----------------------------------------------------------------    
        public int MediaId
        {
            get { return _MediaId; }
            set { _MediaId = value; }
        }
        //-----------------------------------------------------------------
        public byte MediaTypeId
        {
            get { return _MediaTypeId; }
            set { _MediaTypeId = value; }
        }
        //-----------------------------------------------------------------
        public short MediaGroupId
        {
            get { return _MediaGroupId; }
            set { _MediaGroupId = value; }
        }
        //-----------------------------------------------------------------
        public string MediaName
        {
            get { return _MediaName; }
            set { _MediaName = value; }
        }
        //-----------------------------------------------------------------
        public string MediaDesc
        {
            get { return _MediaDesc; }
            set { _MediaDesc = value; }
        }
        //-----------------------------------------------------------------
        public string FilePath
        {
            get { return _FilePath; }
            set { _FilePath = value; }
        }
        //-----------------------------------------------------------------
        public int FileSize
        {
            get { return _FileSize; }
            set { _FileSize = value; }
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

        private List<Medias> Init(SqlCommand cmd)
        {
            SqlConnection con = db.GetConnection();
            cmd.Connection = con;
            List<Medias> l_Medias = new List<Medias>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    Medias m_Medias = new Medias(db.ConnectionString)
                    {
                        MediaId = smartReader.GetInt32("MediaId"),
                        MediaTypeId = smartReader.GetByte("MediaTypeId"),
                        MediaGroupId = smartReader.GetInt16("MediaGroupId"),
                        MediaName = smartReader.GetString("MediaName"),
                        MediaDesc = smartReader.GetString("MediaDesc"),
                        FilePath = smartReader.GetString("FilePath"),
                        FileSize = smartReader.GetInt32("FileSize"),
                        CrUserId = smartReader.GetInt32("CrUserId"),
                        CrDateTime = smartReader.GetDateTime("CrDateTime")
                    };

                    l_Medias.Add(m_Medias);
                }
                reader.Close();
                return l_Medias;
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
                RetVal = InsertOrUpdate(Replicated, ActUserId, ref SysMessageId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
            //byte RetVal = 0;
            //try
            //{
            //    SqlCommand cmd = new SqlCommand("Medias_InsertOrUpdate");
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
            //    cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
            //    cmd.Parameters.Add(new SqlParameter("@MediaTypeId", this.MediaTypeId));
            //    cmd.Parameters.Add(new SqlParameter("@MediaGroupId", this.MediaGroupId));
            //    cmd.Parameters.Add(new SqlParameter("@MediaName", this.MediaName));
            //    cmd.Parameters.Add(new SqlParameter("@MediaDesc", this.MediaDesc));
            //    cmd.Parameters.Add(new SqlParameter("@FilePath", this.FilePath));
            //    cmd.Parameters.Add(new SqlParameter("@FileSize", this.FileSize));
            //    cmd.Parameters.Add("@MediaId", SqlDbType.Int).Direction = ParameterDirection.Output;
            //    cmd.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
            //    cmd.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
            //    db.ExecuteSQL(cmd);
            //    this.MediaId = Convert.ToInt32((cmd.Parameters["@MediaId"].Value == null) ? 0 : (cmd.Parameters["@MediaId"].Value));
            //    SysMessageId = Convert.ToInt16((cmd.Parameters["@SysMessageId"].Value == null) ? "0" : cmd.Parameters["@SysMessageId"].Value);
            //    RetVal = Convert.ToByte((cmd.Parameters["@SysMessageTypeId"].Value == null) ? "0" : cmd.Parameters["@SysMessageTypeId"].Value);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //return RetVal;
        }
        //--------------------------------------------------------------
        public byte Update(byte Replicated, int ActUserId, ref short SysMessageId)
        {
            byte RetVal = 0;
            try
            {
                RetVal = InsertOrUpdate(Replicated, ActUserId, ref SysMessageId);
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
                SqlCommand cmd = new SqlCommand("Medias_InsertOrUpdate");
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@MediaTypeId", this.MediaTypeId));
                cmd.Parameters.Add(new SqlParameter("@MediaGroupId", this.MediaGroupId));
                cmd.Parameters.Add(new SqlParameter("@MediaName", this.MediaName));
                cmd.Parameters.Add(new SqlParameter("@MediaDesc", this.MediaDesc));
                cmd.Parameters.Add(new SqlParameter("@FilePath", this.FilePath));
                cmd.Parameters.Add(new SqlParameter("@FileSize", this.FileSize));
                cmd.Parameters.Add(new SqlParameter("@MediaId", this.MediaId)).Direction = ParameterDirection.InputOutput;
                cmd.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                db.ExecuteSQL(cmd);
                this.MediaId = Convert.ToInt32((cmd.Parameters["@MediaId"].Value == null) ? 0 : (cmd.Parameters["@MediaId"].Value));
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
                SqlCommand cmd = new SqlCommand("Medias_Delete");
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@MediaId", this.MediaId));
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
        public List<Medias> GetList()
        {
            List<Medias> RetVal = new List<Medias>();
            try
            {
                string sql = "SELECT * FROM Medias";
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
        public static List<Medias> Static_GetList(string ConStr)
        {
            List<Medias> RetVal = new List<Medias>();
            try
            {
                Medias m_Medias = new Medias(ConStr);
                RetVal = m_Medias.GetList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //--------------------------------------------------------------     
        public static List<Medias> Static_GetList()
        {
            return Static_GetList("");
        }
        //--------------------------------------------------------------   
        public static List<Medias> Static_GetListAll(string ConStr, string TextOptionAll = "...")
        {
            Medias m_Medias = new Medias(ConStr);
            List<Medias> RetVal = m_Medias.GetList();
            TextOptionAll = (TextOptionAll == null) ? "" : TextOptionAll.Trim();
            if (TextOptionAll.Length > 0)
            {
                m_Medias.MediaName = TextOptionAll;
                m_Medias.MediaDesc = TextOptionAll;
                RetVal.Insert(0, m_Medias);
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public static List<Medias> Static_GetListAll(string TextOptionAll = "...")
        {
            return Static_GetListAll("", TextOptionAll);
        }
        //--------------------------------------------------------------    
        public List<Medias> GetListOrderBy(string OrderBy)
        {
            List<Medias> RetVal = new List<Medias>();
            try
            {
                OrderBy = StringUtil.InjectionString(OrderBy).Trim();
                string sql = "SELECT * FROM Medias ";
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
        public static List<Medias> Static_GetListOrderBy(string ConStr, string OrderBy)
        {
            Medias m_Medias = new Medias(ConStr);
            return m_Medias.GetListOrderBy(OrderBy);
        }
        //--------------------------------------------------------------    
        public static List<Medias> Static_GetListOrderBy(string OrderBy)
        {
            return Static_GetListOrderBy("", OrderBy);
        }
        //--------------------------------------------------------------    
        public static List<Medias> Static_GetListAllOrderBy(string ConStr, string OrderBy, string TextOptionAll = "...")
        {
            List<Medias> RetVal = new List<Medias>();
            Medias m_Medias = new Medias(ConStr);
            try
            {
                OrderBy = StringUtil.InjectionString(OrderBy).Trim();
                if (OrderBy.Length > 0)
                {
                    RetVal = m_Medias.GetListOrderBy(OrderBy);
                }
                TextOptionAll = (TextOptionAll == null) ? "" : TextOptionAll.Trim();
                if (TextOptionAll.Length > 0)
                {
                    m_Medias.MediaName = TextOptionAll;
                    m_Medias.MediaDesc = TextOptionAll;
                    RetVal.Insert(0, m_Medias);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public static List<Medias> Static_GetListAllOrderBy(string OrderBy, string TextOptionAll = "...")
        {
            return Static_GetListAllOrderBy("", OrderBy, TextOptionAll);
        }
        //--------------------------------------------------------------  
        public List<Medias> GetListByMediaId(int MediaId)
        {
            List<Medias> RetVal = new List<Medias>();
            try
            {
                if (MediaId > 0)
                {
                    string sql = "SELECT * FROM Medias WHERE (MediaId=" + MediaId.ToString() + ")";
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
        public Medias Get(int MediaId)
        {
            Medias RetVal = new Medias(db.ConnectionString);
            try
            {
                List<Medias> list = GetListByMediaId(MediaId);
                if (list.Count > 0)
                {
                    RetVal = (Medias)list[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public Medias Get()
        {
            return Get(this.MediaId);
        }
        //-------------------------------------------------------------- 
        public static Medias Static_Get(int MediaId)
        {
            return Static_Get(MediaId);
        }
        //--------------------------------------------------------------     
        public List<Medias> GetPage(int ActUserId, int RowAmount, int PageIndex, string OrderBy, byte MediaTypeId, short MediaGroupId, string MediaName, string DateFrom, string DateTo, ref int RowCount)
        {
            List<Medias> RetVal = new List<Medias>();
            try
            {
                SqlCommand cmd = new SqlCommand("Medias_GetPage");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@RowAmount", RowAmount));
                cmd.Parameters.Add(new SqlParameter("@PageIndex", PageIndex));
                cmd.Parameters.Add(new SqlParameter("@OrderBy", StringUtil.InjectionString(OrderBy)));
                cmd.Parameters.Add(new SqlParameter("@MediaTypeId", MediaTypeId));
                cmd.Parameters.Add(new SqlParameter("@MediaGroupId", MediaGroupId));
                cmd.Parameters.Add(new SqlParameter("@MediaName", StringUtil.InjectionString(MediaName)));
                if (DateFrom != "") cmd.Parameters.Add(new SqlParameter("@DateFrom", StringUtil.ConvertToDateTime(DateFrom)));
                if (DateTo != "") cmd.Parameters.Add(new SqlParameter("@DateTo", StringUtil.ConvertToDateTime(DateTo)));
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
        //--------------------------------------------------------------   
        public List<Medias> GetListByArtice(int ActUserId, int ArticleId)
        {
            List<Medias> RetVal = new List<Medias>();
            try
            {
                SqlCommand cmd = new SqlCommand("Medias_GetListByArtice");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@ArticleId", ArticleId));
                RetVal = Init(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //--------------------------------------------------------------     
        public List<Medias> GetPage(int ActUserId, int RowAmount, int PageIndex, string OrderBy, string DateFrom, string DateTo, int ArticleId, ref int RowCount)
        {
            List<Medias> RetVal = new List<Medias>();
            try
            {
                SqlCommand cmd = new SqlCommand("Medias_GetPage");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@RowAmount", RowAmount));
                cmd.Parameters.Add(new SqlParameter("@PageIndex", PageIndex));
                cmd.Parameters.Add(new SqlParameter("@OrderBy", StringUtil.InjectionString(OrderBy)));
                cmd.Parameters.Add(new SqlParameter("@MediaTypeId", this.MediaTypeId));
                cmd.Parameters.Add(new SqlParameter("@MediaGroupId", this.MediaGroupId));
                cmd.Parameters.Add(new SqlParameter("@ArticleId", ArticleId));
                cmd.Parameters.Add(new SqlParameter("@MediaName", StringUtil.InjectionString(this.MediaName)));
                if (DateFrom != "") cmd.Parameters.Add(new SqlParameter("@DateFrom", StringUtil.ConvertToDateTime(DateFrom)));
                if (DateTo != "") cmd.Parameters.Add(new SqlParameter("@DateTo", StringUtil.ConvertToDateTime(DateTo)));
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
        //--------------------------------------------------------------     
        public List<Medias> Search(int ActUserId, string OrderBy, byte MediaTypeId, short MediaGroupId, string MediaName, string DateFrom, string DateTo, ref int RowCount)
        {
            int RowAmount = 0;
            int PageIndex = 0;
            return GetPage(ActUserId, RowAmount, PageIndex, OrderBy, MediaTypeId, MediaGroupId, MediaName, DateFrom, DateTo, ref RowCount);
        }
        //-------------------------------------------------------------- 
        public string GetImageUrl()
        {
            string RetVal = _FilePath;
            if (string.IsNullOrEmpty(_FilePath))
            {
                RetVal = Constants.NO_IMAGE_URL;
            }
            if (!string.IsNullOrEmpty(RetVal) && !RetVal.StartsWith("http://")) RetVal = Constants.WEBSITE_IMAGEDOMAIN + RetVal;
            return RetVal;
        }
        //--------------------------------------------------------------  
        public string GetImageUrl_Thumb()
        {
            return GetImageUrl().Replace("/Original/", "/Thumb/");
        }
    }
}
