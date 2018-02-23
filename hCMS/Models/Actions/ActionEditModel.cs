using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CMSLib;
using hCMS.Library;

namespace hCMS.Models.Actions
{
    public class ActionEditModel : ViewModelBase
    {
        private List<ActionStatus> _listActionStatus;
        private List<CMSLib.Actions> _listActions;
        public short ActionId { get; set; }

        [Display(Name = "Tên chức năng")]
        [Required(ErrorMessage = "Vui lòng nhập {0} (*)")]
        public string ActionName { get; set; }

        [Display(Name = "Mô tả chức năng")]
        [Required(ErrorMessage = "Vui lòng nhập {0} (*)")]
        public string ActionDesc { get; set; }

        public short ParentActionId { get; set; }

        public byte LevelId { get; set; }

        public string Url { get; set; }

        public byte ActionStatusId { get; set; }

        public short ActionOrder { get; set; }

        public bool Display { get; set; }

        public List<CMSLib.Actions> ListActions
        {
            get => !_listActions.HasValue() ? new CMSLib.Actions().GetAllHierachy2($"\xA0\xA0\xA0") : _listActions;
            set => _listActions = value;
        }

        public List<ActionStatus> ListActionStatus
        {
            get => !_listActionStatus.HasValue() ? ActionStatus.Static_GetList() : _listActionStatus;
            set => _listActionStatus = value;
        }
    }
}