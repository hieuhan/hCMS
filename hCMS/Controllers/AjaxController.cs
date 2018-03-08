using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSLib;
using hCMS.AppCode;
using hCMS.Library;

namespace hCMS.Controllers
{
    [CmsAuthorize]
    public class AjaxController : Controller
    {
        // GET: Ajax
        [HttpPost]
        public AjaxResult MediaSelect(int mediaId = 0)
        {
            string mediaPath = string.Empty;
            if (mediaId > 0)
            {
                var media = new Medias { MediaId = mediaId }.Get();

                if (media.MediaId > 0)
                {
                    mediaPath = media.FilePath;
                }
            }
            return new AjaxResult
            {
                StatusCode = 200,
                AllowGet = true,
                Message = mediaPath,
                Completed = !string.IsNullOrEmpty(mediaPath)
            };
        }
    }
}