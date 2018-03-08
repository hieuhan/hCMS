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
    public class Provinces
    {
        private short _ProvinceId;
        private string _ProvinceName;
        private string _ProvinceDesc;
        private short _CountryId;
        private short _DisplayOrder;
        private int _CrUserId;
        private DateTime _CrDateTime;
        private int _SoLuong;

        public int CustomerProvinceId
        {
            get { return _customerProvinceId; }
            set { _customerProvinceId = value; }
        }

        private DBAccess db;

        private int _customerProvinceId;

        //-----------------------------------------------------------------
        public Provinces()
        {
            db = new DBAccess(DbConstants.CONNECTION_STRING);
        }
        //-----------------------------------------------------------------        
        public Provinces(string constr)
        {
            db = new DBAccess((constr == "") ? DbConstants.CONNECTION_STRING : constr);
        }
        //-----------------------------------------------------------------
        ~Provinces()
        {

        }
        //-----------------------------------------------------------------
        public virtual void Dispose()
        {

        }
        //-----------------------------------------------------------------    
        public short ProvinceId
        {
            get { return _ProvinceId; }
            set { _ProvinceId = value; }
        }
        //-----------------------------------------------------------------
        public string ProvinceName
        {
            get { return _ProvinceName; }
            set { _ProvinceName = value; }
        }
        //-----------------------------------------------------------------
        public string ProvinceDesc
        {
            get { return _ProvinceDesc; }
            set { _ProvinceDesc = value; }
        }
        //-----------------------------------------------------------------
        public short CountryId
        {
            get { return _CountryId; }
            set { _CountryId = value; }
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
        public int SoLuong
        {
            get { return _SoLuong; }
            set { _SoLuong = value; }
        }
        //-----------------------------------------------------------------

        private List<Provinces> Init(SqlCommand cmd)
        {
            SqlConnection con = db.GetConnection();
            cmd.Connection = con;
            List<Provinces> listProvinces = new List<Provinces>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    Provinces province =
                        new Provinces(db.ConnectionString)
                        {
                            ProvinceId = smartReader.GetInt16("ProvinceId"),
                            ProvinceName = smartReader.GetString("ProvinceName"),
                            ProvinceDesc = smartReader.GetString("ProvinceDesc"),
                            CountryId = smartReader.GetInt16("CountryId"),
                            DisplayOrder = smartReader.GetInt16("DisplayOrder"),
                            CrUserId = smartReader.GetInt32("CrUserId"),
                            CrDateTime = smartReader.GetDateTime("CrDateTime")
                        };

                    listProvinces.Add(province);
                }
                reader.Close();
                return listProvinces;
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
        public byte Insert(byte replicated, int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                retVal = InsertOrUpdate(replicated, actUserId, ref sysMessageId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        //--------------------------------------------------------------
        public byte Update(byte replicated, int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                retVal = InsertOrUpdate(replicated, actUserId, ref sysMessageId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        //-----------------------------------------------------------
        public byte InsertOrUpdate(byte replicated, int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("Provinces_InsertOrUpdate");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Replicated", replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", actUserId));
                cmd.Parameters.Add(new SqlParameter("@ProvinceName", this.ProvinceName));
                cmd.Parameters.Add(new SqlParameter("@ProvinceDesc", this.ProvinceDesc));
                cmd.Parameters.Add(new SqlParameter("@CountryId", this.CountryId));
                cmd.Parameters.Add(new SqlParameter("@DisplayOrder", this.DisplayOrder));
                cmd.Parameters.Add(new SqlParameter("@ProvinceId", this.ProvinceId)).Direction = ParameterDirection.InputOutput;
                cmd.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                db.ExecuteSQL(cmd);
                this.ProvinceId = Convert.ToInt16((cmd.Parameters["@ProvinceId"].Value == null) ? 0 : (cmd.Parameters["@ProvinceId"].Value));
                sysMessageId = Convert.ToInt16((cmd.Parameters["@SysMessageId"].Value == null) ? "0" : cmd.Parameters["@SysMessageId"].Value);
                retVal = Convert.ToByte((cmd.Parameters["@SysMessageTypeId"].Value == null) ? "0" : cmd.Parameters["@SysMessageTypeId"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        //--------------------------------------------------------------            
        public byte Delete(byte replicated, int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("Provinces_Delete");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Replicated", replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", actUserId));
                cmd.Parameters.Add(new SqlParameter("@ProvinceId", this.ProvinceId));
                cmd.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                db.ExecuteSQL(cmd);
                sysMessageId = Convert.ToInt16((cmd.Parameters["@SysMessageId"].Value == null) ? "0" : cmd.Parameters["@SysMessageId"].Value);
                retVal = Convert.ToByte((cmd.Parameters["@SysMessageTypeId"].Value == null) ? "0" : cmd.Parameters["@SysMessageTypeId"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        //-------------------------------------------------------------- 
        public List<Provinces> GetList(short countryId)
        {
            List<Provinces> retVal = new List<Provinces>();
            try
            {
                string sql = "SELECT * FROM Provinces" + (countryId == 0 ? "" : " WHERE CountryId=" + countryId) + " ORDER BY DisplayOrder, ProvinceName";
                SqlCommand cmd = new SqlCommand(sql);
                retVal = Init(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        //--------------------------------------------------------------
        public static List<Provinces> Static_GetList(short countryId)
        {
            List<Provinces> retVal = new List<Provinces>();
            try
            {
                retVal = new Provinces().GetList(countryId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        //--------------------------------------------------------------  
        public List<Provinces> GetListByProvinceId(short provinceId)
        {
            List<Provinces> retVal = new List<Provinces>();
            try
            {
                if (provinceId > 0)
                {
                    string sql = "SELECT * FROM Provinces WHERE (ProvinceId=" + provinceId + ")";
                    SqlCommand cmd = new SqlCommand(sql);
                    retVal = Init(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        //--------------------------------------------------------------     
        public static Provinces Static_Get(short provinceId, List<Provinces> list)
        {
            var retVal = list.Find(i => i.ProvinceId == provinceId) ?? new Provinces();
            return retVal;
        }
        //-------------------------------------------------------------- 
        public Provinces Get(short provinceId)
        {
            Provinces retVal = new Provinces(db.ConnectionString);
            try
            {
                List<Provinces> list = GetListByProvinceId(provinceId);
                if (list.Count > 0)
                {
                    retVal = list[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        //-------------------------------------------------------------- 
        public Provinces Get()
        {
            return Get(this.ProvinceId);
        }
        //-------------------------------------------------------------- 
        public static Provinces Static_Get(short provinceId)
        {
            return new Provinces().Get(provinceId);
        }
        //--------------------------------------------------------------     
        public List<Provinces> Provinces_GetList(int actUserId, string orderBy, string provinceName, short countryId)
        {
            List<Provinces> retVal = new List<Provinces>();
            try
            {
                SqlCommand cmd = new SqlCommand("Provinces_GetList");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ActUserId", actUserId));
                cmd.Parameters.Add(new SqlParameter("@OrderBy", StringUtil.InjectionString(orderBy)));
                cmd.Parameters.Add(new SqlParameter("@ProvinceName", StringUtil.InjectionString(provinceName)));
                cmd.Parameters.Add(new SqlParameter("@CountryId", countryId));
                retVal = Init(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        //-------------------------------------------------------------- 
        public DataSet Provinces_GetNameByJson(string name, int rowAmount, ref string result)
        {
            DataSet retVal = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Provinces_GetNameByJson");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Name", name));
                cmd.Parameters.Add(new SqlParameter("@RowAmount", rowAmount));
                cmd.Parameters.Add("@Result", SqlDbType.NVarChar, 4000).Direction = ParameterDirection.Output;

                retVal = db.GetDataSet(cmd);
                result = Convert.ToString(cmd.Parameters["@Result"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        //-------------------------------------------------------------- 

        public List<Provinces> GetPage(string DateFrom, string DateTo, string OrderBy, int PageSize, int PageNumber, ref int RowCount)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Provinces_GetPage");
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ProvinceId", this.ProvinceId));
                cmd.Parameters.Add(new SqlParameter("@ProvinceName", this.ProvinceName));
                cmd.Parameters.Add(new SqlParameter("@ProvinceDesc", this.ProvinceDesc));
                cmd.Parameters.Add(new SqlParameter("@CountryId", this.CountryId));
                cmd.Parameters.Add(new SqlParameter("@DisplayOrder", this.DisplayOrder));
                cmd.Parameters.Add(new SqlParameter("@CrUserId", this.CrUserId));
                cmd.Parameters.Add(new SqlParameter("@DateFrom", DateFrom));
                cmd.Parameters.Add(new SqlParameter("@DateTo", DateTo));
                cmd.Parameters.Add(new SqlParameter("@OrderBy", OrderBy));
                cmd.Parameters.Add(new SqlParameter("@PageSize", PageSize));
                cmd.Parameters.Add(new SqlParameter("@PageNumber", PageNumber));
                cmd.Parameters.Add("@RowCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                List<Provinces> list = Init(cmd);
                RowCount = int.Parse(cmd.Parameters["@RowCount"].Value.ToString());
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //--------------------------------------------------------------
        public static string Static_GetDisplayString(short ProvinceId)
        {
            string RetVal = "";
            Provinces m_Provinces = new Provinces();
            m_Provinces.ProvinceId = ProvinceId;
            m_Provinces = m_Provinces.Get();
            RetVal = m_Provinces.ProvinceName;
            return RetVal;
        }

    }
}
