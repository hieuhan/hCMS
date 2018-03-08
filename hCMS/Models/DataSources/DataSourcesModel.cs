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
        private List<OrderByClauses> _listOrderByClauses;
        private List<CMSLib.Users> _listUsers;

        public List<DataTypes> ListDataTypes
        {
            get => !_listDataTypes.HasValue() ? new DataTypes().GetList() : _listDataTypes;
            set => _listDataTypes = value;
        }

        public List<OrderByClauses> ListOrderByClauses
        {
            get => !_listOrderByClauses.HasValue() ? new OrderByClauses().GetList("DataSources") : _listOrderByClauses;
            set => _listOrderByClauses = value;
        }

        public List<CMSLib.Users> ListUsers
        {
            get => !_listUsers.HasValue() ? new CMSLib.Users().GetAll() : _listUsers;
            set => _listUsers = value;
        }

        public string DataSourceName { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }

        public int RowCount { get; set; }
        public byte DataTypeId { get; set; }
        public string OrderBy { get; set; }
    }
}