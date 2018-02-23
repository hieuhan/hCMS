using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace CMSHelperLib
{
    public static class ImageHelpers
    {


        /// <summary>  
        /// Save image as jpeg  
        /// </summary>  
        /// <param name="path">path where to save</param>  
        /// <param name="img">image to save</param>  
        public static void SaveJpeg(string path, Image img)
        {
            var qualityParam = new EncoderParameter(Encoder.Quality, 100L);
            var jpegCodec = GetEncoderInfo("image/jpeg");

            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            img.Save(path, jpegCodec, encoderParams);
        }

        /// <summary>  
        /// Save image  
        /// </summary>  
        /// <param name="path">path where to save</param>  
        /// <param name="img">image to save</param>  
        /// <param name="imageCodecInfo">codec info</param>  
        public static void Save(string path, Image img, ImageCodecInfo imageCodecInfo)
        {
            var qualityParam = new EncoderParameter(Encoder.Quality, 100L);

            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            img.Save(path, imageCodecInfo, encoderParams);
        }

        /// <summary>  
        /// get codec info by mime type  
        /// </summary>  
        /// <param name="mimeType"></param>  
        /// <returns></returns>  
        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            return ImageCodecInfo.GetImageEncoders().FirstOrDefault(t => t.MimeType == mimeType);
        }

        /// <summary>  
        /// the image remains the same size, and it is placed in the middle of the new canvas  
        /// </summary>  
        /// <param name="image">image to put on canvas</param>  
        /// <param name="width">canvas width</param>  
        /// <param name="height">canvas height</param>  
        /// <param name="canvasColor">canvas color</param>  
        /// <returns></returns>  
        public static Image PutOnCanvas(Image image, int width, int height, Color canvasColor)
        {
            var res = new Bitmap(width, height);
            using (var g = Graphics.FromImage(res))
            {
                g.Clear(canvasColor);
                var x = (width - image.Width) / 2;
                var y = (height - image.Height) / 2;
                g.DrawImageUnscaled(image, x, y, image.Width, image.Height);
            }

            return res;
        }

        /// <summary>  
        /// the image remains the same size, and it is placed in the middle of the new canvas  
        /// </summary>  
        /// <param name="image">image to put on canvas</param>  
        /// <param name="width">canvas width</param>  
        /// <param name="height">canvas height</param>  
        /// <returns></returns>  
        public static Image PutOnWhiteCanvas(Image image, int width, int height)
        {
            return PutOnCanvas(image, width, height, Color.White);
        }

        /// <summary>  
        /// resize an image and maintain aspect ratio  
        /// </summary>  
        /// <param name="image">image to resize</param>  
        /// <param name="newWidth">desired width</param>  
        /// <param name="maxHeight">max height</param>  
        /// <param name="onlyResizeIfWider">if image width is smaller than newWidth use image width</param>  
        /// <returns>resized image</returns>  
        public static Image Resize(Image image, int newWidth, int maxHeight, bool onlyResizeIfWider)
        {
            if (onlyResizeIfWider && image.Width <= newWidth) newWidth = image.Width;

            var newHeight = image.Height * newWidth / image.Width;
            if (newHeight > maxHeight)
            {
                // Resize with height instead  
                newWidth = image.Width * maxHeight / image.Height;
                newHeight = maxHeight;
            }

            var res = new Bitmap(newWidth, newHeight);

            using (var graphic = Graphics.FromImage(res))
            {
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return res;
        }

        /// <summary>  
        /// Crop an image   
        /// </summary>  
        /// <param name="img">image to crop</param>  
        /// <param name="cropArea">rectangle to crop</param>  
        /// <returns>resulting image</returns>  
        public static Image Crop(Image img, Rectangle cropArea)
        {
            var bmpImage = new Bitmap(img);
            var bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return bmpCrop;
        }

        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        //The actual converting function  
        public static string GetImage(object img)
        {
            return "data:image/jpg;base64," + Convert.ToBase64String((byte[])img);
        }


        public static void PerformImageResizeAndPutOnCanvas(string filePath, string filePathResize, int pWidth, int pHeight)
        {
            System.Drawing.Image imgBef;
            imgBef = System.Drawing.Image.FromFile(filePath);


            System.Drawing.Image _imgR;
            _imgR = Resize(imgBef, pWidth, pHeight, true);


            System.Drawing.Image _img2;
            _img2 = PutOnCanvas(_imgR, pWidth, pHeight, System.Drawing.Color.White);

            //Save JPEG  
            SaveJpeg(filePathResize, _img2);

        }
    }

    public interface IImageInfo
    {
        string Path { get; set; }
        string ContentType { get; set; }
        string FileName { get; set; }
        int FileSize { get; set; }
        byte[] PhotoStream { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        void Save();
        void Save(string newPath);
        void Save(string newPath, string newFilename);
        IImageInfo ResizeMe(int? maxHeight, int? maxWidth);
    }

    public class ImageInfo : IImageInfo
    {
        private string path;
        private string contentType;
        private string fileName;
        private int fileSize;
        private byte[] photoStream;
        private int width;
        private int height;

        public string Path
        {
            get { return this.path; }
            set { this.path = value; }
        }

        public string ContentType
        {
            get { return this.contentType; }
            set { this.contentType = value; }
        }

        public string FileName
        {
            get { return this.fileName; }
            set { this.fileName = value; }
        }

        public int FileSize
        {
            get { return this.fileSize; }
            set { this.fileSize = value; }
        }

        public byte[] PhotoStream
        {
            get { return this.photoStream; }
            set { this.photoStream = value; }
        }

        public int Width
        {
            get { return this.width; }
            set { this.width = value; }
        }

        public int Height
        {
            get { return this.height; }
            set { this.height = value; }
        }

        public void Save(string newPath)
        {
            this.path = newPath;
            Save();
        }

        public void Save(string newPath, string newFileName)
        {
            this.fileName = newFileName;
            this.path = newPath;
            Save();
        }

        public void Save()
        {
            FileManager fMgr = new FileManager();
            fMgr.SaveImage(this);
        }

        public IImageInfo ResizeMe(int? maxHeight, int? maxWidth)
        {
            FileManager fMgr = new FileManager();
            return fMgr.GetResizedImage(this, maxHeight, maxWidth);
        }
    }

    public abstract class ImageManagerBase
    {
        public static System.Drawing.Image GetImageFromStream(byte[] stream)
        {
            return System.Drawing.Image.FromStream(new MemoryStream(stream));
        }

        public static byte[] GetImageByteArray(Stream stream, int contentLength)
        {
            byte[] buffer = new byte[contentLength];
            stream.Read(buffer, 0, contentLength);
            return buffer;
        }

        public static IImageInfo GetImageInfo(byte[] stream)
        {
            IImageInfo info = new ImageInfo();
            info.FileSize = stream.Length;
            info.PhotoStream = stream;
            Image img = GetImageFromStream(stream);
            info.Width = img.Size.Width;
            info.Height = img.Size.Height;
            return info;
        }

        public static IImageInfo GetImageInfo(Stream stream, int contentLength)
        {
            IImageInfo info = new ImageInfo();
            byte[] imgBuffer = GetImageByteArray(stream, contentLength);
            info.FileSize = imgBuffer.Length;
            info.PhotoStream = imgBuffer;
            Image img = GetImageFromStream(imgBuffer);
            info.Width = img.Size.Width;
            info.Height = img.Size.Height;
            return info;
        }

        public IImageInfo GetResizedImage(IImageInfo image, int? maxHeight, int? maxWidth)
        {
            if ((!maxHeight.HasValue && !maxWidth.HasValue))
                throw new ArgumentOutOfRangeException("maxHeight", "You must provide a non-zero maxHeight or maxWidth");

            byte[] resizedStream = GetResizedImageStream(image.PhotoStream, maxHeight, maxWidth, image.ContentType);
            Image newImg = GetImageFromStream(resizedStream);
            IImageInfo info = new ImageInfo();
            info.ContentType = image.ContentType;
            info.FileName = image.FileName;
            info.FileSize = resizedStream.Length;
            info.PhotoStream = resizedStream;
            info.Width = newImg.Size.Width;
            info.Height = newImg.Size.Height;
            return info;
        }

        public static string GetImageSize(IImageInfo image)
        {
            string retVal = "Unknown";
            FileInfo fi = new FileInfo(GetPath(image.FileName, image.Path));
            if (fi.Exists)
            {
                retVal = string.Format("{0} Kb", ((int)(fi.Length / 1000)).ToString());
            }
            return retVal;
        }

        public byte[] GetResizedImageStream(byte[] stream, int? maxHeight, int? maxWidth, string contentType)
        {
            byte[] buffer = stream;
            Image img = GetImageFromStream(stream);

            int width = img.Size.Width;
            int height = img.Size.Height;
            int mWidth = (maxWidth.HasValue) ? maxWidth.Value : 0;
            int mHeight = (maxHeight.HasValue) ? maxHeight.Value : 0;

            bool doWidthResize = (mWidth > 0 && width > mWidth && width > mHeight);
            bool doHeightResize = (mHeight > 0 && height > mHeight && height > mWidth);

            //only resize if the image is bigger than the max
            if (doWidthResize || doHeightResize)
            {
                int iStart;
                Decimal divider;
                if (doWidthResize)
                {
                    iStart = width;
                    divider = Math.Abs((Decimal)iStart / (Decimal)mWidth);
                    width = mWidth;
                    height = (int)Math.Round((Decimal)(height / divider));
                }
                else
                {
                    iStart = height;
                    divider = Math.Abs((Decimal)iStart / (Decimal)mHeight);
                    height = mHeight;
                    width = (int)Math.Round((Decimal)(width / divider));
                }

                Image newImg = img.GetThumbnailImage(width, height, null, new System.IntPtr());
                using (MemoryStream ms = new MemoryStream())
                {
                    if (contentType.IndexOf("jpeg") > -1)
                        newImg.Save(ms, ImageFormat.Jpeg);
                    else if (contentType.IndexOf("png") > -1)
                        newImg.Save(ms, ImageFormat.Png);
                    else
                        newImg.Save(ms, ImageFormat.Gif);

                    buffer = ms.ToArray();
                }
            }

            return buffer;
        }

        public void SaveImage(IImageInfo image)
        {
            //save file to file system
            string path = GetPath(image.FileName, image.Path);
            System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Create);
            fs.Write(image.PhotoStream, 0, image.FileSize);

            SaveImageFile(fs, path);
            fs.Dispose();
            fs.Close();
        }

        internal static string GetPath(string fileName, string path)
        {
            string directory = (path != null) ? string.Format("{0}/", path) : "";
            return HttpContext.Current.Server.MapPath(string.Format("{0}{1}", directory, fileName));
        }

        internal void SaveImageFile(System.IO.FileStream fs, string path)
        {
            Bitmap bmp = new System.Drawing.Bitmap(fs);
            if (Path.GetExtension(path).Equals(".gif"))
                bmp.Save(fs, ImageFormat.Gif);
            else if (Path.GetExtension(path).Equals(".png"))
                bmp.Save(fs, ImageFormat.Png);
            else
                bmp.Save(fs, ImageFormat.Jpeg);

            bmp.Dispose();
        }

        public static void DeleteImageFromFileSystem(string fileName, string path)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            FileInfo fi = new FileInfo(GetPath(fileName, path));
            if (fi.Exists)
                File.Delete(fi.FullName);
            else
                throw new FileNotFoundException("The image file was not found.");
        }
    }

    public partial class FileManager : ImageManagerBase
    {
        public void DeleteImageFromFileSystem(string fileName)
        {
            DeleteImageFromFileSystem(fileName, null);
        }

        public void DeleteImageFromFileSystem(string fileName, string folder)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            FileInfo fi = new FileInfo(GetPath(fileName, folder));
            if (fi.Exists)
                File.Delete(fi.FullName);
            else
                throw new FileNotFoundException("The image file was not found.");
        }


    }
}
