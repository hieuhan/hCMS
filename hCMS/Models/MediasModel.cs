using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hCMS.Models
{
    public class MediasModel:ViewModelBase
    {
        public int MediaId { get; set; }

        public byte MediaTypeId { get; set; }

        public short MediaGroupId { get; set; }

        public string MediaName { get; set; }

        public string MediaDesc { get; set; }

        public string FilePath { get; set; }

        public int FileSize { get; set; }

        public int CrUserId { get; set; }

        public DateTime CrDateTime { get; set; }

        public int RowCount { get; set; }

        public int[] MediasId { get; set; }

        public List<HttpPostedFileBase> FileMedias { get; set; }

        public HttpPostedFileBase FileMedia { get; set; }

        public List<CMSLib.Medias> ListMedias { get; set; }

        public List<CMSLib.MediaGroups> ListMediaGroups { get; set; }

        public List<CMSLib.MediaTypes> ListDataTypes { get; set; }

        public List<CMSLib.Users> ListUsers { get; set; }
    }
}