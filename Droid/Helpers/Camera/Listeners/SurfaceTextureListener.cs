using System;
using Android.Graphics;
using Android.Views;

namespace Steamboat.Mobile.Droid.Helpers.Camera.Listeners
{
    public class SurfaceTextureListener : Java.Lang.Object, TextureView.ISurfaceTextureListener
    {
        ICamera2 _camera;

        public SurfaceTextureListener(ICamera2 camera)
        {
            _camera = camera;
        }

        public void OnSurfaceTextureAvailable(SurfaceTexture surface_texture, int width, int height)
        {
            _camera.OpenCamera(width, height);
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface_texture, int width, int height)
        {
            _camera.ConfigureTransform(width, height);
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
