﻿using System;
using Android.Media;
using Java.IO;
using Java.Lang;
using Java.Nio;

namespace Steamboat.Mobile.Droid.Helpers.Camera
{
    public class ImageAvailableListener : Java.Lang.Object, ImageReader.IOnImageAvailableListener
    {
        public ImageAvailableListener(ICamera2 fragment, File file)
        {
            if (fragment == null)
                throw new System.ArgumentNullException("fragment");
            if (file == null)
                throw new System.ArgumentNullException("file");

            owner = fragment;
            this.file = file;
        }

        private readonly File file;
        private readonly ICamera2 owner;

        public void OnImageAvailable(ImageReader reader)
        {
            owner.mBackgroundHandler.Post(new ImageSaver(reader.AcquireNextImage(), owner));
        }

        // Saves a JPEG {@link Image} into the specified {@link File}.
        private class ImageSaver : Java.Lang.Object, IRunnable
        {
            // The JPEG image
            private Image mImage;

            private ICamera2 mOwner;

            public ImageSaver(Image image, ICamera2 owner)
            {
                if (image == null)
                    throw new System.ArgumentNullException("image");

                mImage = image;
                mOwner = owner;
            }

            public void Run()
            {
                ByteBuffer buffer = mImage.GetPlanes()[0].Buffer;
                byte[] bytes = new byte[buffer.Remaining()];
                buffer.Get(bytes);
                mImage.Close();
                mOwner.OnCaptureComplete(bytes);
            }
        }
    }
}
