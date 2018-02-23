using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;

using System.Drawing.Drawing2D;

using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace CMSHelperLib
{
    public class FileUploadHelpers
    {
        private static readonly HashSet<char> InvalidFileNameChars = new HashSet<char>(Path.GetInvalidFileNameChars());
        private static readonly HashSet<string> ValidImageExtensions = new HashSet<string>()
        {
            "png",
            "jpg",
            "jpeg",
            "gif",
            "bmp"
        };

        public static string GetDirByFileType(string fileName)
        {
            string retVal = "Others";
            if (!string.IsNullOrEmpty(fileName))
            {
                string fileExt = Path.GetExtension(fileName).ToLower();
                string imageFile = ".jpg;.jpeg;.gif;.png;.bmp";
                string videoFile = ".3gp;.mp4;.flv";
                string audioFile = ".mp3;.wav;.wma";
                string m3u8File = ".m3u8";

                if (imageFile.IndexOf(fileExt, StringComparison.Ordinal) >= 0) retVal = "Images/Original";
                else if (videoFile.IndexOf(fileExt, StringComparison.Ordinal) >= 0) retVal = "Videos";
                else if (audioFile.IndexOf(fileExt, StringComparison.Ordinal) >= 0) retVal = "Audios";
                else if (m3u8File.IndexOf(fileExt, StringComparison.Ordinal) >= 0) retVal = "M3u8";
            }
            return retVal;
        }

        public static string GetFileName(string filePath)
        {
            string retVal = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    retVal = Path.GetFileNameWithoutExtension(filePath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        public static string Save(HttpPostedFileBase file, string physicalApplicationPath, string virtualPath, bool createImageThumbnail = false)
        {
            string retVal = string.Empty, filePath = string.Empty, fileName = string.Empty, directoryPath = string.Empty;
            try
            {
                if (file.ContentLength > 0)
                {
                    fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    if (fileName != null)
                    {
                        virtualPath = Path.Combine(virtualPath, GetDirByFileType(file.FileName), DateTime.Now.ToString("yyyy/MM/dd"));
                        directoryPath = Path.Combine(physicalApplicationPath, virtualPath);

                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        fileName = RemoveInvalidFileNameChars(fileName);
                        fileName = fileName + "_" + DateTime.Now.ToString("ddMMHHmmss");
                        fileName = fileName + Path.GetExtension(file.FileName);
                        fileName = fileName.Replace(" ", "_");

                        filePath = Path.Combine(directoryPath, fileName);
                        file.SaveAs(filePath);

                        if (createImageThumbnail)
                        {
                            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MEDIA_THUMNAIL_WIDTH"]))
                            {
                                int imgWidth = int.Parse(ConfigurationManager.AppSettings["MEDIA_THUMNAIL_WIDTH"]);
                                int imgHeight = int.Parse(ConfigurationManager.AppSettings["MEDIA_THUMNAIL_HEIGHT"]);
                                if (imgWidth > 0 && imgHeight > 0)
                                {
                                    string fileThumb = filePath.Replace("Original\\", "Thumb\\");
                                    string dirThumb = directoryPath.Replace("Original\\", "Thumb\\");
                                    if (!Directory.Exists(dirThumb))
                                    {
                                        Directory.CreateDirectory(dirThumb);
                                    }

                                    ImageHelpers.PerformImageResizeAndPutOnCanvas(filePath, fileThumb, imgWidth, imgHeight);
                                }
                            }
                            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MEDIA_WIDTH"]))
                            {
                                int imgWidth = int.Parse(ConfigurationManager.AppSettings["MEDIA_WIDTH"].ToString());
                                int imgHeight = int.Parse(ConfigurationManager.AppSettings["MEDIA_HEIGHT"].ToString());
                                if (imgWidth > 0 && imgHeight > 0)
                                {
                                    string fileThumb = filePath.Replace("Original\\", "Standard\\");
                                    string dirThumb = directoryPath.Replace("Original\\", "Standard\\");
                                    if (!Directory.Exists(dirThumb))
                                    {
                                        Directory.CreateDirectory(dirThumb);
                                    }
                                    ImageHelpers.PerformImageResizeAndPutOnCanvas(filePath, fileThumb, imgWidth, imgHeight);
                                }
                            }
                            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MEDIA_MOBILE_WIDTH"]))
                            {
                                int imgWidth = int.Parse(ConfigurationManager.AppSettings["MEDIA_MOBILE_WIDTH"].ToString());
                                int imgHeight = int.Parse(ConfigurationManager.AppSettings["MEDIA_MOBILE_HEIGHT"].ToString());
                                if (imgWidth > 0 && imgHeight > 0)
                                {
                                    string fileThumb = filePath.Replace("Original\\", "Mobile\\");
                                    string dirThumb = directoryPath.Replace("Original\\", "Mobile\\");
                                    if (!Directory.Exists(dirThumb))
                                    {
                                        Directory.CreateDirectory(dirThumb);
                                    }
                                    ImageHelpers.PerformImageResizeAndPutOnCanvas(filePath, fileThumb, imgWidth, imgHeight);
                                }
                            }
                            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MEDIA_ICON_WIDTH"]))
                            {
                                int imgWidth = int.Parse(ConfigurationManager.AppSettings["MEDIA_ICON_WIDTH"].ToString());
                                int imgHeight = int.Parse(ConfigurationManager.AppSettings["MEDIA_ICON_HEIGHT"].ToString());
                                if (imgWidth > 0 && imgHeight > 0)
                                {
                                    string fileThumb = filePath.Replace("Original\\", "Icon\\");
                                    string dirThumb = directoryPath.Replace("Original\\", "Icon\\");
                                    if (!Directory.Exists(dirThumb))
                                    {
                                        Directory.CreateDirectory(dirThumb);
                                    }
                                    ImageHelpers.PerformImageResizeAndPutOnCanvas(filePath, fileThumb, imgWidth, imgHeight);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retVal;
        }

        protected void ResizeImage(string filePath, int width, int height)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(filePath);
            Bitmap bitMAP1 = new Bitmap(width, height);
            Graphics imgGraph = Graphics.FromImage(bitMAP1);

            //bitMAP1. = CompositingQuality.HighQuality;

            imgGraph.SmoothingMode = SmoothingMode.HighQuality;

            imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            var imgDimesions = new Rectangle(0, 0, width, height);

            imgGraph.DrawImage(image, imgDimesions);
            filePath = filePath.Replace("Original\\", "Thumb\\");
            bitMAP1.Save(filePath, ImageFormat.Jpeg);

            imgGraph.Dispose();

            imgGraph.Dispose();

            image.Dispose();
        }

        public static string RemoveInvalidFileNameChars(string name)
        {
            if (!name.Any(c => InvalidFileNameChars.Contains(c)))
            {
                return name;
            }
            return new string(name.Where(c => !InvalidFileNameChars.Contains(c)).ToArray());
        }

        private static string ValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);
            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
        }

        private static bool IsImageExtension(string ext)
        {
            var a = ValidImageExtensions.Contains(ext);
            return a;
        }

    }
}
