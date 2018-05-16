using System;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class LinkButton : Button
    {
        public static readonly BindableProperty ActiveColorProperty =
            BindableProperty.Create(nameof(ActiveColor), typeof(Color), typeof(LinkButton), default(Color));

        public static readonly BindableProperty ActiveImageProperty =
            BindableProperty.Create(nameof(ActiveImage), typeof(string), typeof(LinkButton), default(string));

        public static readonly BindableProperty TapColorProperty =
            BindableProperty.Create(nameof(TapColor), typeof(Color), typeof(LinkButton), default(Color));

        public static readonly BindableProperty TapImageProperty =
            BindableProperty.Create(nameof(TapImage), typeof(string), typeof(LinkButton), default(string));

        public static readonly BindableProperty DisabledColorProperty =
            BindableProperty.Create(nameof(DisabledColor), typeof(Color), typeof(LinkButton), default(Color));

        public static readonly BindableProperty DisabledImageProperty =
            BindableProperty.Create(nameof(DisabledImage), typeof(string), typeof(LinkButton), default(string));

        public static readonly BindableProperty ImageHeightProperty =
            BindableProperty.Create(nameof(ImageHeight), typeof(int), typeof(LinkButton), 30);

        public static readonly BindableProperty ImageWidthProperty =
            BindableProperty.Create(nameof(ImageWidth), typeof(int), typeof(LinkButton), 30);

        public static readonly BindableProperty ImageTextDistanceProperty =
           BindableProperty.Create(nameof(ImageTextDistance), typeof(double), typeof(LinkButton), 0.0);

        public enum StatusEnum { Active, Tap, Disabled };

        public Color ActiveColor
        {
            get { return (Color)GetValue(ActiveColorProperty); }
            set { SetValue(ActiveColorProperty, value); }
        }

        public string ActiveImage
        {
            get { return (string)GetValue(ActiveImageProperty); }
            set { SetValue(ActiveImageProperty, value); }
        }

        public Color TapColor
        {
            get { return (Color)GetValue(TapColorProperty); }
            set { SetValue(TapColorProperty, value); }
        }

        public string TapImage
        {
            get { return (string)GetValue(TapImageProperty); }
            set { SetValue(TapImageProperty, value); }
        }

        public Color DisabledColor
        {
            get { return (Color)GetValue(DisabledColorProperty); }
            set { SetValue(DisabledColorProperty, value); }
        }

        public string DisabledImage
        {
            get { return (string)GetValue(DisabledImageProperty); }
            set { SetValue(DisabledImageProperty, value); }
        }

        public int ImageWidth
        {
            get { return (int)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        public int ImageHeight
        {
            get { return (int)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        public double ImageTextDistance
        {
            get { return (double)GetValue(ImageTextDistanceProperty); }
            set { SetValue(ImageTextDistanceProperty, value); }
        }

        public string GetImageSourceForStatus(StatusEnum status)
        {
            var imageSource = "";

            switch (status)
            {
                case StatusEnum.Active:
                    imageSource = this.ActiveImage;
                    break;
                case StatusEnum.Tap:
                    imageSource = this.TapImage;
                    break;
                case StatusEnum.Disabled:
                    imageSource = this.DisabledImage;
                    break;
            }

            if (String.IsNullOrEmpty(imageSource))
                imageSource = this.ActiveImage;

            return imageSource;
        }
    }
}
