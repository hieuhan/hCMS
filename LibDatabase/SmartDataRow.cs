using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibDatabase
{
    public sealed class SmartDataRow
    {
        private DateTime _defaultDate;

        private DataRow _row;

        public SmartDataRow()
        {
        }

        public SmartDataRow(DataRow row)
        {
            this._defaultDate = DateTime.MinValue;
            this._row = row;
        }

        public int GetInt32(string column)
        {
            try
            {
                return (int)this._row[column];
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
                return (long)this._row[column];
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
                return (short)this._row[column];
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
                return float.Parse(this._row[column].ToString());
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
                return (bool)this._row[column];
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
                return this._row[column].ToString();
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
                return (DateTime)this._row[column];
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
                return (decimal)this._row[column];
            }
            catch
            {
                return 0m;
            }
        }
    }
}
