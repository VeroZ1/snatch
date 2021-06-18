using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Snatch
{
    internal class ImageUtils
    {
        /// <summary>
        /// Convert Base64 to Image
        /// </summary>
        /// <param name="base64str">Base64 string</param>
        /// <returns>Bitmap</returns>
        public static Bitmap GetImageFromBase64(string base64str)
        {
            try
            {
                byte[] arr = Convert.FromBase64String(base64str);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bitmap = new Bitmap(ms);
                return bitmap;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Convert Base64 to Image
        /// </summary>
        /// <param name="string">fullPath</param>
        /// <returns>Bitmap</returns>
        public static Bitmap GetImageFromFile(string fullPath)
        {
            try
            {
                Bitmap bitmap = new Bitmap(fullPath);
                return bitmap;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Convert Image to Base64
        /// </summary>
        /// <param name="image">Image</param>
        /// <returns>Base64 string</returns>
        public static string GetBase64FromImage(Image image)
        {
            string base64str = "";
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Bitmap bmp = new Bitmap(image);
                    bmp.Save(ms, GetImageFormat(bmp));
                    byte[] arr = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(arr, 0, (int)ms.Length);
                    ms.Close();

                    base64str = Convert.ToBase64String(arr);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return base64str;
        }

        /// <summary>
        /// Convert Image to Base64
        /// </summary>
        /// <param name="string">fullPath</param>
        /// <returns>Base64 string</returns>
        public static string GetBase64FromImage(string fullPath)
        {
            string base64str = "";
            try
            {
                Bitmap bmp = new Bitmap(fullPath);
                using (MemoryStream ms = new MemoryStream())
                {
                    bmp.Save(ms, GetImageFormat(bmp));
                    byte[] arr = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(arr, 0, (int)ms.Length);
                    ms.Close();

                    base64str = Convert.ToBase64String(arr);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return base64str;
        }

        public static ImageFormat GetImageFormat(Image img)
        {
            if (img.RawFormat.Equals(ImageFormat.Jpeg))
                return ImageFormat.Jpeg;
            if (img.RawFormat.Equals(ImageFormat.Bmp))
                return ImageFormat.Bmp;
            if (img.RawFormat.Equals(ImageFormat.Png))
                return ImageFormat.Png;
            if (img.RawFormat.Equals(ImageFormat.Emf))
                return ImageFormat.Emf;
            if (img.RawFormat.Equals(ImageFormat.Exif))
                return ImageFormat.Exif;
            if (img.RawFormat.Equals(ImageFormat.Gif))
                return ImageFormat.Gif;
            if (img.RawFormat.Equals(ImageFormat.Icon))
                return ImageFormat.Icon;
            if (img.RawFormat.Equals(ImageFormat.MemoryBmp))
                // return ImageFormat.MemoryBmp;
                return ImageFormat.Bmp;
            if (img.RawFormat.Equals(ImageFormat.Tiff))
                return ImageFormat.Tiff;
            else
                return ImageFormat.Wmf;
        }

        public static bool IsImage(string path)
        {
            try
            {
                Image img = Image.FromFile(path);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}