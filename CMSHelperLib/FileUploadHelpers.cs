using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;

using System.Drawing.Drawing2D;

using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using LibUtils;

namespace CMSHelperLib
{
    public class FileUploadHelpers
    {
        public static bool IsImageFile(string FileName)
        {
            bool RetVal = false;
            string fileExt = Path.GetExtension(FileName).ToLower();
            string imageFile = ".jpg;.gif;.png;.bmp;.jpeg";
            if (imageFile.IndexOf(fileExt) >= 0) RetVal = true;
            return RetVal;
        }

        public static string GetDirByFileType(string FileName)
        {
            string RetVal = "Others";
            string fileExt = Path.GetExtension(FileName).ToLower();
            string imageFile = ".jpg;.jpeg;.gif;.png;.bmp";
            string videoFile = ".3gp;.mp4;.flv";
            string audioFile = ".mp3;.wav;.wma";
            string m3u8File = ".m3u8";

            if (imageFile.IndexOf(fileExt) >= 0) RetVal = "Images/Original";
            else if (videoFile.IndexOf(fileExt) >= 0) RetVal = "Videos";
            else if (audioFile.IndexOf(fileExt) >= 0) RetVal = "Audios";
            else if (m3u8File.IndexOf(fileExt) >= 0) RetVal = "M3u8";

            return RetVal;
        }

        public static byte GetFileTypeId(string FileName)
        {
            byte RetVal = 0;
            string fileExt = Path.GetExtension(FileName).ToLower();
            string imageFile = ".jpg;.jpeg;.gif;.png;.bmp";
            string videoFile = ".3gp;.mp4;.flv";
            string audioFile = ".mp3;.wav;.wma";

            if (imageFile.IndexOf(fileExt) >= 0) RetVal = 1;
            else if (videoFile.IndexOf(fileExt) >= 0) RetVal = 2;
            else if (audioFile.IndexOf(fileExt) >= 0) RetVal = 3;

            return RetVal;
        }

        public static string GetFileName(string FilePath)
        {
            string RetVal = "";
            if (!string.IsNullOrEmpty(FilePath))
            {
                FilePath = FilePath.Replace("\\", "/");
                if (FilePath.Contains("/"))
                {
                    int m_Pos = FilePath.LastIndexOf("/");
                    RetVal = FilePath.Substring(m_Pos + 1);
                }
                else
                {
                    RetVal = FilePath;
                }
            }
            return RetVal;
        }

        //public static string SaveFile(FileUpload fUpl, string PhysicalApplicationPath, string VirtualPath, bool CreateImageThumbnail = false)
        //{
        //    string RetVal = "";
        //    try
        //    {
        //        if (fUpl.HasFile)
        //        {
        //            VirtualPath = Path.Combine(VirtualPath, GetDirByFileType(fUpl.FileName), DateTime.Now.ToString("yyyy/MM/dd"));
        //            string FilePath = "";
        //            string fileName = "";
        //            string DirectoryPath = Path.Combine(PhysicalApplicationPath, VirtualPath);

        //            if (!Directory.Exists(DirectoryPath))
        //            {
        //                Directory.CreateDirectory(DirectoryPath);
        //            }
        //            fileName = StringUtil.RemoveSign4VietnameseString(Path.GetFileNameWithoutExtension(fUpl.FileName)).Replace(" ", "_");
        //            fileName = fileName + "_" + DateTime.Now.ToString("ddMMHHmmss");
        //            fileName = fileName + Path.GetExtension(fUpl.FileName);
        //            fileName = fileName.Replace(" ", "_");
        //            FilePath = Path.Combine(DirectoryPath, fileName);
        //            fUpl.PostedFile.SaveAs(FilePath);

