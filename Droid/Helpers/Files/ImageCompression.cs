using System;
using System.IO;
using Android.Graphics;
using Java.IO;

namespace Steamboat.Mobile.Droid.Helpers.Files
{
    public static class ImageCompression
    {
        public static byte[] CompressFile(string path, int compressionValue)
        {
            var bitmap = BitmapFactory.DecodeFile(path);
            var stream = new MemoryStream();
            bitmap.Compress(Bitmap.CompressFormat.Jpeg, compressionValue, stream);
            var array = stream.ToArray();
            bitmap.Dispose();
            return array;
        }

        public static byte[] CompressFile(byte[] file, int compressionValue)
        {
            var bitmap = BitmapFactory.DecodeByteArray(file, 0, file.Length);
            var stream = new MemoryStream();
            bitmap.Compress(Bitmap.CompressFormat.Jpeg, compressionValue, stream);
            var array = stream.ToArray();
            bitmap.Dispose();
            return array;
        }
    }
}
