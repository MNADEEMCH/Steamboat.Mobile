using System;
using Android.Hardware.Camera2;
using Java.Lang;

namespace Steamboat.Mobile.Droid.Helpers.Camera.Listeners
{
    public class CameraCaptureListener : CameraCaptureSession.CaptureCallback
    {
        private readonly ICamera2 owner;

        public CameraCaptureListener(ICamera2 owner)
        {
            if (owner == null)
                throw new System.ArgumentNullException("owner");
            this.owner = owner;
        }

        public override void OnCaptureCompleted(CameraCaptureSession session, CaptureRequest request, TotalCaptureResult result)
        {
            base.OnCaptureCompleted(session, request, result);
            Process(result);
        }

        public override void OnCaptureProgressed(CameraCaptureSession session, CaptureRequest request, CaptureResult partialResult)
        {
            base.OnCaptureProgressed(session, request, partialResult);
            Process(partialResult);
        }

        private void Process(CaptureResult result)
        {
            switch (owner.mState)
            {
                case Camera2BasicState.STATE_PREVIEW:
                    //do nothing
                    break;
                case Camera2BasicState.STATE_WAITING_AF_AUTO:
                    {
                        Integer afMode = (Integer)result.Get(CaptureResult.ControlAfMode);

                        if (afMode != null)
                            if ((int)ControlAFMode.Auto == afMode.IntValue())
                            {
                                owner.LockFocus();
                            }
                    }
                    break;
                case Camera2BasicState.STATE_WAITING_LOCK:
                    {
                        Integer afState = (Integer)result.Get(CaptureResult.ControlAfState);

                        if (afState != null)
                            if ((((int)ControlAFState.FocusedLocked) == afState.IntValue()))
                            {
                                owner.UnlockFocus();

                            }
                            else if ((((int)ControlAFState.NotFocusedLocked) == afState.IntValue()))
                            {

                                owner.GetRidOfNotFocusedLock();

                            }
                            else
                            {
                                Console.Write("" + afState.IntValue());
                            }
                    }
                    break;
            }
        }
    }
}
