using System;
using Android.Hardware.Camera2;

namespace Steamboat.Mobile.Droid.Helpers.Camera.Callbacks
{
    public class CameraCaptureStillPictureSessionCallback : CameraCaptureSession.CaptureCallback
    {
        private static readonly string TAG = "CameraCaptureStillPictureSessionCallback";

        private readonly ICamera2 _owner;
        private readonly bool _restartPreview;

        public CameraCaptureStillPictureSessionCallback(ICamera2 owner, bool restartPreview = false)
        {
            if (owner == null)
                throw new System.ArgumentNullException("owner");
            this._owner = owner;
            this._restartPreview = restartPreview;
        }

        public override void OnCaptureCompleted(CameraCaptureSession session, CaptureRequest request, TotalCaptureResult result)
        {
            if (_restartPreview)
                _owner.StartPreview();
        }
    }
}
