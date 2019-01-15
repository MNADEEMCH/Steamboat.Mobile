using System;
using Android.Graphics;

namespace Steamboat.Mobile.Droid.Helpers.Files
{
    public class ImageTransformations
    {
        public static byte[] GetRotatedSavedImage(string path, int orientation)
        {
            var bitmap = BitmapFactory.DecodeFile(path);
            Bitmap rotatedBitmap = bitmap;

            switch (orientation)
            {
                case 90:
                    rotatedBitmap = RotateImage(bitmap, 90);
                    break;

                case 180:
                    rotatedBitmap = RotateImage(bitmap, 180);
                    break;

                case 270:
                    rotatedBitmap = RotateImage(bitmap, 270);
                    break;
            }

            return ImageBytes.GetByteArray(rotatedBitmap);
        }

        public static byte[] GetRotatedSavedImage(byte[] image, int orientation)
        {
            var bitmap = BitmapFactory.DecodeByteArray(image, 0, image.Length);
            Bitmap rotatedBitmap = bitmap;

            switch (orientation)
            {
                case 90:
                    rotatedBitmap = RotateImage(bitmap, 90);
                    break;

                case 180:
                    rotatedBitmap = RotateImage(bitmap, 180);
                    break;

                case 270:
                    rotatedBitmap = RotateImage(bitmap, 270);
                    break;
            }

            return ImageBytes.GetByteArray(rotatedBitmap);
        }

        private static Bitmap RotateImage(Bitmap source, float angle)
        {
            Matrix matrix = new Matrix();
            matrix.PostRotate(angle);
            return Bitmap.CreateBitmap(source, 0, 0, source.Width, source.Height, matrix, true);
        }

        public static Bitmap CroppImage(Bitmap bitmap, bool widthCut, int width, int height)
        {
            Bitmap croppedBmp = null;

            if (widthCut)
            {
                var ratio = (double)height / width;
                var cropedImageWidth = (int)(bitmap.Height / ratio);
                var widthSideToCrop = (bitmap.Width - cropedImageWidth) / 2;
                croppedBmp = Bitmap.CreateBitmap(bitmap, widthSideToCrop, 0, cropedImageWidth, bitmap.Height);
            }
            else
            {
                var ratio = (double)width / height;
                var cropedImageHeight = (int)(bitmap.Width / ratio);
                var heightSideToCrop = (bitmap.Height - cropedImageHeight) / 2;
                croppedBmp = Bitmap.CreateBitmap(bitmap, 0, heightSideToCrop, bitmap.Width, cropedImageHeight);
            }

            return croppedBmp;
        }
    }
}
