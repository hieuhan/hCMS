using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMSLib;
using hCMS.Library;

namespace hCMS.Models.Users
{
    public class UserRolesModel:ViewModelBase
    {
        public short[] RolesId { get; set; }
        public int UserId { get; set; }
        public List<UserRoles> ListUserRoles { get; set; }

        private List<CMSLib.Roles> _listRoles;
        public List<CMSLib.Roles> ListRoles
        {
            get { return !_listRoles.HasValue() ? new CMSLib.Roles().GetAll() : _listRoles; }
            set { _listRoles = value; }
        }
    }
}