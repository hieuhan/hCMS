using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibDatabase
{
    public sealed class SmartDataReader
    {
        private DateTime _defaultDate;

        private SqlDataReader _reader;

        public SmartDataReader()
        {
        }

        public SmartDataReader(SqlDataReader reader)
        {
            this._defaultDate = DateTime.MinValue;
            this._reader = reader;
        }

        public int GetInt32(string column)
        {
            try
            {
                return (!this._reader.IsDBNull(this._reader.GetOrdinal(column))) ? Convert.ToInt32(((DbDataReader)this._reader)[column]) : 0;
            }
            catch
            {
                return 0;
            }
        }

        public long GetInt64(string column)
        {
            try
            {
                return this._reader.IsDBNull(this._reader.GetOrdinal(column)) ? 0 : Convert.ToInt64(((DbDataReader)this._reader)[column]);
            }
            catch
            {
                return 0L;
            }
        }

        public short GetInt16(string column)
        {
            try
            {
                return (short)((!this._reader.IsDBNull(this._reader.GetOrdinal(column))) ? Convert.ToInt16(((DbDataReader)this._reader)[column]) : 0);
            }
            catch
            {
                return 0;
            }
        }

        public byte GetByte(string column)
        {
            try
            {
                return (byte)((!this._reader.IsDBNull(this._reader.GetOrdinal(column))) ? Convert.ToByte(((DbDataReader)this._reader)[column]) : 0);
            }
            catch
            {
                return 0;
            }
        }

        public float GetFloat(string column)
        {
            try
            {
                return this._reader.IsDBNull(this._reader.GetOrdinal(column)) ? 0f : float.Parse(((DbDataReader)this._reader)[column].ToString());
            }
            catch
            {
                return 0f;
            }
        }

        public bool GetBoolean(string column)
        {
            try
            {
                return !this._reader.IsDBNull(this._reader.GetOrdinal(column)) && (bool)((DbDataReader)this._reader)[column];
            }
            catch
            {
                return false;
            }
        }

        public string GetString(string column)
        {
            try
            {
                return this._reader.IsDBNull(this._reader.GetOrdinal(column)) ? "" : ((DbDataReader)this._reader)[column].ToString();
            }
            catch
            {
                return "";
            }
        }

        public DateTime GetDateTime(string column)
        {
            try
            {
                return this._reader.IsDBNull(this._reader.GetOrdinal(column)) ? this._defaultDate : ((DateTime)((DbDataReader)this._reader)[column]);
            }
            catch
            {
                return this._defaultDate;
            }
        }

        public decimal GetDecimal(string column)
        {
            try
            {
                return this._reader.IsDBNull(this._reader.GetOrdinal(column)) ? 0m : ((decimal)((DbDataReader)this._reader)[column]);
            }
            catch
            {
                return 0m;
            }
        }

        public bool Read()
        {
            return this._reader.Read();
        }

        public void disposeReader(SqlDataReader reader)
        {
            if (reader != null || !reader.IsClosed)
            {
                reader.Close();
                reader.Dispose();
            }
        }

        public void DisposeReader(SqlDataReader reader)
        {
            this.disposeReader(reader);
        }

        public double GetDouble(string column)
        {
            try
            {
                return this._reader.IsDBNull(this._reader.GetOrdinal(column)) ? 0.0 : ((double)((DbDataReader)this._reader)[column]);
            }
            catch
            {
                return 0.0;
            }
        }
    }
}
