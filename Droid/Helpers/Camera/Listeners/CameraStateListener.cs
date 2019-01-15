using System;
using Android.Hardware.Camera2;

namespace Steamboat.Mobile.Droid.Helpers.Camera.Listeners
{
    public class CameraStateListener : CameraDevice.StateCallback
    {
        ICamera2 _camera;
        public CameraStateListener(ICamera2 camera)
        {
            _camera = camera;
        }
        public override void OnOpened(CameraDevice camera)
        {
            _camera.mCameraDevice = camera;
            _camera.StartPreview();
            _camera.mCameraOpenCloseLock.Release();
            if (null != _camera.mAutoFitTextureView)
                _camera.ConfigureTransform(_camera.mAutoFitTextureView.Width, _camera.mAutoFitTextureView.Height);
        }

        public override void OnDisconnected(CameraDevice camera)
        {
            _camera.mCameraOpenCloseLock.Release();
            camera.Close();
            _camera.mCameraDevice = null;
        }

        public override void OnError(CameraDevice camera, CameraError error)
        {
            _camera.mCameraOpenCloseLock.Release();
            camera.Close();
            _camera.mCameraDevice = null;
            if (null != _camera.mActivity)
                _camera.mActivity.Finish();
        }
    }
}
