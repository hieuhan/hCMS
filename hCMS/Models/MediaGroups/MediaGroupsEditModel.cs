using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMSHelperLib;
using hCMS.Library;

namespace hCMS.Models.MediaGroups
{
    public class MediaGroupsEditModel:ViewModelBase
    {
        private List<CMSLib.MediaGroups> _listMediaGroups;
        private List<CMSLib.Sites> _listSites;
        public short MediaGroupId { get; set; }
        public short SiteId { get; set; }
        public string MediaGroupName { get; set; }
        public string MediaGroupDesc { get; set; }
        public short ParentGroupId { get; set; }
        public int CrUserId { get; set; }
        public DateTime CrDateTime { get; set; }

        public List<CMSLib.MediaGroups> ListMediaGroups
        {
            get => !_listMediaGroups.HasValue() ? new CMSLib.MediaGroups().GetList() : _listMediaGroups;
            set => _listMediaGroups = value;
        }

        public List<CMSLib.Sites> ListSites
        {
            get => !_listSites.HasValue() ? new CMSLib.Sites().GetList(SessionHelpers.UserId) : _listSites; 
            set => _listSites = value;
        }
    }
}