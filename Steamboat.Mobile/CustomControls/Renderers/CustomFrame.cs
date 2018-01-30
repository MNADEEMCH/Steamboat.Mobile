using System;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class CustomFrame:Frame
    {

        public static readonly BindableProperty IsActiveProperty =
            BindableProperty.Create(nameof(IsActive), typeof(bool), typeof(CustomFrame), false);

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly BindableProperty ActiveOutlineColorProperty =
            BindableProperty.Create(nameof(ActiveOutlineColor), typeof(Color), typeof(CustomFrame), Color.Default);

        public Color ActiveOutlineColor
        {
            get { return (Color)GetValue(ActiveOutlineColorProperty); }
            set { SetValue(ActiveOutlineColorProperty, value); }
        }

        public static readonly BindableProperty ActiveBorderWidthProperty =
            BindableProperty.Create(nameof(ActiveBorderWidth), typeof(double), typeof(CustomFrame), 5.0);

        public double ActiveBorderWidth
        {
            get { return (double)GetValue(ActiveBorderWidthProperty); }
            set { SetValue(ActiveBorderWidthProperty, value); }
        }

        public static readonly BindableProperty DefaultBorderWidthProperty =
            BindableProperty.Create(nameof(DefaultBorderWidth), typeof(double), typeof(CustomFrame), 2.0);

        public double DefaultBorderWidth
        {
            get { return (double)GetValue(DefaultBorderWidthProperty); }
            set { SetValue(DefaultBorderWidthProperty, value); }
        }



        public CustomFrame()
        {
        }
    }
}
