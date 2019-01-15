using System;
using Android.App;
using Android.Hardware.Camera2;
using Android.Media;
using Java.Util.Concurrent;

namespace Steamboat.Mobile.Droid.Helpers.Camera
{
    public interface ICamera2
    {
        Activity mActivity { get; set; }
        AutoFitTextureView mAutoFitTextureView { get; set; }
        CameraDevice mCameraDevice { get; set; }
        Semaphore mCameraOpenCloseLock { get; set; }
        CameraCaptureSession mCaptureSession { get; set; }
        Camera2BasicState mState { get; set; }

        void OpenCamera(int width, int height);

        void ConfigureTransform(int viewWidth, int viewHeight);
        void StartPreview();
        void UpdatePreview();

        void UnlockFocus();
        void LockFocus();
        void GetRidOfNotFocusedLock();

        void OnPictureTaken(Image image);
    }
}
