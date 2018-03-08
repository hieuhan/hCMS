using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMSHelperLib;
using hCMS.Library;

namespace hCMS.Models.MediaGroups
{
    public class MediaGroupsModel : ViewModelBase
    {
        public short MediaGroupId { get; set; }
        public string MediaGroupName { get; set; }

        public string MediaGroupDesc { get; set; }

        public int CrUserId { get; set; }

        public DateTime CrDateTime { get; set; }

        public short[] MediaGroupsId { get; set; }

        public short SiteId { get; set; }

        private List<CMSLib.Sites> _listSites;
        public List<CMSLib.Sites> ListSites
        {
            get { return !_listSites.HasValue() ? CMSLib.Sites.Static_GetList(SessionHelpers.UserId) : _listSites; }
            set { _listSites = value; }
        }

        public List<CMSLib.MediaGroups> ListMediaGroups { get; set; }

        public List<CMSLib.Users> ListUsers { get; set; }
    }
}