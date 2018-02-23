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
    public class SystemErrors
    {
        private int _SystemErrorId;
        private byte _ErrorLevelId;
        private string _SystemErrorInfo;
        private int _ObjectId;
        private string _ObjectName;
        private DateTime _CrdateTime;
        private DBAccess db;
        //-----------------------------------------------------------------
        public SystemErrors()
        {
            db = new DBAccess(DbConstants.CONNECTION_STRING);
        }
        //-----------------------------------------------------------------        
        public SystemErrors(string constr)
        {
            db = new DBAccess(constr == "" ? DbConstants.CONNECTION_STRING : constr);
        }
        //-----------------------------------------------------------------
        ~SystemErrors()
        {

        }
        //-----------------------------------------------------------------
        public virtual void Dispose()
        {

        }
        //-----------------------------------------------------------------    
        public int SystemErrorId
        {
            get { return _SystemErrorId; }
            set { _SystemErrorId = value; }
        }
        //-----------------------------------------------------------------
        public byte ErrorLevelId
        {
            get { return _ErrorLevelId; }
            set { _ErrorLevelId = value; }
        }
        //-----------------------------------------------------------------
        public string SystemErrorInfo
        {
            get { return _SystemErrorInfo; }
            set { _SystemErrorInfo = value; }
        }
        //-----------------------------------------------------------------
        public int ObjectId
        {
            get { return _ObjectId; }
            set { _ObjectId = value; }
        }
        //-----------------------------------------------------------------
        public string ObjectName
        {
            get { return _ObjectName; }
            set { _ObjectName = value; }
        }
        //-----------------------------------------------------------------
        public DateTime CrdateTime
        {
            get { return _CrdateTime; }
            set { _CrdateTime = value; }
        }
        //-----------------------------------------------------------------

        private List<SystemErrors> Init(SqlCommand cmd)
        {
            SqlConnection con = db.GetConnection();
            cmd.Connection = con;
            List<SystemErrors> listSystemErrors = new List<SystemErrors>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    SystemErrors systemError =
                        new SystemErrors(db.ConnectionString)
                        {
                            SystemErrorId = smartReader.GetInt32("SystemErrorId"),
                            ErrorLevelId = smartReader.GetByte("ErrorLevelId"),
                            SystemErrorInfo = smartReader.GetString("SystemErrorInfo"),
                            ObjectId = smartReader.GetInt32("ObjectId"),
                            ObjectName = smartReader.GetString("ObjectName"),
                            CrdateTime = smartReader.GetDateTime("CrdateTime")
                        };

                    listSystemErrors.Add(systemError);
                }
                reader.Close();
                return listSystemErrors;
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
        //-----------------------------------------------------------------    
        public DataSet GetTopRow(string myTable, int pageSize)
        {
            try
            {
                string sql = "SELECT TOP(" + pageSize + ") * FROM " + myTable + " ORDER BY CrDateTime desc";
                SqlCommand cmd = new SqlCommand(sql);
                return db.GetDataSet(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //-----------------------------------------------------------
        public List<SystemErrors> GetPage(int actUserId, int rowAmount, int pageIndex, string orderBy, string systemErrorInfo, string dateFrom, string dateTo, ref int rowCount)
        {
            List<SystemErrors> retVal = new List<SystemErrors>();
            try
            {
                SqlCommand cmd = new SqlCommand("SystemErrors_GetPage");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ActUserId", actUserId));
                cmd.Parameters.Add(new SqlParameter("@RowAmount", rowAmount));
                cmd.Parameters.Add(new SqlParameter("@PageIndex", pageIndex));
                cmd.Parameters.Add(new SqlParameter("@OrderBy", StringUtil.InjectionString(orderBy)));
                cmd.Parameters.Add(new SqlParameter("@SystemErrorInfo", StringUtil.InjectionString(systemErrorInfo)));
                if (!string.IsNullOrEmpty(dateFrom)) cmd.Parameters.Add(new SqlParameter("@DateFrom", StringUtil.ConvertToDateTime(dateFrom)));
                if (!string.IsNullOrEmpty(dateTo)) cmd.Parameters.Add(new SqlParameter("@DateTo", StringUtil.ConvertToDateTime(dateTo)));
                cmd.Parameters.Add("@RowCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                retVal = Init(cmd);
                rowCount = Convert.ToInt32(cmd.Parameters["@RowCount"].Value ?? "0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        //-------------------------------------------------------------- 

    }
}
