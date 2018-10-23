using System;
using Android.Hardware.Camera2;

namespace Steamboat.Mobile.Droid.Helpers.Camera
{
    public class MyCameraStateCallback : CameraDevice.StateCallback
    {
        ICamera2 fragment;
        public MyCameraStateCallback(ICamera2 frag)
        {
            fragment = frag;
        }
        public override void OnOpened(CameraDevice camera)
        {
            fragment.mCameraDevice = camera;
            fragment.StartPreview();
            fragment.mCameraOpenCloseLock.Release();
            if (null != fragment.mTextureView)
                fragment.ConfigureTransform(fragment.mTextureView.Width, fragment.mTextureView.Height);
        }

        public override void OnDisconnected(CameraDevice camera)
        {
            fragment.mCameraOpenCloseLock.Release();
            camera.Close();
            fragment.mCameraDevice = null;
        }

        public override void OnError(CameraDevice camera, CameraError error)
        {
            fragment.mCameraOpenCloseLock.Release();
            camera.Close();
            fragment.mCameraDevice = null;
            if (null != fragment.mActivity)
                fragment.mActivity.Finish();
        }
    }
}
