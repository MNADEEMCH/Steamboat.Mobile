using System;
using Android.Media;
using Java.IO;
using Java.Nio;

namespace Steamboat.Mobile.Droid.Helpers.Files
{
    public static class ImageSaver
    {
        public static void SaveImage(Image image, File file)
        {
            ByteBuffer buffer = image.GetPlanes()[0].Buffer;
            buffer.Rewind();
            byte[] bytes = new byte[buffer.Remaining()];
            buffer.Get(bytes);

            using (var output = new FileOutputStream(file))
            {
                try
                {
                    output.Write(bytes);
                }
                catch (IOException e)
                {
                    e.PrintStackTrace();
                }
            }
        }

        public static void SaveImage(byte[] bytes, File file)
        {
            using (var output = new FileOutputStream(file))
            {
                try
                {
                    output.Write(bytes);
                }
                catch (IOException e)
                {
                    e.PrintStackTrace();
                }
            }
        }
    }
}
