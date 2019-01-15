using System;
using Android.Media;

namespace Steamboat.Mobile.Droid.Helpers.Camera.Listeners
{
    public class ImageAvailableListener : Java.Lang.Object, ImageReader.IOnImageAvailableListener
    {
        private readonly ICamera2 _owner;

        public ImageAvailableListener(ICamera2 camera)
        {
            if (camera == null)
                throw new System.ArgumentNullException("camera");

            _owner = camera;
        }

        public void OnImageAvailable(ImageReader reader)
        {
            _owner.OnPictureTaken(reader.AcquireNextImage());
        }
    }
}
