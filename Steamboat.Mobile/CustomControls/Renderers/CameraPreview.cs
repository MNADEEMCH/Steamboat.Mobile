using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public enum CameraOptions
    {
        Rear,
        Front
    }

    public class CameraPreview : View
    {
        public static readonly BindableProperty CameraProperty =
            BindableProperty.Create(nameof(Camera), typeof(CameraOptions), typeof(CameraPreview), CameraOptions.Rear);

        public static readonly BindableProperty FilenameProperty =
            BindableProperty.Create(nameof(Filename), typeof(string), typeof(CameraPreview), default(string));

        public static readonly BindableProperty PosterPathProperty =
            BindableProperty.Create(nameof(PosterPath), typeof(string), typeof(CameraPreview), default(string));

        public static readonly BindableProperty TotalBytesProperty =
            BindableProperty.Create(nameof(TotalBytes), typeof(long), typeof(CameraPreview), default(long));

        public static readonly BindableProperty SaveCommandProperty =
            BindableProperty.Create(nameof(SaveCommand), typeof(ICommand), typeof(CameraPreview), default(ICommand));

        public static readonly BindableProperty EncodingIdProperty =
            BindableProperty.Create(nameof(EncodingId), typeof(int), typeof(CameraPreview), default(int));

        public CameraOptions Camera
        {
            get { return (CameraOptions)GetValue(CameraProperty); }
            set { SetValue(CameraProperty, value); }
        }

        public string Filename
        {
            get { return (string)GetValue(FilenameProperty); }
            set { SetValue(FilenameProperty, value); }
        }

        public string PosterPath
        {
            get { return (string)GetValue(PosterPathProperty); }
            set { SetValue(PosterPathProperty, value); }
        }

        public long TotalBytes
        {
            get { return (long)GetValue(TotalBytesProperty); }
            set { SetValue(TotalBytesProperty, value); }
        }

        public ICommand SaveCommand
        {
            get { return (ICommand)GetValue(SaveCommandProperty); }
            set { SetValue(SaveCommandProperty, value); }
        }

        public int EncodingId
        {
            get { return (int)GetValue(EncodingIdProperty); }
            set { SetValue(EncodingIdProperty, value); }
        }

        public Action StartRecording;
        public Action StopRecording;
        public Action Dispose;

        public void OnMediaSaved(string mediaPath, string posterPath, bool isLandscape, long totalBytes)
        {
            Debug.WriteLine("OnMovieSaved {0} {1}", mediaPath, posterPath);
            SetValue(PosterPathProperty, posterPath);
            SetValue(FilenameProperty, mediaPath);
            SetValue(TotalBytesProperty, totalBytes);
            SaveCommand.Execute(isLandscape);
        }
    }
}
