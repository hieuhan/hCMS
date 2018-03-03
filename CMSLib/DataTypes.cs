using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibDatabase;
using LibUtils;

namespace CMSLib
{
    public class DataTypes
    {
        private byte _DataTypeId;
        private string _DataTypeName;
        private string _DataTypeDesc;
        private byte _DisplayOrder;
        private short _FeatureGroupId;
        private DBAccess db;
        //-----------------------------------------------------------------
        public DataTypes()
        {
            db = new DBAccess(DbConstants.CONNECTION_STRING);
        }
        //-----------------------------------------------------------------        
        public DataTypes(string constr)
        {
            db = new DBAccess((constr == "") ? DbConstants.CONNECTION_STRING : constr);
        }
        //-----------------------------------------------------------------
        ~DataTypes()
        {

        }
        //-----------------------------------------------------------------
        public virtual void Dispose()
        {

        }
        //-----------------------------------------------------------------    
        public byte DataTypeId
        {
            get { return _DataTypeId; }
            set { _DataTypeId = value; }
        }
        //-----------------------------------------------------------------
        public string DataTypeName
        {
            get { return _DataTypeName; }
            set { _DataTypeName = value; }
        }
        //-----------------------------------------------------------------
        public string DataTypeDesc
        {
            get { return _DataTypeDesc; }
            set { _DataTypeDesc = value; }
        }
        //-----------------------------------------------------------------
        public byte DisplayOrder
        {
            get { return _DisplayOrder; }
            set { _DisplayOrder = value; }
        }
        //-----------------------------------------------------------------
        public short FeatureGroupId
        {
            get { return _FeatureGroupId; }
            set { _FeatureGroupId = value; }
        }
        //-----------------------------------------------------------------

