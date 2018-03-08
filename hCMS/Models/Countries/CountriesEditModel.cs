using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using hCMS.Library;

namespace hCMS.Models.Countries
{
    public class CountriesEditModel:ViewModelBase
    {
        public short CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryDesc { get; set; }
        public string ImagePath { get; set; }
        public short DisplayOrder { get; set; }
        public int CrUserId { get; set; }
        public DateTime CrDateTime { get; set; }
    }
}