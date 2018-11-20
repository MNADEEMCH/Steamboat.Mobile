using System;
using System.Windows.Input;
using Steamboat.Mobile.Models.NavigationParameters;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class CameraPreview : View
    {
        public Action StartRecording;
        public Action StopRecording;
        public Action Dispose;
        public Action ToggleFlash;
        public Action SwapCamera;

        public static readonly BindableProperty SaveCommandProperty =
           BindableProperty.Create(nameof(SaveCommand), typeof(ICommand), typeof(CameraPreview), default(ICommand));

        public ICommand SaveCommand
        {
            get { return (ICommand)GetValue(SaveCommandProperty); }
            set { SetValue(SaveCommandProperty, value); }
        }

        public void OnPhotoTaken(byte[] media, ImageSource imageSource)
        {
            var param = new PhotoTakenParameter()
            {
                Media = media,
                ImageSource = imageSource
            };

            SaveCommand.Execute(param);
        }
    }
}
