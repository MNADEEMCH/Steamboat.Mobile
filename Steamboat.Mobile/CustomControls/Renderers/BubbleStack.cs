using System;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class BubbleStack : StackLayout
    {
        #region Properties

        public static readonly BindableProperty BorderRadiusProperty =
            BindableProperty.Create(nameof(BorderRadius), typeof(int), typeof(BubbleStack), 0);

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(BubbleStack), default(Color));

        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create(nameof(BorderWidth), typeof(int), typeof(BubbleStack), 0);

        public static readonly BindableProperty FillColorProperty =
            BindableProperty.Create(nameof(FillColor), typeof(Color), typeof(BubbleStack), default(Color));

        public static readonly BindableProperty IsLabelProperty =
            BindableProperty.Create(nameof(IsLabel), typeof(bool), typeof(BubbleStack), default(bool));

        public int BorderRadius
        {
            get { return (int)GetValue(BorderRadiusProperty); }
            set { SetValue(BorderRadiusProperty, value); }
        }

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public int BorderWidth
        {
            get { return (int)GetValue(BorderWidthProperty); }
            set { SetValue(BorderWidthProperty, value); }
        }

        public Color FillColor
        {
            get { return (Color)GetValue(FillColorProperty); }
            set { SetValue(FillColorProperty, value); }
        }

        public bool IsLabel
        {
            get { return (bool)GetValue(IsLabelProperty); }
            set { SetValue(IsLabelProperty, value); }
        }

        #endregion
    }
}