        //            //Create thumbnail if image
        //            if (CreateImageThumbnail && IsImageFile(fUpl.FileName))
        //            {
        //                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MEDIA_THUMNAIL_WIDTH"]))
        //                {
        //                    int imgWidth = int.Parse(ConfigurationManager.AppSettings["MEDIA_THUMNAIL_WIDTH"].ToString());
        //                    int imgHeight = int.Parse(ConfigurationManager.AppSettings["MEDIA_THUMNAIL_HEIGHT"].ToString());
        //                    if (imgWidth > 0 && imgHeight > 0)
        //                    {
        //                        string fileThumb = FilePath.Replace("Original\\", "Thumb\\");
        //                        string dirThumb = DirectoryPath.Replace("Original\\", "Thumb\\");
        //                        if (!Directory.Exists(dirThumb))
        //                        {
        //                            Directory.CreateDirectory(dirThumb);
        //                        }
        //                        CreateThumbnail(FilePath, imgWidth, imgHeight).Save(fileThumb, ImageFormat.Jpeg);
        //                    }
        //                }
        //                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MEDIA_WIDTH"]))
        //                {
        //                    int imgWidth = int.Parse(ConfigurationManager.AppSettings["MEDIA_WIDTH"].ToString());
        //                    int imgHeight = int.Parse(ConfigurationManager.AppSettings["MEDIA_HEIGHT"].ToString());
        //                    if (imgWidth > 0 && imgHeight > 0)
        //                    {
        //                        string fileThumb = FilePath.Replace("Original\\", "Standard\\");
        //                        string dirThumb = DirectoryPath.Replace("Original\\", "Standard\\");
        //                        if (!Directory.Exists(dirThumb))
        //                        {
        //                            Directory.CreateDirectory(dirThumb);
        //                        }
        //                        CreateThumbnail(FilePath, imgWidth, imgHeight).Save(fileThumb, ImageFormat.Jpeg);
        //                    }
        //                }
        //                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MEDIA_MOBILE_WIDTH"]))
        //                {
        //                    int imgWidth = int.Parse(ConfigurationManager.AppSettings["MEDIA_MOBILE_WIDTH"].ToString());
        //                    int imgHeight = int.Parse(ConfigurationManager.AppSettings["MEDIA_MOBILE_HEIGHT"].ToString());
        //                    if (imgWidth > 0 && imgHeight > 0)
        //                    {
        //                        string fileThumb = FilePath.Replace("Original\\", "Mobile\\");
        //                        string dirThumb = DirectoryPath.Replace("Original\\", "Mobile\\");
        //                        if (!Directory.Exists(dirThumb))
        //                        {
        //                            Directory.CreateDirectory(dirThumb);
        //                        }
        //                        CreateThumbnail(FilePath, imgWidth, imgHeight).Save(fileThumb, ImageFormat.Jpeg);
        //                    }
        //                }
        //                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MEDIA_ICON_WIDTH"]))
        //                {
        //                    int imgWidth = int.Parse(ConfigurationManager.AppSettings["MEDIA_ICON_WIDTH"].ToString());
        //                    int imgHeight = int.Parse(ConfigurationManager.AppSettings["MEDIA_ICON_HEIGHT"].ToString());
        //                    if (imgWidth > 0 && imgHeight > 0)
        //                    {
        //                        string fileThumb = FilePath.Replace("Original\\", "Icon\\");
        //                        string dirThumb = DirectoryPath.Replace("Original\\", "Icon\\");
        //                        if (!Directory.Exists(dirThumb))
        //                        {
        //                            Directory.CreateDirectory(dirThumb);
        //                        }
        //                        CreateThumbnail(FilePath, imgWidth, imgHeight).Save(fileThumb, ImageFormat.Jpeg);
        //                    }
        //                }
        //            }

        //            RetVal = Path.Combine(VirtualPath, fileName);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        sms.utils.LogFiles.LogError(((new System.Diagnostics.StackTrace()).GetFrames()[0]).GetMethod().Name + "\t" + ex.ToString());
        //    }
        //    return RetVal;
        //}

