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
    public class Wards
    {
        private int _WardId;
        private string _WardName;
        private string _WardDesc;
        private short _CountryId;
        private short _ProvinceId;
        private short _DistrictId;
        private int _DisplayOrder;
        private int _CrUserId;
        private DateTime _CrDateTime;
        private DBAccess db;
        //-----------------------------------------------------------------
        public Wards()
        {
            db = new DBAccess(DbConstants.CONNECTION_STRING);
        }
        //-----------------------------------------------------------------        
        public Wards(string constr)
        {
            db = (constr.Length > 0) ? new DBAccess(constr) : new DBAccess();
        }
        //-----------------------------------------------------------------
        ~Wards()
        {

        }
        //-----------------------------------------------------------------
        public virtual void Dispose()
        {

        }
        //-----------------------------------------------------------------    
        public int WardId
        {
            get { return _WardId; }
            set { _WardId = value; }
        }
        //-----------------------------------------------------------------
        public string WardName
        {
            get { return _WardName; }
            set { _WardName = value; }
        }
        //-----------------------------------------------------------------
        public string WardDesc
        {
            get { return _WardDesc; }
            set { _WardDesc = value; }
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
        public short DistrictId
        {
            get { return _DistrictId; }
            set { _DistrictId = value; }
        }
        //-----------------------------------------------------------------
        public int DisplayOrder
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

        private List<Wards> Init(SqlCommand cmd)
        {
            SqlConnection con = db.GetConnection();
            cmd.Connection = con;
            List<Wards> listWards = new List<Wards>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    Wards ward = new Wards(db.ConnectionString)
                    {
                        WardId = smartReader.GetInt32("WardId"),
                        WardName = smartReader.GetString("WardName"),
                        WardDesc = smartReader.GetString("WardDesc"),
                        CountryId = smartReader.GetInt16("CountryId"),
                        ProvinceId = smartReader.GetInt16("ProvinceId"),
                        DistrictId = smartReader.GetInt16("DistrictId"),
                        DisplayOrder = smartReader.GetInt32("DisplayOrder"),
                        CrUserId = smartReader.GetInt32("CrUserId"),
                        CrDateTime = smartReader.GetDateTime("CrDateTime")
                    };

                    listWards.Add(ward);
                }
                reader.Close();
                return listWards;
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
                SqlCommand cmd = new SqlCommand("Wards_Insert");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Replicated", replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", actUserId));
                cmd.Parameters.Add(new SqlParameter("@WardName", this.WardName));
                cmd.Parameters.Add(new SqlParameter("@WardDesc", this.WardDesc));
                cmd.Parameters.Add(new SqlParameter("@CountryId", this.CountryId));
                cmd.Parameters.Add(new SqlParameter("@ProvinceId", this.ProvinceId));
                cmd.Parameters.Add(new SqlParameter("@DistrictId", this.DistrictId));
                cmd.Parameters.Add(new SqlParameter("@DisplayOrder", this.DisplayOrder));
                cmd.Parameters.Add("@WardId", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                db.ExecuteSQL(cmd);
                this.WardId = (cmd.Parameters["@WardId"].Value == DBNull.Value) ? 0 : Convert.ToInt32(cmd.Parameters["@WardId"].Value);
                sysMessageId = Convert.ToInt16((cmd.Parameters["@SysMessageId"].Value == DBNull.Value) ? "0" : cmd.Parameters["@SysMessageId"].Value);
                retVal = Convert.ToByte((cmd.Parameters["@SysMessageTypeId"].Value == DBNull.Value) ? "0" : cmd.Parameters["@SysMessageTypeId"].Value);
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
                SqlCommand cmd = new SqlCommand("Wards_Update");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Replicated", replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", actUserId));
                cmd.Parameters.Add(new SqlParameter("@WardName", this.WardName));
                cmd.Parameters.Add(new SqlParameter("@WardDesc", this.WardDesc));
                cmd.Parameters.Add(new SqlParameter("@CountryId", this.CountryId));
                cmd.Parameters.Add(new SqlParameter("@ProvinceId", this.ProvinceId));
                cmd.Parameters.Add(new SqlParameter("@DistrictId", this.DistrictId));
                cmd.Parameters.Add(new SqlParameter("@DisplayOrder", this.DisplayOrder));
                cmd.Parameters.Add(new SqlParameter("@WardId", this.WardId));
                cmd.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                db.ExecuteSQL(cmd);
                sysMessageId = Convert.ToInt16((cmd.Parameters["@SysMessageId"].Value == DBNull.Value) ? "0" : cmd.Parameters["@SysMessageId"].Value);
                retVal = Convert.ToByte((cmd.Parameters["@SysMessageTypeId"].Value == DBNull.Value) ? "0" : cmd.Parameters["@SysMessageTypeId"].Value);
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
                SqlCommand cmd = new SqlCommand("Wards_InsertOrUpdate");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Replicated", replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", actUserId));
                cmd.Parameters.Add(new SqlParameter("@WardName", this.WardName));
                cmd.Parameters.Add(new SqlParameter("@WardDesc", this.WardDesc));
                cmd.Parameters.Add(new SqlParameter("@CountryId", this.CountryId));
                cmd.Parameters.Add(new SqlParameter("@ProvinceId", this.ProvinceId));
                cmd.Parameters.Add(new SqlParameter("@DistrictId", this.DistrictId));
                cmd.Parameters.Add(new SqlParameter("@DisplayOrder", this.DisplayOrder));
                cmd.Parameters.Add(new SqlParameter("@WardId", this.WardId)).Direction = ParameterDirection.InputOutput;
                cmd.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                db.ExecuteSQL(cmd);
                this.WardId = (cmd.Parameters["@WardId"].Value == DBNull.Value) ? 0 : Convert.ToInt32(cmd.Parameters["@WardId"].Value);
                sysMessageId = Convert.ToInt16((cmd.Parameters["@SysMessageId"].Value == DBNull.Value) ? "0" : cmd.Parameters["@SysMessageId"].Value);
                retVal = Convert.ToByte((cmd.Parameters["@SysMessageTypeId"].Value == DBNull.Value) ? "0" : cmd.Parameters["@SysMessageTypeId"].Value);
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
                SqlCommand cmd = new SqlCommand("Wards_Delete");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Replicated", replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", actUserId));
                cmd.Parameters.Add(new SqlParameter("@WardId", this.WardId));
                cmd.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                db.ExecuteSQL(cmd);
                sysMessageId = Convert.ToInt16((cmd.Parameters["@SysMessageId"].Value == DBNull.Value) ? "0" : cmd.Parameters["@SysMessageId"].Value);
                retVal = Convert.ToByte((cmd.Parameters["@SysMessageTypeId"].Value == DBNull.Value) ? "0" : cmd.Parameters["@SysMessageTypeId"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        //-------------------------------------------------------------- 
        public List<Wards> GetList(short districtId)
        {
            List<Wards> retVal = new List<Wards>();
            try
            {
                string sql = "SELECT * FROM Wards WHERE DistrictId=" + districtId + " ORDER BY DisplayOrder,WardDesc";
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
        public List<Wards> GetListOrderBy(string orderBy)
        {
            List<Wards> retVal = new List<Wards>();
            try
            {
                string sql = "SELECT * FROM Wards ORDER BY " + StringUtil.InjectionString(orderBy);
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
        public List<Wards> GetListByWardId(int wardId)
        {
            List<Wards> retVal = new List<Wards>();
            try
            {
                if (wardId > 0)
                {
                    string sql = "SELECT * FROM Wards WHERE (WardId=" + wardId + ")";
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
        public Wards Get(int wardId)
        {
            Wards retVal = new Wards();
            try
            {
                List<Wards> list = GetListByWardId(wardId);
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
        public static Wards Static_Get(int wardId, List<Wards> list)
        {
            Wards retVal;
            try
            {
                retVal = list.Find(i => i.WardId == wardId) ?? new Wards();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        //-------------------------------------------------------------- 
        public Wards Get()
        {
            return Get(this.WardId);
        }
        //-------------------------------------------------------------- 
        public static Wards Static_Get(int wardId)
        {
            return Static_Get(wardId);
        }
        //--------------------------------------------------------------     
        public static List<Wards> Static_GetList(short districtId)
        {
            List<Wards> retVal = new List<Wards>();
            try
            {
                retVal = new Wards().GetList(districtId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        //--------------------------------------------------------------     
        public List<Wards> GetPage(int actUserId, int rowAmount, int pageIndex, string orderBy, ref int rowCount)
        {
            List<Wards> retVal = new List<Wards>();
            try
            {
                SqlCommand cmd = new SqlCommand("Wards_GetPage");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ActUserId", actUserId));
                cmd.Parameters.Add(new SqlParameter("@RowAmount", rowAmount));
                cmd.Parameters.Add(new SqlParameter("@PageIndex", pageIndex));
                cmd.Parameters.Add(new SqlParameter("@OrderBy", orderBy));
                cmd.Parameters.Add(new SqlParameter("@WardName", this.WardName));
                cmd.Parameters.Add(new SqlParameter("@CountryId", this.CountryId));
                cmd.Parameters.Add(new SqlParameter("@ProvinceId", this.ProvinceId));
                cmd.Parameters.Add(new SqlParameter("@DistrictId", this.DistrictId));
                cmd.Parameters.Add("@RowCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                retVal = Init(cmd);
                rowCount = Convert.ToInt32(cmd.Parameters["@RowCount"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
    }
}
