using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMSLib;
using hCMS.Library;

namespace hCMS.Models.Countries
{
    public class CountriesModel:ViewModelBase
    {
        public List<CMSLib.Countries> ListCountries { get; set; }
        public short CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryDesc { get; set; }
        public string IconPath { get; set; }
        public  short DisplayOrder { get; set; }
        public int CrUserId { get; set; }
        public DateTime CrDateTime { get; set; }

        public int RowCount { get; set; }
    }
}