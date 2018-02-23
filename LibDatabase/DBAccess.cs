using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibDatabase
{
    public class DBAccess
    {
        private string _connectionString;

        public string ConnectionString
        {
            get
            {
                return this._connectionString;
            }
            set
            {
                this._connectionString = value;
            }
        }

        public DBAccess()
        {
            this._connectionString = DbConstants.CONNECTION_STRING;
        }

        public DBAccess(string connStr)
        {
            this._connectionString = connStr.Length > 0 ? connStr : DbConstants.CONNECTION_STRING;
        }

        public DBAccess(string server, string userName, string userPass, string dbName)
        {
            this._connectionString = DBAccess.BuildConstr(server, userName, userPass, dbName);
        }

        public static string BuildConstr(string server, string userName, string userPass, string dbName)
        {
            if (server.Length > 0 && userName.Length > 0 && userPass.Length > 0 && dbName.Length > 0)
            {
                return string.Format("server={0};uid={1};pwd={2};database={3};", server, userName, userPass, dbName);
            }
            return DbConstants.CONNECTION_STRING;
        }

        public string SelectConnStr(string connStr, string ip1, string ip2)
        {
            string text = ExactValue(connStr, "server", ';');
            if (text == ip1)
            {
                if (this.CheckConnection(connStr))
                {
                    return connStr;
                }
                return connStr.Replace(text, ip2);
            }
            if (this.CheckConnection(connStr))
            {
                return connStr;
            }
            return connStr.Replace(text, ip1);
        }

        public bool CheckConnection(string connStr)
        {
            bool result = false;
            try
            {
                SqlConnection sqlConnection = new SqlConnection(connStr);
                sqlConnection.Open();
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                    result = true;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public SqlConnection GetConnection()
        {
            try
            {
                return new SqlConnection(this._connectionString);
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Khong ket noi duoc toi CSDL: " + ex.Message);
            }
        }

        public OleDbConnection GetOleConnection()
        {
            try
            {
                return new OleDbConnection(this._connectionString);
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Khong ket noi duoc toi CSDL: " + ex.Message);
            }
        }

        public void closeConnection(SqlConnection con)
        {
            try
            {
                if (con != null || con.State == ConnectionState.Open)
                {
                    con.Close();
                    con.Dispose();
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public SqlConnection OpenConnection(string connectionString)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                return sqlConnection;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public SqlConnection OpenConnection()
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(this._connectionString);
                sqlConnection.Open();
                return sqlConnection;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void CloseConnection(SqlConnection mySqlConnection)
        {
            try
            {
                if (mySqlConnection.State == ConnectionState.Open)
                {
                    mySqlConnection.Close();
                    mySqlConnection.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataSet GetDataSet(string strSQL)
        {
            DBAccess dBAccess = new DBAccess();
            try
            {
                SqlConnection sqlConnection = dBAccess.OpenConnection(this._connectionString);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(strSQL, sqlConnection);
                DataSet dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet);
                sqlDataAdapter.Dispose();
                dBAccess.CloseConnection(sqlConnection);
                return dataSet;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " => " + strSQL);
            }
        }

        public SqlDataReader GetReader(SqlCommand cmd)
        {
            DBAccess dBAccess = new DBAccess();
            try
            {
                SqlConnection sqlConnection2 = cmd.Connection = dBAccess.OpenConnection(this._connectionString);
                return cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " => " + cmd.CommandText);
            }
        }

        public SqlDataReader GetReader(SqlCommand cmd, SqlConnection con)
        {
            try
            {
                cmd.Connection = con;
                con.Open();
                return cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " => " + cmd.CommandText);
            }
        }

        public DataTable GetDataTable(string strSQL)
        {
            return this.GetDataSet(strSQL).Tables[0];
        }

        public DataSet GetDataSet(string strSQL, SqlParameter[] parameters)
        {
            DBAccess dBAccess = new DBAccess();
            try
            {
                SqlConnection sqlConnection = dBAccess.OpenConnection(this._connectionString);
                SqlCommand sqlCommand = new SqlCommand(strSQL, sqlConnection);
                sqlCommand.Parameters.Clear();
                foreach (SqlParameter value in parameters)
                {
                    sqlCommand.Parameters.Add(value);
                }
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                DataSet dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet);
                dBAccess.CloseConnection(sqlConnection);
                return dataSet;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " => " + strSQL);
            }
        }

        public DataTable GetDataTable(string strSQL, params SqlParameter[] parameters)
        {
            return this.GetDataSet(strSQL, parameters).Tables[0];
        }

        public DataSet GetDataSet(SqlCommand mCommand)
        {
            DBAccess dBAccess = new DBAccess();
            try
            {
                SqlConnection mySqlConnection = mCommand.Connection = dBAccess.OpenConnection(this._connectionString);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(mCommand);
                DataSet dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet);
                dBAccess.CloseConnection(mySqlConnection);
                return dataSet;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " => " + mCommand.CommandText);
            }
        }

        public DataTable GetDataTable(SqlCommand mCommand)
        {
            return this.GetDataSet(mCommand).Tables[0];
        }

        public void ExecuteSQL(string strSQL)
        {
            DBAccess dBAccess = new DBAccess();
            SqlConnection sqlConnection = dBAccess.OpenConnection(this._connectionString);
            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(strSQL, sqlConnection) { Transaction = sqlTransaction };
                sqlCommand.ExecuteNonQuery();
                sqlTransaction.Commit();
                sqlCommand.Dispose();
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                throw new Exception(ex.Message + " => " + strSQL);
            }
            finally
            {
                dBAccess.CloseConnection(sqlConnection);
            }
        }

        public void ExecuteSQL(string strSQL, params SqlParameter[] parameters)
        {
            DBAccess dBAccess = new DBAccess();
            try
            {
                SqlConnection sqlConnection = dBAccess.OpenConnection(this._connectionString);
                SqlCommand sqlCommand = new SqlCommand(strSQL, sqlConnection);
                sqlCommand.Parameters.Clear();
                foreach (SqlParameter value in parameters)
                {
                    sqlCommand.Parameters.Add(value);
                }
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Dispose();
                dBAccess.CloseConnection(sqlConnection);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " => " + strSQL);
            }
        }

        public void ExecuteSQL(string strSQL, IList paraList)
        {
            DBAccess dBAccess = new DBAccess();
            try
            {
                SqlConnection sqlConnection = dBAccess.OpenConnection(this._connectionString);
                SqlCommand sqlCommand = new SqlCommand(strSQL, sqlConnection);
                sqlCommand.Parameters.Clear();
                foreach (SqlParameter para in paraList)
                {
                    sqlCommand.Parameters.Add(para);
                }
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Dispose();
                dBAccess.CloseConnection(sqlConnection);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " => " + strSQL);
            }
        }

        public void ExecuteSQL(SqlCommand mCommand)
        {
            DBAccess dBAccess = new DBAccess();
            try
            {
                SqlConnection mySqlConnection = mCommand.Connection = dBAccess.OpenConnection(this._connectionString);
                mCommand.ExecuteNonQuery();
                mCommand.Dispose();
                dBAccess.CloseConnection(mySqlConnection);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " => " + mCommand.CommandText);
            }
        }

        public void ExecuteSQL(string strSQL, SqlConnection mConn)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand(strSQL, mConn);
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " => " + strSQL);
            }
        }

        public int ExecuteScalar(string strSQL)
        {
            int num = 0;
            DBAccess dBAccess = new DBAccess();
            try
            {
                SqlConnection sqlConnection = dBAccess.OpenConnection(this._connectionString);
                SqlCommand sqlCommand = new SqlCommand(strSQL, sqlConnection);
                num = Convert.ToInt32(sqlCommand.ExecuteScalar().ToString());
                sqlCommand.Dispose();
                dBAccess.CloseConnection(sqlConnection);
                return num;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " => " + strSQL);
            }
        }

        public int ExecuteScalar(string strSQL, params SqlParameter[] parameters)
        {
            int num = 0;
            DBAccess dBAccess = new DBAccess();
            try
            {
                SqlConnection sqlConnection = dBAccess.OpenConnection(this._connectionString);
                SqlCommand sqlCommand = new SqlCommand(strSQL, sqlConnection);
                sqlCommand.Parameters.Clear();
                foreach (SqlParameter value in parameters)
                {
                    sqlCommand.Parameters.Add(value);
                }
                num = Convert.ToInt32(sqlCommand.ExecuteScalar().ToString());
                sqlCommand.Dispose();
                dBAccess.CloseConnection(sqlConnection);
                return num;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " => " + strSQL);
            }
        }

        public int ExecuteScalar(string strSQL, IList paraList)
        {
            int num = 0;
            DBAccess dBAccess = new DBAccess();
            try
            {
                SqlConnection sqlConnection = dBAccess.OpenConnection(this._connectionString);
                SqlCommand sqlCommand = new SqlCommand(strSQL, sqlConnection);
                sqlCommand.Parameters.Clear();
                foreach (SqlParameter para in paraList)
                {
                    sqlCommand.Parameters.Add(para);
                }
                num = Convert.ToInt32(sqlCommand.ExecuteScalar().ToString());
                sqlCommand.Dispose();
                dBAccess.CloseConnection(sqlConnection);
                return num;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " => " + strSQL);
            }
        }

        public int ExecuteScalar(SqlCommand mCommand)
        {
            int num = 0;
            DBAccess dBAccess = new DBAccess();
            try
            {
                SqlConnection mySqlConnection = mCommand.Connection = dBAccess.OpenConnection(this._connectionString);
                num = Convert.ToInt32(mCommand.ExecuteScalar().ToString());
                mCommand.Dispose();
                dBAccess.CloseConnection(mySqlConnection);
                return num;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " => " + mCommand.CommandText);
            }
        }

        public int ExecuteScalar(string strSQL, SqlConnection mConn)
        {
            int num = 0;
            try
            {
                SqlCommand sqlCommand = new SqlCommand(strSQL, mConn);
                num = Convert.ToInt32(sqlCommand.ExecuteScalar().ToString());
                sqlCommand.Dispose();
                return num;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " => " + strSQL);
            }
        }

        public object ExecuteScalarReturnObj(string strSQL)
        {
            DBAccess dBAccess = new DBAccess();
            try
            {
                SqlConnection sqlConnection = dBAccess.OpenConnection(this._connectionString);
                SqlCommand sqlCommand = new SqlCommand(strSQL, sqlConnection);
                object result = sqlCommand.ExecuteScalar();
                sqlCommand.Dispose();
                dBAccess.CloseConnection(sqlConnection);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " => " + strSQL);
            }
        }

        public DataTable SelectDBRows(string query)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(this._connectionString);
                DataTable dataTable = new DataTable();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlConnection.Open();
                sqlDataAdapter.SelectCommand = new SqlCommand(query, sqlConnection);
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
                sqlDataAdapter.Dispose();
                sqlConnection.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long GetNumber(string Query)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(this._connectionString);
                SqlCommand sqlCommand = new SqlCommand(Query, sqlConnection);
                sqlConnection.Open();
                object obj = (int)sqlCommand.ExecuteScalar();
                sqlConnection.Close();
                sqlConnection.Dispose();
                sqlCommand.Dispose();
                return (long)obj;
            }
            catch
            {
                return 0L;
            }
        }

        public string GetString(string Query)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(this._connectionString);
                SqlCommand sqlCommand = new SqlCommand(Query, sqlConnection);
                sqlConnection.Open();
                object obj = (string)sqlCommand.ExecuteScalar();
                sqlConnection.Close();
                sqlConnection.Dispose();
                sqlCommand.Dispose();
                return (string)obj;
            }
            catch
            {
                return "";
            }
        }

        public static string ExactValue(string str, string objName, char delimiter)
        {
            string[] array = str.Split(delimiter);
            string result = "";
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Contains(objName))
                {
                    result = array[i].Replace(objName, "");
                    result = result.Replace("=", "").Trim();
                    i = array.Length;
                }
            }
            return result;
        }
    }
}
