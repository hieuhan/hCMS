using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using hCMS.Library;

namespace hCMS.Models.Provinces
{
    public class ProvincesModel:ViewModelBase
    {
        private List<CMSLib.Countries> _listCountries;
        public short ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public string ProvinceDesc { get; set; }
        public short CountryId { get; set; }
        public short DisplayOrder { get; set; }
        public int CrUserId { get; set; }
        public DateTime CrDateTime { get; set; }

        public int RowCount { get; set; }
        public List<CMSLib.Provinces> ListProvinces { get; set; }

        public List<CMSLib.Countries> ListCountries
        {
            get => !_listCountries.HasValue() ? new CMSLib.Countries().GetList() : _listCountries; 
            set => _listCountries = value;
        }
    }
}