using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMSLib;
using hCMS.Library;

namespace hCMS.Models.Actions
{
    public class ActionRolesModel:ViewModelBase
    {
        public short ActionId { get; set; }
        public short[] RolesId { get; set; }
        public List<RoleActions> ListRoleActions { get; set; }
        private List<CMSLib.Roles> _listRoles;

        public List<CMSLib.Roles> ListRoles
        {
            get => !_listRoles.HasValue() ? new CMSLib.Roles().GetAll() : _listRoles;
            set => _listRoles = value;
        }
    }
}