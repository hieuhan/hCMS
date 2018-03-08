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
    public class MediaTypes
    {
        private byte _MediaTypeId;
        private string _MediaTypeName;
        private string _MediaTypeDesc;
        private byte _DisplayOrder;
        private DBAccess db;
        //-----------------------------------------------------------------
        public MediaTypes()
        {
            db = new DBAccess(DbConstants.CONNECTION_STRING);
        }
        //-----------------------------------------------------------------        
        public MediaTypes(string constr)
        {
            db = new DBAccess((constr == "") ? DbConstants.CONNECTION_STRING : constr);
        }
        //-----------------------------------------------------------------
        ~MediaTypes()
        {

        }
        //-----------------------------------------------------------------
        public virtual void Dispose()
        {

        }
        //-----------------------------------------------------------------    
        public byte MediaTypeId
        {
            get { return _MediaTypeId; }
            set { _MediaTypeId = value; }
        }
        //-----------------------------------------------------------------
        public string MediaTypeName
        {
            get { return _MediaTypeName; }
            set { _MediaTypeName = value; }
        }
        //-----------------------------------------------------------------
        public string MediaTypeDesc
        {
            get { return _MediaTypeDesc; }
            set { _MediaTypeDesc = value; }
        }
        //-----------------------------------------------------------------
        public byte DisplayOrder
        {
            get { return _DisplayOrder; }
            set { _DisplayOrder = value; }
        }
        //-----------------------------------------------------------------

        private List<MediaTypes> Init(SqlCommand cmd)
        {
            SqlConnection con = db.GetConnection();
            cmd.Connection = con;
            List<MediaTypes> l_MediaTypes = new List<MediaTypes>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    MediaTypes m_MediaTypes = new MediaTypes(db.ConnectionString);
                    m_MediaTypes.MediaTypeId = smartReader.GetByte("MediaTypeId");
                    m_MediaTypes.MediaTypeName = smartReader.GetString("MediaTypeName");
                    m_MediaTypes.MediaTypeDesc = smartReader.GetString("MediaTypeDesc");
                    m_MediaTypes.DisplayOrder = smartReader.GetByte("DisplayOrder");

                    l_MediaTypes.Add(m_MediaTypes);
                }
                reader.Close();
                return l_MediaTypes;
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
                SqlCommand cmd = new SqlCommand("MediaTypes_Insert");
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@MediaTypeName", this.MediaTypeName));
                cmd.Parameters.Add(new SqlParameter("@MediaTypeDesc", this.MediaTypeDesc));
                cmd.Parameters.Add(new SqlParameter("@DisplayOrder", this.DisplayOrder));
                cmd.Parameters.Add("@MediaTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                db.ExecuteSQL(cmd);
                this.MediaTypeId = Convert.ToByte((cmd.Parameters["@MediaTypeId"].Value == null) ? 0 : (cmd.Parameters["@MediaTypeId"].Value));
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
                SqlCommand cmd = new SqlCommand("MediaTypes_Update");
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@MediaTypeName", this.MediaTypeName));
                cmd.Parameters.Add(new SqlParameter("@MediaTypeDesc", this.MediaTypeDesc));
                cmd.Parameters.Add(new SqlParameter("@DisplayOrder", this.DisplayOrder));
                cmd.Parameters.Add(new SqlParameter("@MediaTypeId", this.MediaTypeId));
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
                SqlCommand cmd = new SqlCommand("MediaTypes_Delete");
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@MediaTypeId", this.MediaTypeId));
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
        public List<MediaTypes> GetList()
        {
            List<MediaTypes> RetVal = new List<MediaTypes>();
            try
            {
                string sql = "SELECT * FROM MediaTypes";
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
        public static List<MediaTypes> Static_GetList(string ConStr)
        {
            List<MediaTypes> RetVal = new List<MediaTypes>();
            try
            {
                MediaTypes m_MediaTypes = new MediaTypes(ConStr);
                RetVal = m_MediaTypes.GetList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //--------------------------------------------------------------     
        public static List<MediaTypes> Static_GetList()
        {
            return Static_GetList("");
        }
        //--------------------------------------------------------------   
        public static List<MediaTypes> Static_GetListAll(string ConStr, string TextOptionAll = "...")
        {
            MediaTypes m_MediaTypes = new MediaTypes(ConStr);
            List<MediaTypes> RetVal = m_MediaTypes.GetList();
            TextOptionAll = (TextOptionAll == null) ? "" : TextOptionAll.Trim();
            if (TextOptionAll.Length > 0)
            {
                m_MediaTypes.MediaTypeName = TextOptionAll;
                m_MediaTypes.MediaTypeDesc = TextOptionAll;
                RetVal.Insert(0, m_MediaTypes);
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public static List<MediaTypes> Static_GetListAll(string TextOptionAll = "...")
        {
            return Static_GetListAll("", TextOptionAll);
        }
        //--------------------------------------------------------------    
        public List<MediaTypes> GetListOrderBy(string OrderBy)
        {
            List<MediaTypes> RetVal = new List<MediaTypes>();
            try
            {
                OrderBy = StringUtil.InjectionString(OrderBy).Trim();
                string sql = "SELECT * FROM MediaTypes ";
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
        public static List<MediaTypes> Static_GetListOrderBy(string ConStr, string OrderBy)
        {
            MediaTypes m_MediaTypes = new MediaTypes(ConStr);
            return m_MediaTypes.GetListOrderBy(OrderBy);
        }
        //--------------------------------------------------------------    
        public static List<MediaTypes> Static_GetListOrderBy(string OrderBy)
        {
            return Static_GetListOrderBy("", OrderBy);
        }
        //--------------------------------------------------------------    
        public static List<MediaTypes> Static_GetListAllOrderBy(string ConStr, string OrderBy, string TextOptionAll = "...")
        {
            List<MediaTypes> RetVal = new List<MediaTypes>();
            MediaTypes m_MediaTypes = new MediaTypes(ConStr);
            try
            {
                OrderBy = StringUtil.InjectionString(OrderBy).Trim();
                if (OrderBy.Length > 0)
                {
                    RetVal = m_MediaTypes.GetListOrderBy(OrderBy);
                }
                TextOptionAll = (TextOptionAll == null) ? "" : TextOptionAll.Trim();
                if (TextOptionAll.Length > 0)
                {
                    m_MediaTypes.MediaTypeName = TextOptionAll;
                    m_MediaTypes.MediaTypeDesc = TextOptionAll;
                    RetVal.Insert(0, m_MediaTypes);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public static List<MediaTypes> Static_GetListAllOrderBy(string OrderBy, string TextOptionAll = "...")
        {
            return Static_GetListAllOrderBy("", OrderBy, TextOptionAll);
        }
        //--------------------------------------------------------------  
        public List<MediaTypes> GetListByMediaTypeId(byte MediaTypeId)
        {
            List<MediaTypes> RetVal = new List<MediaTypes>();
            try
            {
                if (MediaTypeId > 0)
                {
                    string sql = "SELECT * FROM MediaTypes WHERE (MediaTypeId=" + MediaTypeId.ToString() + ")";
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
        public MediaTypes Get(byte MediaTypeId)
        {
            MediaTypes RetVal = new MediaTypes(db.ConnectionString);
            try
            {
                List<MediaTypes> list = GetListByMediaTypeId(MediaTypeId);
                if (list.Count > 0)
                {
                    RetVal = (MediaTypes)list[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public MediaTypes Get()
        {
            return Get(this.MediaTypeId);
        }
        //-------------------------------------------------------------- 
        public static MediaTypes Static_Get(byte MediaTypeId)
        {
            return Static_Get(MediaTypeId);
        }
        //--------------------------------------------------------------
    }
}
