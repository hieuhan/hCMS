using System;

namespace hCMS.Models
{
    public class PaginationModel
    {
        public int TotalPage { get; set; }

        public int PageSize { get; set; }

        public int LinkLimit { get; set; }

        public int PageIndex { get; set; }

        public string UrlPaging { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int PageCount => (int)Math.Ceiling(TotalPage / (double)PageSize);
    }
}