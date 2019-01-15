using System;
using Android.Media;
using Java.IO;

namespace Steamboat.Mobile.Droid.Helpers.Files
{
    public static class ImageMetadata
    {
        public static void SetPictureOrientation(string path, int orientation)
        {
            var orientationToSet = Orientation.Undefined;

            switch (orientation)
            {
                case 90:
                    orientationToSet = Orientation.Rotate90;
                    break;

                case 180:
                    orientationToSet = Orientation.Rotate180;
                    break;

                case 270:
                    orientationToSet = Orientation.Rotate270;
                    break;
            }

            ExifInterface exif = new ExifInterface(path);
            exif.SetAttribute(ExifInterface.TagOrientation, ((int)orientationToSet).ToString());
            exif.SaveAttributes();
        }

        public static int GetPictureOrientation(string path)
        {
            var ei = new ExifInterface(path);
            var orientation = ei.GetAttributeInt(ExifInterface.TagOrientation,
                                                 (int)Orientation.Undefined);
            var degreesOrientation = 0;

            switch (orientation)
            {
                case (int)Orientation.Rotate90:
                    degreesOrientation = 90;
                    break;

                case (int)Orientation.Rotate180:
                    degreesOrientation = 180;
                    break;

                case (int)Orientation.Rotate270:
                    degreesOrientation = 270;
                    break;
            }
            return degreesOrientation;
        }
    }
}
