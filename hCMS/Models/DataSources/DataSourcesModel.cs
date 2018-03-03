using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMSLib;
using hCMS.Library;

namespace hCMS.Models.DataSources
{
    public class DataSourcesModel:ViewModelBase
    {
        public List<CMSLib.DataSources> ListDataSources
        {
            get => _listDataSources;
            set => _listDataSources = value;
        }

        private List<DataTypes> _listDataTypes;
        private List<CMSLib.DataSources> _listDataSources;

        public List<DataTypes> ListDataTypes
        {
            get { return !_listDataTypes.HasValue() ? new DataTypes().GetList() : _listDataTypes; }
            set { _listDataTypes = value; }
        }
        public int RowCount { get; set; }
        public byte DataTypeId { get; set; }
    }
}