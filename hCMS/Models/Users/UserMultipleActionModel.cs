using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hCMS.Models.Users
{
    public class UserMultipleActionModel:ViewModelBase
    {
        public int[] UserIds { get; set; }
    }
}