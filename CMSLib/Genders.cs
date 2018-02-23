using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LibDatabase;
using LibUtils;

namespace CMSLib
{
    public class Genders
    {
        private byte _GenderId;
        private string _GenderName;
        private string _GenderDesc;
        private DBAccess db;
        //-----------------------------------------------------------------
        public Genders()
        {
            db = new DBAccess(DbConstants.CONNECTION_STRING);
        }
        //-----------------------------------------------------------------        
        public Genders(string constr)
        {
            db = new DBAccess((constr == "") ? DbConstants.CONNECTION_STRING : constr);
        }
        //-----------------------------------------------------------------
        ~Genders()
        {

        }
        //-----------------------------------------------------------------
        public virtual void Dispose()
        {

        }
        //-----------------------------------------------------------------    
        public byte GenderId
        {
            get { return _GenderId; }
            set { _GenderId = value; }
        }
        //-----------------------------------------------------------------
        public string GenderName
        {
            get { return _GenderName; }
            set { _GenderName = value; }
        }
        //-----------------------------------------------------------------
        public string GenderDesc
        {
            get { return _GenderDesc; }
            set { _GenderDesc = value; }
        }
        //-----------------------------------------------------------------

        private List<Genders> Init(SqlCommand cmd)
        {
            SqlConnection con = db.GetConnection();
            cmd.Connection = con;
            List<Genders> listGenders = new List<Genders>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    Genders gender = new Genders(db.ConnectionString);
                    gender.GenderId = smartReader.GetByte("GenderId");
                    gender.GenderName = smartReader.GetString("GenderName");
                    gender.GenderDesc = smartReader.GetString("GenderDesc");
                    listGenders.Add(gender);
                }
                reader.Close();
                return listGenders;
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
                SqlCommand cmd = new SqlCommand("Genders_Insert");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Replicated", replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", actUserId));
                cmd.Parameters.Add(new SqlParameter("@GenderName", this.GenderName));
                cmd.Parameters.Add(new SqlParameter("@GenderDesc", this.GenderDesc));
                cmd.Parameters.Add("@GenderId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                db.ExecuteSQL(cmd);
                this.GenderId = Convert.ToByte((cmd.Parameters["@GenderId"].Value == null) ? 0 : (cmd.Parameters["@GenderId"].Value));
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
        public byte Update(byte replicated, int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("Genders_Update");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Replicated", replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", actUserId));
                cmd.Parameters.Add(new SqlParameter("@GenderName", this.GenderName));
                cmd.Parameters.Add(new SqlParameter("@GenderDesc", this.GenderDesc));
                cmd.Parameters.Add(new SqlParameter("@GenderId", this.GenderId));
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
        public byte Delete(byte replicated, int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("Genders_Delete");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Replicated", replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", actUserId));
                cmd.Parameters.Add(new SqlParameter("@GenderId", this.GenderId));
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
        public List<Genders> GetList()
        {
            List<Genders> retVal = new List<Genders>();
            try
            {
                string sql = "SELECT * FROM Genders";
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
        public static List<Genders> Static_GetList(string conStr)
        {
            List<Genders> retVal = new List<Genders>();
            try
            {
                Genders gender = new Genders(conStr);
                retVal = gender.GetList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        //--------------------------------------------------------------     
        public static List<Genders> Static_GetList()
        {
            return Static_GetList("");
        }
        //--------------------------------------------------------------   
        public static List<Genders> Static_GetListAll(string conStr, string textOptionAll = "...")
        {
            Genders gender = new Genders(conStr);
            List<Genders> retVal = gender.GetList();
            textOptionAll = (textOptionAll == null) ? "" : textOptionAll.Trim();
            if (textOptionAll.Length > 0)
            {
                gender.GenderName = textOptionAll;
                gender.GenderDesc = textOptionAll;
                retVal.Insert(0, gender);
            }
            return retVal;
        }
        //-------------------------------------------------------------- 
        public static List<Genders> Static_GetListAll(string textOptionAll = "...")
        {
            return Static_GetListAll("", textOptionAll);
        }
        //--------------------------------------------------------------    
        public List<Genders> GetListOrderBy(string orderBy)
        {
            List<Genders> retVal = new List<Genders>();
            try
            {
                orderBy = StringUtil.InjectionString(orderBy).Trim();
                string sql = "SELECT * FROM Genders ";
                if (orderBy.Length > 0)
                {
                    sql += "ORDER BY " + orderBy;
                }
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
        public static List<Genders> Static_GetListOrderBy(string conStr, string orderBy)
        {
            Genders gender = new Genders(conStr);
            return gender.GetListOrderBy(orderBy);
        }
        //--------------------------------------------------------------    
        public static List<Genders> Static_GetListOrderBy(string orderBy)
        {
            return Static_GetListOrderBy("", orderBy);
        }
        //--------------------------------------------------------------    
        public static List<Genders> Static_GetListAllOrderBy(string conStr, string orderBy, string textOptionAll = "...")
        {
            List<Genders> retVal = new List<Genders>();
            Genders gender = new Genders(conStr);
            try
            {
                orderBy = StringUtil.InjectionString(orderBy).Trim();
                if (orderBy.Length > 0)
                {
                    retVal = gender.GetListOrderBy(orderBy);
                }
                textOptionAll = textOptionAll?.Trim() ?? "";
                if (textOptionAll.Length > 0)
                {
                    gender.GenderName = textOptionAll;
                    gender.GenderDesc = textOptionAll;
                    retVal.Insert(0, gender);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        //-------------------------------------------------------------- 
        public static List<Genders> Static_GetListAllOrderBy(string orderBy, string textOptionAll = "...")
        {
            return Static_GetListAllOrderBy("", orderBy, textOptionAll);
        }
        //--------------------------------------------------------------  
        public List<Genders> GetListByGenderId(byte genderId)
        {
            List<Genders> retVal = new List<Genders>();
            try
            {
                if (genderId > 0)
                {
                    string sql = "SELECT * FROM Genders WHERE (GenderId=" + genderId + ")";
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
        public Genders Get(byte GenderId)
        {
            Genders retVal = new Genders(db.ConnectionString);
            try
            {
                List<Genders> list = GetListByGenderId(GenderId);
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
        public Genders Get()
        {
            return Get(this.GenderId);
        }
        //-------------------------------------------------------------- 
        public static Genders Static_Get(byte genderId)
        {
            return Static_Get(genderId);
        }
        //-----------------------------------------------------------------------------
        public static Genders Static_Get(byte genderId, List<Genders> lList)
        {
            Genders retVal = new Genders();
            if (genderId > 0 && lList.Count > 0)
            {
                retVal = lList.Find(i => i.GenderId == genderId) ?? new Genders();
            }
            return retVal;
        }
        //--------------------------------------------------------------
    }
}
