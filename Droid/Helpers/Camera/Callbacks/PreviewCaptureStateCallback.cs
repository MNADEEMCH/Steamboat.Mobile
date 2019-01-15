using System;
using Android.Hardware.Camera2;

namespace Steamboat.Mobile.Droid.Helpers.Camera.Callbacks
{
    public class PreviewCaptureStateCallback : CameraCaptureSession.StateCallback
    {
        ICamera2 _camera;
        public PreviewCaptureStateCallback(ICamera2 camera)
        {
            _camera = camera;
        }
        public override void OnConfigured(CameraCaptureSession session)
        {
            _camera.mCaptureSession = session;
            _camera.UpdatePreview();

        }

        public override void OnConfigureFailed(CameraCaptureSession session)
        {

        }
    }
}
