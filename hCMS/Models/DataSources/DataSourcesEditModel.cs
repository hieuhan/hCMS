using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMSLib;
using hCMS.Library;

namespace hCMS.Models.DataSources
{
    public class DataSourcesEditModel:ViewModelBase
    {
        private List<DataTypes> _listDataTypes;
        public short DataSourceId { get; set; }
        public byte DataTypeId { get; set; }
        public string DataSourceName { get; set; }
        public string DataSourceDesc { get; set; }
        public short DisplayOrder { get; set; }
        public int CrUserId { get; set; }
        public DateTime CrDateTime { get; set; }

        public List<DataTypes> ListDataTypes
        {
            get => !_listDataTypes.HasValue() ? DataTypes.Static_GetList() : _listDataTypes;
            set => _listDataTypes = value;
        }
    }
}