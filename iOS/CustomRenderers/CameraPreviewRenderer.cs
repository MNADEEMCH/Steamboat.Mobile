using System;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using AVFoundation;
using CoreGraphics;
using Foundation;
using System.Diagnostics;
using Steamboat.Mobile.iOS.Utilities.Camera;
using Steamboat.Mobile.Models.Camera;
using Steamboat.Mobile.Models.NavigationParameters;

[assembly: ExportRenderer(typeof(CameraPreview), typeof(CameraPreviewRenderer))]
namespace Steamboat.Mobile.iOS.CustomRenderers
{
    public class CameraPreviewRenderer : ViewRenderer<CameraPreview, UIView>
    {
        UIView _cameraPreview;
        CameraManager _cameraManager = new CameraManager();

        protected override void OnElementChanged(ElementChangedEventArgs<CameraPreview> e)
        {
            base.OnElementChanged(e);

            if (Xamarin.Forms.DesignMode.IsDesignModeEnabled)
                return;

            if (Control == null)
            {
                _cameraPreview = new UIView(new CGRect());
                _cameraPreview.BackgroundColor = UIColor.Black;
                SetNativeControl(_cameraPreview);

                e.NewElement.StartCamera = (() => { });
                e.NewElement.CloseCamera = (() => { });
                e.NewElement.ToggleCamera = (() => { ToggleCamera(); });
                e.NewElement.TakePicture = ((pictureSettings) => { TakePicture(pictureSettings); });
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (Xamarin.Forms.DesignMode.IsDesignModeEnabled)
                return;

            if (_cameraManager != null)
            {
                InitManager();
            }
        }

        private void InitManager()
        {
            _cameraManager.CameraReadyEventHandler += CameraReady;
            _cameraManager.AddPreviewLayerToView(Control);
        }

        private void CameraReady(object sender, CamCharacteristics cameraCharacteristics)
        {
            Element.OnCameraReady(cameraCharacteristics);
        }

        private void ToggleCamera()
        {
            _cameraManager?.ToggleCamera();
        }

        private void TakePicture(PictureSettings pictureSettings)
        {
            _cameraManager?.ToggleFlash(pictureSettings.EnableFlash);
            _cameraManager?.TakePicture(
                ((image, err) =>
                {
                    NSData imageData = null;
                    ImageSource imageSource = null;

                    //COMPRESSION
                    if (pictureSettings.ApplyCompression)
                        imageData = image.AsJPEG((float)pictureSettings.CompressionQuality);
                    else
                        imageData = image.AsJPEG(1);

                    imageSource = ImageSource.FromStream(imageData.AsStream);

                    var pictureTakenOutput = new PhotoTakenParameter
                    {
                        ImageSource = imageSource,
                        Media = imageData.ToArray()
                    };
                    Element.OnPictureTaken(pictureTakenOutput);
                })
            );
        }
    }
}