        public static string SaveFile(HttpPostedFileBase fUpl, string PhysicalApplicationPath, string VirtualPath, bool CreateImageThumbnail = false)
        {
            string RetVal = "";
            try
            {
                if (fUpl.ContentLength > 0)
                {
                    VirtualPath = Path.Combine(VirtualPath, GetDirByFileType(fUpl.FileName), DateTime.Now.ToString("yyyy/MM/dd"));
                    string FilePath = "";
                    string fileName = "";
                    string DirectoryPath = Path.Combine(PhysicalApplicationPath, VirtualPath);

                    if (!Directory.Exists(DirectoryPath))
                    {
                        Directory.CreateDirectory(DirectoryPath);
                    }
                    fileName = StringUtil.RemoveSign4VietnameseString(Path.GetFileNameWithoutExtension(fUpl.FileName)).Replace(" ", "_");
                    fileName = fileName + "_" + DateTime.Now.ToString("ddMMHHmmss");
                    fileName = fileName + Path.GetExtension(fUpl.FileName);
                    fileName = fileName.Replace(" ", "_");
                    FilePath = Path.Combine(DirectoryPath, fileName);
                    fUpl.SaveAs(FilePath);

                    //Create thumbnail if image
                    if (CreateImageThumbnail && IsImageFile(fUpl.FileName))
                    {
                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MEDIA_THUMNAIL_WIDTH"]))
                        {
                            int imgWidth = int.Parse(ConfigurationManager.AppSettings["MEDIA_THUMNAIL_WIDTH"].ToString());
                            int imgHeight = int.Parse(ConfigurationManager.AppSettings["MEDIA_THUMNAIL_HEIGHT"].ToString());
                            if (imgWidth > 0 && imgHeight > 0)
                            {
                                string fileThumb = FilePath.Replace("Original\\", "Thumb\\");
                                string dirThumb = DirectoryPath.Replace("Original\\", "Thumb\\");
                                if (!Directory.Exists(dirThumb))
                                {
                                    Directory.CreateDirectory(dirThumb);
                                }
                                CreateThumbnail(FilePath, imgWidth, imgHeight).Save(fileThumb, ImageFormat.Jpeg);
                            }
                        }
                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MEDIA_WIDTH"]))
                        {
                            int imgWidth = int.Parse(ConfigurationManager.AppSettings["MEDIA_WIDTH"].ToString());
                            int imgHeight = int.Parse(ConfigurationManager.AppSettings["MEDIA_HEIGHT"].ToString());
                            if (imgWidth > 0 && imgHeight > 0)
                            {
                                string fileThumb = FilePath.Replace("Original\\", "Standard\\");
                                string dirThumb = DirectoryPath.Replace("Original\\", "Standard\\");
                                if (!Directory.Exists(dirThumb))
                                {
                                    Directory.CreateDirectory(dirThumb);
                                }
                                CreateThumbnail(FilePath, imgWidth, imgHeight).Save(fileThumb, ImageFormat.Jpeg);
                            }
                        }
                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MEDIA_MOBILE_WIDTH"]))
                        {
                            int imgWidth = int.Parse(ConfigurationManager.AppSettings["MEDIA_MOBILE_WIDTH"].ToString());
                            int imgHeight = int.Parse(ConfigurationManager.AppSettings["MEDIA_MOBILE_HEIGHT"].ToString());
                            if (imgWidth > 0 && imgHeight > 0)
                            {
                                string fileThumb = FilePath.Replace("Original\\", "Mobile\\");
                                string dirThumb = DirectoryPath.Replace("Original\\", "Mobile\\");
                                if (!Directory.Exists(dirThumb))
                                {
                                    Directory.CreateDirectory(dirThumb);
                                }
                                CreateThumbnail(FilePath, imgWidth, imgHeight).Save(fileThumb, ImageFormat.Jpeg);
                            }
                        }
                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MEDIA_ICON_WIDTH"]))
                        {
                            int imgWidth = int.Parse(ConfigurationManager.AppSettings["MEDIA_ICON_WIDTH"].ToString());
                            int imgHeight = int.Parse(ConfigurationManager.AppSettings["MEDIA_ICON_HEIGHT"].ToString());
                            if (imgWidth > 0 && imgHeight > 0)
                            {
                                string fileThumb = FilePath.Replace("Original\\", "Icon\\");
                                string dirThumb = DirectoryPath.Replace("Original\\", "Icon\\");
                                if (!Directory.Exists(dirThumb))
                                {
                                    Directory.CreateDirectory(dirThumb);
                                }
                                CreateThumbnail(FilePath, imgWidth, imgHeight).Save(fileThumb, ImageFormat.Jpeg);
                            }
                        }
                    }

                    RetVal = Path.Combine(VirtualPath, fileName);
                }
            }
            catch (Exception ex)
            {
                
            }
            return RetVal;
        }

        //public static string SaveFile(HttpPostedFile fUpl, string PhysicalApplicationPath, string VirtualPath, bool CreateImageThumbnail = false)
        //{
        //    string RetVal = "";
        //    try
        //    {
        //        if (fUpl != null)
        //        {
        //            VirtualPath = Path.Combine(VirtualPath, GetDirByFileType(fUpl.FileName), DateTime.Now.ToString("yyyy/MM/dd"));
        //            string FilePath = "";
        //            string fileName = "";
        //            string DirectoryPath = Path.Combine(PhysicalApplicationPath, VirtualPath);

