using System;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using AVFoundation;
using CoreGraphics;
using Foundation;
using Steamboat.Mobile.iOS.Utilities;
using System.Diagnostics;

[assembly: ExportRenderer(typeof(CameraPreview), typeof(CameraPreviewRenderer))]
namespace Steamboat.Mobile.iOS.CustomRenderers
{
    public class CameraPreviewRenderer : ViewRenderer<CameraPreview, UIView>
    {
        AVCaptureVideoOrientation orientation;
        CameraManager cameraManager = new CameraManager();
        UIView cameraPreview;

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (Xamarin.Forms.DesignMode.IsDesignModeEnabled)
            {
                return;
            }

            if (cameraManager != null)
            {
                InitManager();
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CameraPreview> e)
        {
            base.OnElementChanged(e);

            if (Xamarin.Forms.DesignMode.IsDesignModeEnabled)
            {
                return;
            }

            if (Control == null)
            {
                cameraPreview = new UIView(new CGRect());
                cameraPreview.BackgroundColor = UIColor.Red;
                SetNativeControl(cameraPreview);

                this.BackgroundColor = UIColor.Cyan;
            }

            if (e.NewElement != null)
            {
                e.NewElement.StartRecording = (() => { StartRecording(); });
                e.NewElement.Dispose = (() => { OnDispose(); });
                e.NewElement.ToggleFlash = (() => { ToggleFlash(); });
                e.NewElement.SwapCamera = (() => { SwapCamera(); });
            }
        }

        private void ToggleFlash()
        {
            if (cameraManager.FlashMode == CameraFlashMode.On)
                cameraManager.FlashMode = CameraFlashMode.Off;
            else
                cameraManager.FlashMode = CameraFlashMode.On;
        }

        private void SwapCamera()
        {
            if (cameraManager.CameraDevice == CameraDevice.Back)
                cameraManager.CameraDevice = CameraDevice.Front;
            else
                cameraManager.CameraDevice = CameraDevice.Back;
        }

        protected override void Dispose(bool disposing)
        {
            Debug.WriteLine("Dispose");
            if (disposing)
            {
                if (Control != null)
                {
                    Control.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        public void StartRecording()
        {
            Debug.WriteLine("StartRecording");

            if (cameraManager.OutputMode == CameraOutputMode.StillImage)
            {
                cameraManager.CapturePicture(async (img, err) =>
                {
                    NSData imgData = img.AsJPEG(0.1f);

                    Element.OnPhotoTaken(imgData.ToArray());
                });
            }
            else
            {
                cameraManager.startRecordingVideo();
            }
        }

        void InitManager()
        {
            if (cameraManager == null)
            {
                cameraManager = new CameraManager();
            }
            cameraManager.AddPreviewLayerToView(Control, CameraOutputMode.StillImage, OnCameraReady);
            Debug.WriteLine("^^^ NEW CAMERA ^^^");
        }

        private void OnCameraReady()
        {

        }

        public void OnDispose()
        {
            Debug.WriteLine("OnDispose");
            Dispose(true);
        }
    }
}
