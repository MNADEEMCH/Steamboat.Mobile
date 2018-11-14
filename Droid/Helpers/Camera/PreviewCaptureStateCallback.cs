using System;
using Android.Hardware.Camera2;

namespace Steamboat.Mobile.Droid.Helpers.Camera
{
    public class PreviewCaptureStateCallback : CameraCaptureSession.StateCallback
    {
        ICamera2 fragment;
        public PreviewCaptureStateCallback(ICamera2 frag)
        {
            fragment = frag;
        }
        public override void OnConfigured(CameraCaptureSession session)
        {
            fragment.mCaptureSession = session;
            fragment.UpdatePreview();

        }

        public override void OnConfigureFailed(CameraCaptureSession session)
        {

        }
    }
}