        //            if (!Directory.Exists(DirectoryPath))
        //            {
        //                Directory.CreateDirectory(DirectoryPath);
        //            }
        //            fileName = StringUtil.RemoveSign4VietnameseString(Path.GetFileNameWithoutExtension(fUpl.FileName)).Replace(" ", "_");
        //            fileName = fileName + "_" + DateTime.Now.ToString("ddMMHHmmss");
        //            fileName = fileName + Path.GetExtension(fUpl.FileName);
        //            fileName = fileName.Replace(" ", "_");
        //            FilePath = Path.Combine(DirectoryPath, fileName);
        //            fUpl.SaveAs(FilePath);

        //            //Create thumbnail if image
        //            if (CreateImageThumbnail && IsImageFile(fUpl.FileName))
        //            {
        //                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MEDIA_THUMNAIL_WIDTH"]))
        //                {
        //                    int imgWidth = int.Parse(ConfigurationManager.AppSettings["MEDIA_THUMNAIL_WIDTH"].ToString());
        //                    int imgHeight = int.Parse(ConfigurationManager.AppSettings["MEDIA_THUMNAIL_HEIGHT"].ToString());
        //                    if (imgWidth > 0 && imgHeight > 0)
        //                    {
        //                        string fileThumb = FilePath.Replace("Original\\", "Thumb\\");
        //                        string dirThumb = DirectoryPath.Replace("Original\\", "Thumb\\");
        //                        if (!Directory.Exists(dirThumb))
        //                        {
        //                            Directory.CreateDirectory(dirThumb);
        //                        }
        //                        CreateThumbnail(FilePath, imgWidth, imgHeight).Save(fileThumb, ImageFormat.Jpeg);
        //                    }
        //                }
        //                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MEDIA_WIDTH"]))
        //                {
        //                    int imgWidth = int.Parse(ConfigurationManager.AppSettings["MEDIA_WIDTH"].ToString());
        //                    int imgHeight = int.Parse(ConfigurationManager.AppSettings["MEDIA_HEIGHT"].ToString());
        //                    if (imgWidth > 0 && imgHeight > 0)
        //                    {
        //                        string fileThumb = FilePath.Replace("Original\\", "Standard\\");
        //                        string dirThumb = DirectoryPath.Replace("Original\\", "Standard\\");
        //                        if (!Directory.Exists(dirThumb))
        //                        {
        //                            Directory.CreateDirectory(dirThumb);
        //                        }
        //                        CreateThumbnail(FilePath, imgWidth, imgHeight).Save(fileThumb, ImageFormat.Jpeg);
        //                    }
        //                }
        //                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MEDIA_MOBILE_WIDTH"]))
        //                {
        //                    int imgWidth = int.Parse(ConfigurationManager.AppSettings["MEDIA_MOBILE_WIDTH"].ToString());
        //                    int imgHeight = int.Parse(ConfigurationManager.AppSettings["MEDIA_MOBILE_HEIGHT"].ToString());
        //                    if (imgWidth > 0 && imgHeight > 0)
        //                    {
        //                        string fileThumb = FilePath.Replace("Original\\", "Mobile\\");
        //                        string dirThumb = DirectoryPath.Replace("Original\\", "Mobile\\");
        //                        if (!Directory.Exists(dirThumb))
        //                        {
        //                            Directory.CreateDirectory(dirThumb);
        //                        }
        //                        CreateThumbnail(FilePath, imgWidth, imgHeight).Save(fileThumb, ImageFormat.Jpeg);
        //                    }
        //                }
        //                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MEDIA_ICON_WIDTH"]))
        //                {
        //                    int imgWidth = int.Parse(ConfigurationManager.AppSettings["MEDIA_ICON_WIDTH"].ToString());
        //                    int imgHeight = int.Parse(ConfigurationManager.AppSettings["MEDIA_ICON_HEIGHT"].ToString());
        //                    if (imgWidth > 0 && imgHeight > 0)
        //                    {
        //                        string fileThumb = FilePath.Replace("Original\\", "Icon\\");
        //                        string dirThumb = DirectoryPath.Replace("Original\\", "Icon\\");
        //                        if (!Directory.Exists(dirThumb))
        //                        {
        //                            Directory.CreateDirectory(dirThumb);
        //                        }
        //                        CreateThumbnail(FilePath, imgWidth, imgHeight).Save(fileThumb, ImageFormat.Jpeg);
        //                    }
        //                }
        //            }

