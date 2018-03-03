using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using hCMS.Library;

namespace hCMS.Models.Sites
{
    public class SitesModel:ViewModelBase
    {
        public short[] SitesId { get; set; }
        public int RowCount { get; set; }

        private List<CMSLib.Users> _listUsers;

        public List<CMSLib.Users> ListUsers
        {
            get { return !_listUsers.HasValue() ? new CMSLib.Users().GetAll() : _listUsers; }
            set { _listUsers = value; }
        }

        public List<CMSLib.Sites> ListSites { get; set; }
    }
}