using System;
using Android.App;
using Android.Hardware.Camera2;
using Java.Util.Concurrent;

namespace Steamboat.Mobile.Droid.Helpers.Camera
{
    public interface ICamera2
    {
        CameraDevice mCameraDevice { get; set; }
        Semaphore mCameraOpenCloseLock { get; set; }
        CameraCaptureSession mCaptureSession { get; set; }
        Activity mActivity { get; set; }
        AutoFitTextureView mTextureView { get; set; }

        void OpenCamera(int width, int height);
        void ConfigureTransform(int viewWidth, int viewHeight);
        void StartPreview();
        void UpdatePreview();
    }
}