        //            RetVal = Path.Combine(VirtualPath, fileName);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        sms.utils.LogFiles.LogError(((new System.Diagnostics.StackTrace()).GetFrames()[0]).GetMethod().Name + "\t" + ex.ToString());
        //    }
        //    return RetVal;
        //}

        public static string SaveFile(byte[] buffer, string fileUploadName, string virtualPath, string id, string directory = "Lawyers", bool createImageThumbnail = false)
        {
            string retVal = "";
            try
            {
                if (buffer != null && buffer.Length > 0)
                {
                    string physicalApplicationPath = HostingEnvironment.ApplicationPhysicalPath;
                    virtualPath = Path.Combine(virtualPath, directory, id, DateTime.Now.ToString("yyyy/MM/dd"));
                    string directoryPath = Path.Combine(physicalApplicationPath, virtualPath);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    string fileName = "";//StringUtil.RemoveSign4VietnameseString(Path.GetFileNameWithoutExtension(fileUploadName)).Replace(" ", "_");
                    fileName = fileName + "_" + DateTime.Now.ToString("ddMMHHmmss");
                    fileName = fileName + Path.GetExtension(fileUploadName);
                    fileName = fileName.Replace(" ", "_");
                    string filePath = Path.Combine(directoryPath, fileName);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (FileStream fileStream = new FileStream
                            (filePath, FileMode.Create))
                        {
                            memoryStream.WriteTo(fileStream);
                        }
                    }

                    //Create thumbnail if image
                    if (createImageThumbnail && IsImageFile(fileUploadName))
                    {
                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MEDIA_THUMNAIL_WIDTH"]))
                        {
                            int imgWidth = int.Parse(ConfigurationManager.AppSettings["MEDIA_THUMNAIL_WIDTH"].ToString());
                            int imgHeight = int.Parse(ConfigurationManager.AppSettings["MEDIA_THUMNAIL_HEIGHT"].ToString());
                            if (imgWidth > 0 && imgHeight > 0)
                            {
                                string fileThumb = filePath.Replace("Original\\", "Thumb\\");
                                string dirThumb = directoryPath.Replace("Original\\", "Thumb\\");
                                if (!Directory.Exists(dirThumb))
                                {
                                    Directory.CreateDirectory(dirThumb);
                                }
                                CreateThumbnail(filePath, imgWidth, imgHeight).Save(fileThumb, ImageFormat.Jpeg);
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
                                CreateThumbnail(filePath, imgWidth, imgHeight).Save(fileThumb, ImageFormat.Jpeg);
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
                                CreateThumbnail(filePath, imgWidth, imgHeight).Save(fileThumb, ImageFormat.Jpeg);
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
                                CreateThumbnail(filePath, imgWidth, imgHeight).Save(fileThumb, ImageFormat.Jpeg);
                            }
                        }
                    }

                    retVal = Path.Combine(virtualPath, fileName);
                }
            }
            catch (Exception ex)
            {
                //LogFiles.LogError(((new System.Diagnostics.StackTrace()).GetFrames()[0]).GetMethod().Name + "\t" + ex.ToString());
            }
            return retVal;
        }


        public static Bitmap CreateThumbnail(string lcFilename, int lnWidth, int lnHeight)
        {
            System.Drawing.Bitmap bmpOut = null;
            try
            {
                Bitmap loBMP = new Bitmap(lcFilename);
                ImageFormat loFormat = loBMP.RawFormat;

                decimal lnRatio;
                int lnNewWidth = 0;
                int lnNewHeight = 0;

                //*** If the image is smaller than a thumbnail just return it
                if (loBMP.Width < lnWidth && loBMP.Height < lnHeight)
                    return loBMP;

                if (loBMP.Width > loBMP.Height)
                {
                    lnRatio = (decimal)lnWidth / loBMP.Width;
                    lnNewWidth = lnWidth;
                    decimal lnTemp = loBMP.Height * lnRatio;
                    lnNewHeight = (int)lnTemp;
                }
                else
                {
                    lnRatio = (decimal)lnHeight / loBMP.Height;
                    lnNewHeight = lnHeight;
                    decimal lnTemp = loBMP.Width * lnRatio;
                    lnNewWidth = (int)lnTemp;
                }
                bmpOut = new Bitmap(lnNewWidth, lnNewHeight);
                Graphics g = Graphics.FromImage(bmpOut);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
                g.DrawImage(loBMP, 0, 0, lnNewWidth, lnNewHeight);

                loBMP.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bmpOut;
        }

    }
}
