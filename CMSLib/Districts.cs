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
    public class Districts
    {
        private short _DistrictId;
        private string _DistrictName;
        private string _DistrictDesc;
        private short _CountryId;
        private short _ProvinceId;
        private short _DisplayOrder;
        private int _CrUserId;
        private DateTime _CrDateTime;
        private DBAccess db;
        //-----------------------------------------------------------------
        public Districts()
        {
            db = new DBAccess(DbConstants.CONNECTION_STRING);
        }
        //-----------------------------------------------------------------        
        public Districts(string constr)
        {
            db = new DBAccess((constr == "") ? DbConstants.CONNECTION_STRING : constr);
        }
        //-----------------------------------------------------------------
        ~Districts()
        {

        }
        //-----------------------------------------------------------------
        public virtual void Dispose()
        {

        }
        //-----------------------------------------------------------------    
        public short DistrictId
        {
            get { return _DistrictId; }
            set { _DistrictId = value; }
        }
        //-----------------------------------------------------------------
        public string DistrictName
        {
            get { return _DistrictName; }
            set { _DistrictName = value; }
        }
        //-----------------------------------------------------------------
        public string DistrictDesc
        {
            get { return _DistrictDesc; }
            set { _DistrictDesc = value; }
        }
        //-----------------------------------------------------------------
        public short CountryId
        {
            get { return _CountryId; }
            set { _CountryId = value; }
        }
        //-----------------------------------------------------------------
        public short ProvinceId
        {
            get { return _ProvinceId; }
            set { _ProvinceId = value; }
        }
        //-----------------------------------------------------------------
        public short DisplayOrder
        {
            get { return _DisplayOrder; }
            set { _DisplayOrder = value; }
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

        private List<Districts> Init(SqlCommand cmd)
        {
            SqlConnection con = db.GetConnection();
            cmd.Connection = con;
            List<Districts> l_Districts = new List<Districts>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    Districts m_Districts = new Districts(db.ConnectionString);
                    m_Districts.DistrictId = smartReader.GetInt16("DistrictId");
                    m_Districts.DistrictName = smartReader.GetString("DistrictName");
                    m_Districts.DistrictDesc = smartReader.GetString("DistrictDesc");
                    m_Districts.CountryId = smartReader.GetInt16("CountryId");
                    m_Districts.ProvinceId = smartReader.GetInt16("ProvinceId");
                    m_Districts.DisplayOrder = smartReader.GetInt16("DisplayOrder");
                    m_Districts.CrUserId = smartReader.GetInt32("CrUserId");
                    m_Districts.CrDateTime = smartReader.GetDateTime("CrDateTime");

                    l_Districts.Add(m_Districts);
                }
                reader.Close();
                return l_Districts;
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
                SqlCommand cmd = new SqlCommand("Districts_Insert");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@DistrictName", this.DistrictName));
                cmd.Parameters.Add(new SqlParameter("@DistrictDesc", this.DistrictDesc));
                cmd.Parameters.Add(new SqlParameter("@CountryId", this.CountryId));
                cmd.Parameters.Add(new SqlParameter("@ProvinceId", this.ProvinceId));
                cmd.Parameters.Add(new SqlParameter("@DisplayOrder", this.DisplayOrder));
                cmd.Parameters.Add("@DistrictId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                db.ExecuteSQL(cmd);
                this.DistrictId = (cmd.Parameters["@DistrictId"].Value == null) ? (short)0 : Convert.ToInt16(cmd.Parameters["@DistrictId"].Value);
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
                SqlCommand cmd = new SqlCommand("Districts_Update");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@DistrictName", this.DistrictName));
                cmd.Parameters.Add(new SqlParameter("@DistrictDesc", this.DistrictDesc));
                cmd.Parameters.Add(new SqlParameter("@CountryId", this.CountryId));
                cmd.Parameters.Add(new SqlParameter("@ProvinceId", this.ProvinceId));
                cmd.Parameters.Add(new SqlParameter("@DisplayOrder", this.DisplayOrder));
                cmd.Parameters.Add(new SqlParameter("@DistrictId", this.DistrictId));
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
                SqlCommand cmd = new SqlCommand("Districts_InsertOrUpdate");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@DistrictName", this.DistrictName));
                cmd.Parameters.Add(new SqlParameter("@DistrictDesc", this.DistrictDesc));
                cmd.Parameters.Add(new SqlParameter("@CountryId", this.CountryId));
                cmd.Parameters.Add(new SqlParameter("@ProvinceId", this.ProvinceId));
                cmd.Parameters.Add(new SqlParameter("@DisplayOrder", this.DisplayOrder));
                cmd.Parameters.Add(new SqlParameter("@DistrictId", this.DistrictId)).Direction = ParameterDirection.InputOutput;
                cmd.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                db.ExecuteSQL(cmd);
                this.DistrictId = (cmd.Parameters["@DistrictId"].Value == null) ? (short)0 : Convert.ToInt16(cmd.Parameters["@DistrictId"].Value);
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
                SqlCommand cmd = new SqlCommand("Districts_Delete");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@DistrictId", this.DistrictId));
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
        public List<Districts> GetList()
        {
            List<Districts> RetVal = new List<Districts>();
            try
            {
                string sql = "SELECT * FROM Districts ORDER BY DisplayOrder,DistrictName";
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
        public List<Districts> GetList(short ProvinceId)
        {
            List<Districts> RetVal = new List<Districts>();
            try
            {
                string sql = "SELECT * FROM Districts WHERE ProvinceId=" + ProvinceId.ToString() + " ORDER BY DistrictName";
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
        public List<Districts> GetListByDistrictId(short DistrictId)
        {
            List<Districts> RetVal = new List<Districts>();
            try
            {
                if (DistrictId > 0)
                {
                    string sql = "SELECT * FROM Districts WHERE (DistrictId=" + DistrictId.ToString() + ")";
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
        public Districts Get(short DistrictId)
        {
            Districts RetVal = new Districts();
            try
            {
                List<Districts> list = GetListByDistrictId(DistrictId);
                if (list.Count > 0)
                {
                    RetVal = (Districts)list[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public static Districts Static_Get(short DistrictId, List<Districts> list)
        {
            Districts RetVal = new Districts();
            try
            {
                RetVal = list.Find(i => i.DistrictId == DistrictId);
                if (RetVal == null) RetVal = new Districts();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public Districts Get()
        {
            return Get(this.DistrictId);
        }
        //-------------------------------------------------------------- 
        public static Districts Static_Get(short DistrictId)
        {
            return new Districts().Get(DistrictId);
        }
        //-------------------------------------------------------------- 
        public static List<Districts> Static_GetList()
        {
            return new Districts().GetList();
        }
        //-------------------------------------------------------------- 
        public static List<Districts> Static_GetList(short ProvinceId)
        {
            return new Districts().GetList(ProvinceId);
        }
        //--------------------------------------------------------------     
        public List<Districts> GetPage(int ActUserId, int RowAmount, int PageIndex, string OrderBy, ref int RowCount)
        {
            List<Districts> RetVal = new List<Districts>();
            try
            {
                SqlCommand cmd = new SqlCommand("Districts_GetPage");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@RowAmount", RowAmount));
                cmd.Parameters.Add(new SqlParameter("@PageIndex", PageIndex));
                cmd.Parameters.Add(new SqlParameter("@OrderBy", OrderBy));
                cmd.Parameters.Add(new SqlParameter("@DistrictName", this.DistrictName));
                cmd.Parameters.Add(new SqlParameter("@DistrictDesc", this.DistrictDesc));
                cmd.Parameters.Add(new SqlParameter("@CountryId", this.CountryId));
                cmd.Parameters.Add(new SqlParameter("@ProvinceId", this.ProvinceId));
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
