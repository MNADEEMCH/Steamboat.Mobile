using System;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class GradientRoundedButton : Button
    {
        public static readonly BindableProperty StartColorProperty =
            BindableProperty.Create(nameof(StartColor), typeof(Color), typeof(GradientRoundedButton), Color.FromHex("#17EAD9"));

        public static readonly BindableProperty EndColorProperty =            
            BindableProperty.Create(nameof(EndColor), typeof(Color), typeof(GradientRoundedButton), Color.FromHex("#6078EA"));

        public static readonly BindableProperty DisabledColorProperty =
            BindableProperty.Create(nameof(DisabledColor), typeof(Color), typeof(GradientRoundedButton), Color.FromHex("#E3F4F8"));

        public static readonly BindableProperty ActiveColorProperty =
            BindableProperty.Create(nameof(ActiveColor), typeof(Color), typeof(GradientRoundedButton), Color.FromHex("#ADD7E1"));

        public static readonly BindableProperty DisabledTextColorProperty =
            BindableProperty.Create(nameof(DisabledTextColor), typeof(Color), typeof(GradientRoundedButton), Color.FromHex("#ADD7E1"));
        
        public static readonly BindableProperty IOSBorderRadiusProperty =
            BindableProperty.Create(nameof(IOSBorderRadius), typeof(int), typeof(GradientRoundedButton), 25);

        public static readonly BindableProperty AndroidBorderRadiusProperty =
            BindableProperty.Create(nameof(AndroidBorderRadius), typeof(int), typeof(GradientRoundedButton), 70);

        public Color StartColor
        {
            get { return (Color)GetValue(StartColorProperty); }
            set { SetValue(StartColorProperty, value); }
        }

        public Color EndColor
        {
            get { return (Color)GetValue(EndColorProperty); }
            set { SetValue(EndColorProperty, value); }
        }

        public Color DisabledColor
        {
            get { return (Color)GetValue(DisabledColorProperty); }
            set { SetValue(DisabledColorProperty, value); }
        }

        public Color ActiveColor
        {
            get { return (Color)GetValue(ActiveColorProperty); }
            set { SetValue(ActiveColorProperty, value); }
        }

        public Color DisabledTextColor
        {
            get { return (Color)GetValue(DisabledTextColorProperty); }
            set { SetValue(DisabledTextColorProperty, value); }
        }

        public int IOSBorderRadius
        {
            get { return (int)GetValue(IOSBorderRadiusProperty); }
            set { SetValue(IOSBorderRadiusProperty, value); }
        }

        public int AndroidBorderRadius
        {
            get { return (int)GetValue(AndroidBorderRadiusProperty); }
            set { SetValue(AndroidBorderRadiusProperty, value); }
        }
    }
}
