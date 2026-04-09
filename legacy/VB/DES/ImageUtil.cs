using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace DES
{
    public abstract class ImageUtil
    {
        public static bool GenerateThumbnail(string sSource, string sThumbnail, int thumbWidth, int thumbHeight, ImageFormat format)
        {
            Image.GetThumbnailImageAbort myCallback =
                        new Image.GetThumbnailImageAbort(ThumbnailCallback);

            Bitmap imageBitmap = new Bitmap(sSource);
            Image imageThumbnail = imageBitmap.GetThumbnailImage(thumbWidth, thumbHeight, myCallback, IntPtr.Zero);

            imageThumbnail.Save(sThumbnail, format);

            return true;
        }

        public static bool GenerateThumbnail(Image imageBitmap, string sThumbnail, int thumbWidth, int thumbHeight, ImageFormat format)
        {
            Image.GetThumbnailImageAbort myCallback =
                        new Image.GetThumbnailImageAbort(ThumbnailCallback);

            //Bitmap imageBitmap = new Bitmap(sSource);
            Image imageThumbnail = imageBitmap.GetThumbnailImage(thumbWidth, thumbHeight, myCallback, IntPtr.Zero);

            imageThumbnail.Save(sThumbnail, format);

            return true;
        }

        private static bool ThumbnailCallback()
        {
            return false;
        }

        public static Image GetImage(string sSource)
        {
            return new Bitmap(sSource);
        }
    }
}
