using System;
using Android.Graphics;
using Android.Media;
using Java.Nio;
using Java.IO;
using MemoryStream = System.IO.MemoryStream;
using System.IO;

namespace Steamboat.Mobile.Droid.Helpers.Files
{
    public static class ImageBytes
    {
        public static byte[] GetByteArray(Image image)
        {
            ByteBuffer buffer = image.GetPlanes()[0].Buffer;
            buffer.Rewind();
            byte[] bytes = new byte[buffer.Remaining()];
            buffer.Get(bytes);
            return bytes;
        }

        public static byte[] GetByteArray(Bitmap bitmap)
        {
            byte[] bitmapData;
            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                bitmapData = stream.ToArray();
            }
            return bitmapData;
        }

        public static byte[] GetByteArray(string path)
        {
            var imagefile = new Java.IO.File(path);
            int size = (int)imagefile.Length();
            byte[] bytes = new byte[size];

            var input = new FileStream(path, FileMode.Open);

            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(bytes, 0, bytes.Length)) > 0)
                {
                    ms.Write(bytes, 0, read);
                }
            }

            return bytes;
        }
    }
}
