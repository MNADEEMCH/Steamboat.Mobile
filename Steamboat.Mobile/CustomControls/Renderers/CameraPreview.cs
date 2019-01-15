using System;
using System.Windows.Input;
using Steamboat.Mobile.Models.Camera;
using Steamboat.Mobile.Models.NavigationParameters;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class CameraPreview : View
    {
        #region Properties

        public Action StartCamera;
        public Action CloseCamera;
        public Action<PictureSettings> TakePicture;
        public Action ToggleCamera;

        public static readonly BindableProperty CameraReadyCommandProperty =
           BindableProperty.Create(nameof(CameraReadyCommand), typeof(ICommand), typeof(CameraPreview), default(ICommand));

        public static readonly BindableProperty TakePictureCommandProperty =
            BindableProperty.Create(nameof(TakePictureCommand), typeof(ICommand), typeof(CameraPreview), default(ICommand), BindingMode.OneWayToSource);

        public static readonly BindableProperty PictureTakenCommandProperty =
          BindableProperty.Create(nameof(PictureTakenCommand), typeof(ICommand), typeof(CameraPreview), default(ICommand));

        public static readonly BindableProperty ToggleCameraCommandProperty =
         BindableProperty.Create(nameof(ToggleCameraCommand), typeof(ICommand), typeof(CameraPreview), default(ICommand), BindingMode.OneWayToSource);

        public ICommand CameraReadyCommand
        {
            get { return (ICommand)GetValue(CameraReadyCommandProperty); }
            set { SetValue(CameraReadyCommandProperty, value); }
        }

        public ICommand TakePictureCommand
        {
            get { return (ICommand)GetValue(TakePictureCommandProperty); }
            set { SetValue(TakePictureCommandProperty, value); }
        }

        public ICommand PictureTakenCommand
        {
            get { return (ICommand)GetValue(PictureTakenCommandProperty); }
            set { SetValue(PictureTakenCommandProperty, value); }
        }

        public ICommand ToggleCameraCommand
        {
            get { return (ICommand)GetValue(ToggleCameraCommandProperty); }
            set { SetValue(ToggleCameraCommandProperty, value); }
        }

        #endregion

        public CameraPreview()
        {
            TakePictureCommand = new Command<PictureSettings>((param) => TakePicture(param));
            ToggleCameraCommand = new Command(() => { ToggleCamera(); });
        }

        public void OnCameraReady(CamCharacteristics cameraCharacteristics)
        {
            CameraReadyCommand?.Execute(cameraCharacteristics);
        }

        public void OnPictureTaken(PhotoTakenParameter pictureTakenOutput)
        {
            PictureTakenCommand?.Execute(pictureTakenOutput);
        }
    }
}
