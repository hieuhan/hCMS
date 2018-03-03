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
    public class DataSources
    {
        private short _DataSourceId;
        private byte _DataTypeId;
        private string _DataSourceName;
        private string _DataSourceDesc;
        private short _DisplayOrder;
        private int _CrUserId;
        private DateTime _CrDateTime;
        private DBAccess db;
        //-----------------------------------------------------------------
        public DataSources()
        {
            db = new DBAccess(DbConstants.CONNECTION_STRING);
        }
        //-----------------------------------------------------------------        
        public DataSources(string constr)
        {
            db = new DBAccess((constr == "") ? DbConstants.CONNECTION_STRING : constr);
        }
        //-----------------------------------------------------------------
        ~DataSources()
        {

        }
        //-----------------------------------------------------------------
        public virtual void Dispose()
        {

        }
        //-----------------------------------------------------------------    
        public short DataSourceId
        {
            get { return _DataSourceId; }
            set { _DataSourceId = value; }
        }
        //-----------------------------------------------------------------
        public byte DataTypeId
        {
            get { return _DataTypeId; }
            set { _DataTypeId = value; }
        }
        //-----------------------------------------------------------------
        public string DataSourceName
        {
            get { return _DataSourceName; }
            set { _DataSourceName = value; }
        }
        //-----------------------------------------------------------------
        public string DataSourceDesc
        {
            get { return _DataSourceDesc; }
            set { _DataSourceDesc = value; }
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

        private List<DataSources> Init(SqlCommand cmd)
        {
            SqlConnection con = db.GetConnection();
            cmd.Connection = con;
            List<DataSources> l_DataSources = new List<DataSources>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    DataSources m_DataSources = new DataSources(db.ConnectionString);
                    m_DataSources.DataSourceId = smartReader.GetInt16("DataSourceId");
                    m_DataSources.DataTypeId = smartReader.GetByte("DataTypeId");
                    m_DataSources.DataSourceName = smartReader.GetString("DataSourceName");
                    m_DataSources.DataSourceDesc = smartReader.GetString("DataSourceDesc");
                    m_DataSources.DisplayOrder = smartReader.GetInt16("DisplayOrder");
                    m_DataSources.CrUserId = smartReader.GetInt32("CrUserId");
                    m_DataSources.CrDateTime = smartReader.GetDateTime("CrDateTime");

                    l_DataSources.Add(m_DataSources);
                }
                reader.Close();
                return l_DataSources;
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
                SqlCommand cmd = new SqlCommand("DataSources_InsertOrUpdate");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@DataTypeId", this.DataTypeId));
                cmd.Parameters.Add(new SqlParameter("@DataSourceName", this.DataSourceName));
                cmd.Parameters.Add(new SqlParameter("@DataSourceDesc", this.DataSourceDesc));
                cmd.Parameters.Add(new SqlParameter("@DisplayOrder", this.DisplayOrder));
                cmd.Parameters.Add(new SqlParameter("@DataSourceId", this.DataSourceId)).Direction = ParameterDirection.InputOutput;
                cmd.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                db.ExecuteSQL(cmd);
                this.DataSourceId = Convert.ToInt16((cmd.Parameters["@DataSourceId"].Value == null) ? 0 : (cmd.Parameters["@DataSourceId"].Value));
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
                SqlCommand cmd = new SqlCommand("DataSources_Delete");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@DataSourceId", this.DataSourceId));
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
        public List<DataSources> GetList()
        {
            List<DataSources> RetVal = new List<DataSources>();
            try
            {
                string sql = "SELECT * FROM DataSources";
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
        public static List<DataSources> Static_GetList(string ConStr)
        {
            List<DataSources> RetVal = new List<DataSources>();
            try
            {
                DataSources m_DataSources = new DataSources(ConStr);
                RetVal = m_DataSources.GetList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //--------------------------------------------------------------     
        public static List<DataSources> Static_GetList()
        {
            return Static_GetList("");
        }
        //--------------------------------------------------------------   
        public static List<DataSources> Static_GetListAll(string ConStr, string TextOptionAll = "...")
        {
            DataSources m_DataSources = new DataSources(ConStr);
            List<DataSources> RetVal = m_DataSources.GetList();
            TextOptionAll = (TextOptionAll == null) ? "" : TextOptionAll.Trim();
            if (TextOptionAll.Length > 0)
            {
                m_DataSources.DataSourceName = TextOptionAll;
                m_DataSources.DataSourceDesc = TextOptionAll;
                RetVal.Insert(0, m_DataSources);
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public static List<DataSources> Static_GetListAll(string TextOptionAll = "...")
        {
            return Static_GetListAll("", TextOptionAll);
        }
        //--------------------------------------------------------------    
        public List<DataSources> GetListOrderBy(string OrderBy)
        {
            List<DataSources> RetVal = new List<DataSources>();
            try
            {
                OrderBy = StringUtil.InjectionString(OrderBy).Trim();
                string sql = "SELECT * FROM DataSources ";
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
        public static List<DataSources> Static_GetListOrderBy(string ConStr, string OrderBy)
        {
            DataSources m_DataSources = new DataSources(ConStr);
            return m_DataSources.GetListOrderBy(OrderBy);
        }
        //--------------------------------------------------------------    
        public static List<DataSources> Static_GetListOrderBy(string OrderBy)
        {
            return Static_GetListOrderBy("", OrderBy);
        }
        //--------------------------------------------------------------    
        public static List<DataSources> Static_GetListAllOrderBy(string ConStr, string OrderBy, string TextOptionAll = "...")
        {
            List<DataSources> RetVal = new List<DataSources>();
            DataSources m_DataSources = new DataSources(ConStr);
            try
            {
                OrderBy = StringUtil.InjectionString(OrderBy).Trim();
                if (OrderBy.Length > 0)
                {
                    RetVal = m_DataSources.GetListOrderBy(OrderBy);
                }
                TextOptionAll = (TextOptionAll == null) ? "" : TextOptionAll.Trim();
                if (TextOptionAll.Length > 0)
                {
                    m_DataSources.DataSourceName = TextOptionAll;
                    m_DataSources.DataSourceDesc = TextOptionAll;
                    RetVal.Insert(0, m_DataSources);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public static List<DataSources> Static_GetListAllOrderBy(string OrderBy, string TextOptionAll = "...")
        {
            return Static_GetListAllOrderBy("", OrderBy, TextOptionAll);
        }
        //--------------------------------------------------------------  
        public List<DataSources> GetListByDisplayTypeId(short DisplayTypeId)
        {
            List<DataSources> RetVal = new List<DataSources>();
            try
            {
                if (DisplayTypeId > 0)
                {
                    string sql = "SELECT * FROM DataSources WHERE (DisplayTypeId=" + DisplayTypeId.ToString() + ")";
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
        public List<DataSources> GetListByDataTypeId(byte DataTypeId)
        {
            List<DataSources> RetVal = new List<DataSources>();
            try
            {
                if (DataTypeId > 0)
                {
                    string sql = "SELECT * FROM DataSources WHERE (DataTypeId=" + DataTypeId.ToString() + ") ORDER BY DataSourceName";
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
        public static List<DataSources> Static_GetListByDataTypeId(string ConStr, byte DataTypeId)
        {
            List<DataSources> RetVal = new List<DataSources>();
            try
            {
                DataSources m_DataSources = new DataSources(ConStr);
                RetVal = m_DataSources.GetListByDataTypeId(DataTypeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //--------------------------------------------------------------     
        public static List<DataSources> Static_GetListByDataTypeId(byte DataTypeId)
        {
            return Static_GetListByDataTypeId("", DataTypeId);
        }
        //--------------------------------------------------------------  
        public List<DataSources> GetListByDataSourceId(short DataSourceId)
        {
            List<DataSources> RetVal = new List<DataSources>();
            try
            {
                if (DataSourceId > 0)
                {
                    string sql = "SELECT * FROM DataSources WHERE (DataSourceId=" + DataSourceId.ToString() + ")";
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
        public DataSources Get(short DataSourceId)
        {
            DataSources RetVal = new DataSources(db.ConnectionString);
            try
            {
                List<DataSources> list = GetListByDataSourceId(DataSourceId);
                if (list.Count > 0)
                {
                    RetVal = (DataSources)list[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public DataSources Get()
        {
            return Get(this.DataSourceId);
        }
        //-------------------------------------------------------------- 
        public static DataSources Static_Get(short DataSourceId)
        {
            return Static_Get(DataSourceId);
        }
        //-----------------------------------------------------------------------------
        public static DataSources Static_Get(short DataSourceId, List<DataSources> lList)
        {
            DataSources RetVal = new DataSources();
            if (DataSourceId > 0 && lList.Count > 0)
            {
                RetVal = lList.Find(i => i.DataSourceId == DataSourceId);
                if (RetVal == null) RetVal = new DataSources();
            }
            return RetVal;
        }
        //--------------------------------------------------------------     
        //public List<DataSources> GetPage(int ActUserId, int RowAmount, int PageIndex, string OrderBy, byte DataTypeId, string DataSourceName, string DateFrom, string DateTo, ref int RowCount)
        //{
        //    List<DataSources> RetVal = new List<DataSources>();
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("DataSources_GetPage");
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
        //        cmd.Parameters.Add(new SqlParameter("@RowAmount", RowAmount));
        //        cmd.Parameters.Add(new SqlParameter("@PageIndex", PageIndex));
        //        cmd.Parameters.Add(new SqlParameter("@OrderBy", StringUtil.InjectionString(OrderBy)));
        //        cmd.Parameters.Add(new SqlParameter("@DataTypeId", DataTypeId));
        //        cmd.Parameters.Add(new SqlParameter("@DataSourceName", StringUtil.InjectionString(DataSourceName)));
        //        if (DateFrom != "") cmd.Parameters.Add(new SqlParameter("@DateFrom", StringUtil.ConvertToDateTime(DateFrom)));
        //        if (DateTo != "") cmd.Parameters.Add(new SqlParameter("@DateTo", StringUtil.ConvertToDateTime(DateTo)));
        //        cmd.Parameters.Add("@RowCount", SqlDbType.Int).Direction = ParameterDirection.Output;
        //        RetVal = Init(cmd);
        //        RowCount = Convert.ToInt32(cmd.Parameters["@RowCount"].Value);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return RetVal;
        //}
        //--------------------------------------------------------------     
        //public List<DataSources> Search(int ActUserId, string OrderBy, byte DataTypeId, string DataSourceName, string DateFrom, string DateTo, ref int RowCount)
        //{
        //    int RowAmount = 0;
        //    int PageIndex = 0;
        //    return GetPage(ActUserId, RowAmount, PageIndex, OrderBy, DataTypeId, DataSourceName, DateFrom, DateTo, ref RowCount);
        //}
        //-------------------------------------------------------------- 
        public List<DataSources> GetPage(string DateFrom, string DateTo, string OrderBy, int PageSize, int PageNumber, ref int RowCount)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("DataSources_GetPage");
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@DataSourceId", this.DataSourceId));
                cmd.Parameters.Add(new SqlParameter("@DataTypeId", this.DataTypeId));
                cmd.Parameters.Add(new SqlParameter("@DataSourceName", this.DataSourceName));
                cmd.Parameters.Add(new SqlParameter("@DataSourceDesc", this.DataSourceDesc));
                cmd.Parameters.Add(new SqlParameter("@DisplayOrder", this.DisplayOrder));
                cmd.Parameters.Add(new SqlParameter("@CrUserId", this.CrUserId));
                cmd.Parameters.Add(new SqlParameter("@DateFrom", DateFrom));
                cmd.Parameters.Add(new SqlParameter("@DateTo", DateTo));
                cmd.Parameters.Add(new SqlParameter("@OrderBy", OrderBy));
                cmd.Parameters.Add(new SqlParameter("@PageSize", PageSize));
                cmd.Parameters.Add(new SqlParameter("@PageNumber", PageNumber));
                cmd.Parameters.Add("@RowCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                List<DataSources> list = Init(cmd);
                RowCount = int.Parse(cmd.Parameters["@RowCount"].Value.ToString());
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
