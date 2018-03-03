using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMSLib;

namespace hCMS.Models.Users
{
    public class UserSitesModel:ViewModelBase
    {
        public int UserId { get; set; }
        public short[] SitesId { get; set; }
        public List<CMSLib.Sites> ListSites { get; set; }

        public List<UserSites> ListUserSites { get; set; }
    }
}