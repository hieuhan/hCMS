using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMSLib;
using hCMS.Library;

namespace hCMS.Models.Users
{
    public class UserActionsModel:ViewModelBase
    {
        public int UserId { get; set; }
        public short[] ActionsId { get; set; }
        private List<CMSLib.Actions> _listActions;

        public List<CMSLib.Actions> ListActions
        {
            get { return !_listActions.HasValue() ? new CMSLib.Actions().GetAllHierachy2($"\xA0\xA0\xA0") : _listActions; }
            set { _listActions = value; }
        }

        public List<UserActions> ListUserActions { get; set; }
    }
}