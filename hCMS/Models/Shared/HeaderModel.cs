using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMSLib;

namespace hCMS.Models.Shared
{
    public class HeaderModel
    {
        public List<CMSLib.Actions> ListActionLevel1 { get; set; }
        public List<CMSLib.Actions> ListActionForUser { get; set; }
    }
}