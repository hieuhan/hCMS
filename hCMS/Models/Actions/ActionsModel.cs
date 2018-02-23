using System.Collections.Generic;
using CMSLib;

namespace hCMS.Models.Actions
{
    public class ActionsModel : ViewModelBase
    {
        public short ActionId { get; set; }
        public short[] ActionsId { get; set; }
        public short ParentActionId { get; set; }
        public string ActionName { get; set; }
        public int RowCount { get; set; }
        public List<CMSLib.Actions> ListActions { get; set; }
        public List<CMSLib.Actions> ListParentActions { get; set; }
        public List<ActionStatus> ListActionStatus { get; set; }
        //public List<DisplayOrderModel> DisplayOrders { get; set; }
    }

}