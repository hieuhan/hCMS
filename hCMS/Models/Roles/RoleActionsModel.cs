using System.Collections.Generic;
using hCMS.Library;

namespace hCMS.Models.Roles
{
    public class RoleActionsModel : ViewModelBase
    {
        public short RoleId { get; set; }

        public short[] ActionsId { get; set; }

        public short[] RolesId { get; set; }

        private List<CMSLib.Actions> _listActions;
        public List<CMSLib.Actions> ListActions
        {
            get => !_listActions.HasValue() ? new CMSLib.Actions().GetAllHierachy2($"\xA0\xA0\xA0") : _listActions;
            set => _listActions = value;
        }

        public List<CMSLib.Actions> ListActionsByRole { get; set; }
    }
}