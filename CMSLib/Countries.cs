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
    public class Countries
    {
        private short _CountryId;
        private string _CountryName;
        private string _CountryDesc;
        private string _IconPath;
        private short _DisplayOrder;
        private int _CrUserId;
        private DateTime _CrDateTime;
        private DBAccess db;
        //-----------------------------------------------------------------
        public Countries()
        {
            db = new DBAccess(DbConstants.CONNECTION_STRING);
        }
        //-----------------------------------------------------------------        
        public Countries(string constr)
        {
            db = new DBAccess((constr == "") ? DbConstants.CONNECTION_STRING : constr);
        }
        //-----------------------------------------------------------------
        ~Countries()
        {

        }
        //-----------------------------------------------------------------
        public virtual void Dispose()
        {

        }
        //-----------------------------------------------------------------    
        public short CountryId
        {
            get { return _CountryId; }
            set { _CountryId = value; }
        }
        //-----------------------------------------------------------------
        public string CountryName
        {
            get { return _CountryName; }
            set { _CountryName = value; }
        }
        //-----------------------------------------------------------------
        public string CountryDesc
        {
            get { return _CountryDesc; }
            set { _CountryDesc = value; }
        }
        //-----------------------------------------------------------------
        public string IconPath
        {
            get { return _IconPath; }
            set { _IconPath = value; }
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

        private List<Countries> Init(SqlCommand cmd)
        {
            SqlConnection con = db.GetConnection();
            cmd.Connection = con;
            List<Countries> l_Countries = new List<Countries>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    Countries m_Countries = new Countries(db.ConnectionString);
                    m_Countries.CountryId = smartReader.GetInt16("CountryId");
                    m_Countries.CountryName = smartReader.GetString("CountryName");
                    m_Countries.CountryDesc = smartReader.GetString("CountryDesc");
                    m_Countries.IconPath = smartReader.GetString("IconPath");
                    m_Countries.DisplayOrder = smartReader.GetInt16("DisplayOrder");
                    m_Countries.CrUserId = smartReader.GetInt32("CrUserId");
                    m_Countries.CrDateTime = smartReader.GetDateTime("CrDateTime");

                    l_Countries.Add(m_Countries);
                }
                reader.Close();
                return l_Countries;
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
                SqlCommand cmd = new SqlCommand("Countries_InsertOrUpdate");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@CountryName", this.CountryName));
                cmd.Parameters.Add(new SqlParameter("@CountryDesc", this.CountryDesc));
                cmd.Parameters.Add(new SqlParameter("@IconPath", this.IconPath));
                cmd.Parameters.Add(new SqlParameter("@DisplayOrder", this.DisplayOrder));
                cmd.Parameters.Add(new SqlParameter("@CountryId", this.CountryId)).Direction = ParameterDirection.InputOutput;
                cmd.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                db.ExecuteSQL(cmd);
                this.CountryId = Convert.ToInt16((cmd.Parameters["@CountryId"].Value == null) ? 0 : (cmd.Parameters["@CountryId"].Value));
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
                SqlCommand cmd = new SqlCommand("Countries_Delete");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@CountryId", this.CountryId));
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
        public List<Countries> GetList()
        {
            List<Countries> RetVal = new List<Countries>();
            try
            {
                string sql = "SELECT * FROM Countries";
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
        public static List<Countries> Static_GetList(string ConStr)
        {
            List<Countries> RetVal = new List<Countries>();
            try
            {
                Countries m_Countries = new Countries(ConStr);
                RetVal = m_Countries.GetList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //--------------------------------------------------------------     
        public static List<Countries> Static_GetList()
        {
            return Static_GetList("");
        }
        //--------------------------------------------------------------   
        public static List<Countries> Static_GetListAll(string ConStr, string TextOptionAll = "...")
        {
            Countries m_Countries = new Countries(ConStr);
            List<Countries> RetVal = m_Countries.GetList();
            TextOptionAll = (TextOptionAll == null) ? "" : TextOptionAll.Trim();
            if (TextOptionAll.Length > 0)
            {
                m_Countries.CountryName = TextOptionAll;
                m_Countries.CountryDesc = TextOptionAll;
                RetVal.Insert(0, m_Countries);
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public static List<Countries> Static_GetListAll(string TextOptionAll = "...")
        {
            return Static_GetListAll("", TextOptionAll);
        }
        //--------------------------------------------------------------    
        public List<Countries> GetListOrderBy(string OrderBy)
        {
            List<Countries> RetVal = new List<Countries>();
            try
            {
                OrderBy = StringUtil.InjectionString(OrderBy).Trim();
                string sql = "SELECT * FROM Countries ";
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
        public static List<Countries> Static_GetListOrderBy(string ConStr, string OrderBy)
        {
            Countries m_Countries = new Countries(ConStr);
            return m_Countries.GetListOrderBy(OrderBy);
        }
        //--------------------------------------------------------------    
        public static List<Countries> Static_GetListOrderBy(string OrderBy)
        {
            return Static_GetListOrderBy("", OrderBy);
        }
        //--------------------------------------------------------------    
        public static List<Countries> Static_GetListAllOrderBy(string ConStr, string OrderBy, string TextOptionAll = "...")
        {
            List<Countries> RetVal = new List<Countries>();
            Countries m_Countries = new Countries(ConStr);
            try
            {
                OrderBy = StringUtil.InjectionString(OrderBy).Trim();
                if (OrderBy.Length > 0)
                {
                    RetVal = m_Countries.GetListOrderBy(OrderBy);
                }
                TextOptionAll = (TextOptionAll == null) ? "" : TextOptionAll.Trim();
                if (TextOptionAll.Length > 0)
                {
                    m_Countries.CountryName = TextOptionAll;
                    m_Countries.CountryDesc = TextOptionAll;
                    RetVal.Insert(0, m_Countries);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public static List<Countries> Static_GetListAllOrderBy(string OrderBy, string TextOptionAll = "...")
        {
            return Static_GetListAllOrderBy("", OrderBy, TextOptionAll);
        }
        //--------------------------------------------------------------  
        public List<Countries> GetListByCountryId(short CountryId)
        {
            List<Countries> RetVal = new List<Countries>();
            try
            {
                if (CountryId > 0)
                {
                    string sql = "SELECT * FROM Countries WHERE (CountryId=" + CountryId.ToString() + ")";
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
        public Countries Get(short CountryId)
        {
            Countries RetVal = new Countries(db.ConnectionString);
            try
            {
                List<Countries> list = GetListByCountryId(CountryId);
                if (list.Count > 0)
                {
                    RetVal = (Countries)list[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //-------------------------------------------------------------- 
        public Countries Get()
        {
            return Get(this.CountryId);
        }
        //-------------------------------------------------------------- 
        public static Countries Static_Get(short CountryId)
        {
            return Static_Get(CountryId);
        }
        //--------------------------------------------------------------     
        public List<Countries> Countries_GetList(int ActUserId, string OrderBy, string CountryName)
        {
            List<Countries> RetVal = new List<Countries>();
            try
            {
                SqlCommand cmd = new SqlCommand("Countries_GetList");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@OrderBy", StringUtil.InjectionString(OrderBy)));
                cmd.Parameters.Add(new SqlParameter("@CountryName", StringUtil.InjectionString(CountryName)));
                RetVal = Init(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //--------------------------------------------------------------     
        public static Countries Static_Get(short CountryId, List<Countries> list)
        {
            Countries RetVal = new Countries();
            RetVal = list.Find(i => i.CountryId == CountryId);
            if (RetVal == null) RetVal = new Countries();
            return RetVal;
        }
        //--------------------------------------------------------------     

    }
}
