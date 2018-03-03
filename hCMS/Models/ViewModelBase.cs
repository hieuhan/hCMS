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

        public SystemStatus SystemStatus { get; set; }

        public string SystemMessages { get; set; }
    }

    public enum SystemStatus
    {
        Success,
        Error,
        Unknow
    }
}