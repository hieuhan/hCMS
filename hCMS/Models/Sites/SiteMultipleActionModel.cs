using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hCMS.Models.Sites
{
    public class SiteMultipleActionModel:ViewModelBase
    {
        public short[] SiteIds { get; set; }
    }
}