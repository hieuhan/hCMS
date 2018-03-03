using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace hCMS.Models.Sites
{
    public class SitesEditModel:ViewModelBase
    {
        public short SiteId { get; set; }

        [Display(Name = "Tên website")]
        [Required(ErrorMessage = "Vui lòng nhập {0} (*)")]
        public string SiteName { get; set; }

        [Display(Name = "Mô tả")]
        [Required(ErrorMessage = "Vui lòng nhập {0} (*)")]
        public string SiteDesc { get; set; }

        public string MetaTitle { get; set; }

        public string MetaDesc { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaTagAll { get; set; }
        public string CanonicalTag { get; set; }
        public string H1Tag { get; set; }
        public short DisplayOrder { get; set; }
        public byte BuildIn { get; set; }
        public int CrUserId { get; set; }
        public DateTime CrDateTime { get; set; }
    }
}