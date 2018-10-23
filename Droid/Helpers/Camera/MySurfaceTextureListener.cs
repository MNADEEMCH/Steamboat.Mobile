using System;
using Android.Graphics;
using Android.Views;

namespace Steamboat.Mobile.Droid.Helpers.Camera
{
    public class MySurfaceTextureListener : Java.Lang.Object, TextureView.ISurfaceTextureListener
    {
        ICamera2 fragment;

        public MySurfaceTextureListener(ICamera2 frag)
        {
            fragment = frag;
        }

        public void OnSurfaceTextureAvailable(SurfaceTexture surface_texture, int width, int height)
        {
            fragment.OpenCamera(width, height);
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface_texture, int width, int height)
        {
            fragment.ConfigureTransform(width, height);
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface_texture)
        {
            return true;
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface_texture)
        {
        }

    }
}
