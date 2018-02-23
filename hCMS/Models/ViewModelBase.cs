using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hCMS.Models
{
    public class ViewModelBase
    {
        public string AddOrders { get; set; }
        public string Delete { get; set; }

        public string Active { get; set; }

        public string DeActive { get; set; }

        public PaginationModel Pagination { get; set; }

        public string Messages { get; set; }
    }
}