        private List<DataTypes> Init(SqlCommand cmd)
        {
            SqlConnection con = db.GetConnection();
            cmd.Connection = con;
            List<DataTypes> l_DataTypes = new List<DataTypes>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    DataTypes m_DataTypes = new DataTypes(db.ConnectionString);
                    m_DataTypes.DataTypeId = smartReader.GetByte("DataTypeId");
                    m_DataTypes.DataTypeName = smartReader.GetString("DataTypeName");
                    m_DataTypes.DataTypeDesc = smartReader.GetString("DataTypeDesc");
                    m_DataTypes.DisplayOrder = smartReader.GetByte("DisplayOrder");
                    m_DataTypes.FeatureGroupId = smartReader.GetInt16("FeatureGroupId");

                    l_DataTypes.Add(m_DataTypes);
                }
                reader.Close();
                return l_DataTypes;
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
        //-------------------------------------------------------------- 
        public List<DataTypes> GetList()
        {
            List<DataTypes> RetVal = new List<DataTypes>();
            try
            {
                string sql = "SELECT * FROM DataTypes ORDER BY DisplayOrder,DataTypeName";
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
        public static List<DataTypes> Static_GetList(string ConStr)
        {
            List<DataTypes> RetVal = new List<DataTypes>();
            try
            {
                DataTypes m_DataTypes = new DataTypes(ConStr);
                RetVal = m_DataTypes.GetList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //--------------------------------------------------------------     
        public static List<DataTypes> Static_GetList()
        {
            return Static_GetList("");
        }
        //--------------------------------------------------------------   
        public static List<DataTypes> Static_GetListAll(string ConStr, string TextOptionAll = "...")
        {
            DataTypes m_DataTypes = new DataTypes(ConStr);
            List<DataTypes> RetVal = m_DataTypes.GetList();
            TextOptionAll = (TextOptionAll == null) ? "" : TextOptionAll.Trim();
            if (TextOptionAll.Length > 0)
            {
                m_DataTypes.DataTypeName = TextOptionAll;
                m_DataTypes.DataTypeDesc = TextOptionAll;
                RetVal.Insert(0, m_DataTypes);
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public static List<DataTypes> Static_GetListAll(string TextOptionAll = "...")
        {
            return Static_GetListAll("", TextOptionAll);
        }
        //--------------------------------------------------------------    
        public List<DataTypes> GetListOrderBy(string OrderBy)
        {
            List<DataTypes> RetVal = new List<DataTypes>();
            try
            {
                OrderBy = StringUtil.InjectionString(OrderBy).Trim();
                string sql = "SELECT * FROM DataTypes ";
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
        public static List<DataTypes> Static_GetListOrderBy(string ConStr, string OrderBy)
        {
            DataTypes m_DataTypes = new DataTypes(ConStr);
            return m_DataTypes.GetListOrderBy(OrderBy);
        }
        //--------------------------------------------------------------    
        public static List<DataTypes> Static_GetListOrderBy(string OrderBy)
        {
            return Static_GetListOrderBy("", OrderBy);
        }
        //--------------------------------------------------------------    
        public static List<DataTypes> Static_GetListAllOrderBy(string ConStr, string OrderBy, string TextOptionAll = "...")
        {
            List<DataTypes> RetVal = new List<DataTypes>();
            DataTypes m_DataTypes = new DataTypes(ConStr);
            try
            {
                OrderBy = StringUtil.InjectionString(OrderBy).Trim();
                if (OrderBy.Length > 0)
                {
                    RetVal = m_DataTypes.GetListOrderBy(OrderBy);
                }
                TextOptionAll = (TextOptionAll == null) ? "" : TextOptionAll.Trim();
                if (TextOptionAll.Length > 0)
                {
                    m_DataTypes.DataTypeName = TextOptionAll;
                    m_DataTypes.DataTypeDesc = TextOptionAll;
                    RetVal.Insert(0, m_DataTypes);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public static List<DataTypes> Static_GetListAllOrderBy(string OrderBy, string TextOptionAll = "...")
        {
            return Static_GetListAllOrderBy("", OrderBy, TextOptionAll);
        }
        //--------------------------------------------------------------  
        public List<DataTypes> GetListByDataTypeId(byte DataTypeId)
        {
            List<DataTypes> RetVal = new List<DataTypes>();
            try
            {
                if (DataTypeId > 0)
                {
                    string sql = "SELECT * FROM DataTypes WHERE (DataTypeId=" + DataTypeId.ToString() + ")";
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
        public DataTypes Get(byte DataTypeId)
        {
            DataTypes RetVal = new DataTypes(db.ConnectionString);
            try
            {
                List<DataTypes> list = GetListByDataTypeId(DataTypeId);
                if (list.Count > 0)
                {
                    RetVal = (DataTypes)list[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public DataTypes Get()
        {
            return Get(this.DataTypeId);
        }
        //-------------------------------------------------------------- 
        public static DataTypes Static_Get(byte DataTypeId)
        {
            return Static_Get(DataTypeId);
        }
        //-----------------------------------------------------------------------------
        public static DataTypes Static_Get(byte DataTypeId, List<DataTypes> lList)
        {
            DataTypes RetVal = new DataTypes();
            if (DataTypeId > 0 && lList.Count > 0)
            {
                RetVal = lList.Find(i => i.DataTypeId == DataTypeId);
                if (RetVal == null) RetVal = new DataTypes();
            }
            return RetVal;
        }
        //--------------------------------------------------------------
        //public static string Static_GetDescByCulture(byte DataTypesId)
        //{
        //    string RetVal = "";
        //    DataTypes m_DataTypes = new DataTypes();
        //    m_DataTypes = m_DataTypes.Get(DataTypesId);
        //    string culture = Thread.CurrentThread.CurrentCulture.Name;
        //    if (culture == CmsConstants.CULTURE_VN)
        //    {
        //        RetVal = m_DataTypes.DataTypeDesc;
        //    }
        //    else
        //    {
        //        RetVal = m_DataTypes.DataTypeName;
        //    }
        //    return RetVal;
        //}
        //--------------------------------------------------------------
    }
}
