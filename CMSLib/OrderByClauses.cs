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
    public class OrderByClauses
    {
        private int _OrderByClauseId;
        private string _TableName;
        private string _OrderByName;
        private string _OrderByDesc;
        private string _OrderBy;
        private byte _DisplayOrder;
        private DateTime _CrDateTime;
        private DBAccess db;
        //-----------------------------------------------------------------
        public OrderByClauses()
        {
            db = new DBAccess(DbConstants.CONNECTION_STRING);
        }
        //-----------------------------------------------------------------        
        public OrderByClauses(string constr)
        {
            db = new DBAccess((constr == "") ? DbConstants.CONNECTION_STRING : constr);
        }
        //-----------------------------------------------------------------
        ~OrderByClauses()
        {

        }
        //-----------------------------------------------------------------
        public virtual void Dispose()
        {

        }
        //-----------------------------------------------------------------    
        public int OrderByClauseId
        {
            get { return _OrderByClauseId; }
            set { _OrderByClauseId = value; }
        }
        //-----------------------------------------------------------------
        public string TableName
        {
            get { return _TableName; }
            set { _TableName = value; }
        }
        //-----------------------------------------------------------------
        public string OrderByName
        {
            get { return _OrderByName; }
            set { _OrderByName = value; }
        }
        //-----------------------------------------------------------------
        public string OrderByDesc
        {
            get { return _OrderByDesc; }
            set { _OrderByDesc = value; }
        }
        //-----------------------------------------------------------------
        public string OrderBy
        {
            get { return _OrderBy; }
            set { _OrderBy = value; }
        }
        //-----------------------------------------------------------------
        public byte DisplayOrder
        {
            get { return _DisplayOrder; }
            set { _DisplayOrder = value; }
        }
        //-----------------------------------------------------------------
        public DateTime CrDateTime
        {
            get { return _CrDateTime; }
            set { _CrDateTime = value; }
        }
        //-----------------------------------------------------------------

        private List<OrderByClauses> Init(SqlCommand cmd)
        {
            SqlConnection con = db.GetConnection();
            cmd.Connection = con;
            List<OrderByClauses> l_OrderByClauses = new List<OrderByClauses>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    OrderByClauses m_OrderByClauses = new OrderByClauses(db.ConnectionString);
                    m_OrderByClauses.OrderByClauseId = smartReader.GetInt32("OrderByClauseId");
                    m_OrderByClauses.TableName = smartReader.GetString("TableName");
                    m_OrderByClauses.OrderByName = smartReader.GetString("OrderByName");
                    m_OrderByClauses.OrderByDesc = smartReader.GetString("OrderByDesc");
                    m_OrderByClauses.OrderBy = smartReader.GetString("OrderBy");
                    m_OrderByClauses.DisplayOrder = smartReader.GetByte("DisplayOrder");
                    m_OrderByClauses.CrDateTime = smartReader.GetDateTime("CrDateTime");

                    l_OrderByClauses.Add(m_OrderByClauses);
                }
                reader.Close();
                return l_OrderByClauses;
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
        public List<OrderByClauses> GetList(string TableName)
        {
            List<OrderByClauses> RetVal = new List<OrderByClauses>();
            try
            {
                string sql = "SELECT * FROM OrderByClauses WHERE TableName='" + StringUtil.InjectionString(TableName) + "' ORDER BY DisplayOrder";
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
        public static List<OrderByClauses> Static_GetList(string ConStr, string TableName)
        {
            List<OrderByClauses> RetVal = new List<OrderByClauses>();
            try
            {
                OrderByClauses m_OrderByClauses = new OrderByClauses(ConStr);
                RetVal = m_OrderByClauses.GetList(TableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //--------------------------------------------------------------     
        public static List<OrderByClauses> Static_GetList(string TableName)
        {
            return Static_GetList("", TableName);
        }
        //--------------------------------------------------------------     
    }
}
