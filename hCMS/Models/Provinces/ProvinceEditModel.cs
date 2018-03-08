using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using hCMS.Library;

namespace hCMS.Models.Provinces
{
    public class ProvinceEditModel:ViewModelBase
    {
        public short ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public string ProvinceDesc { get; set; }
        public short CountryId { get; set; }
        public short DisplayOrder { get; set; }
        public int CrUserId { get; set; }
        public DateTime CrDateTime { get; set; }

        private List<CMSLib.Countries> _listCountries;
        public List<CMSLib.Countries> ListCountries
        {
            get => !_listCountries.HasValue() ? new CMSLib.Countries().GetList() : _listCountries;
            set => _listCountries = value;
        }
    }
}