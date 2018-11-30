using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Steamboat.Mobile.Views
{
    public partial class CustomContentPage : ContentPage
    {
		public static readonly BindableProperty iOSKeyboardScrollProperty =
			BindableProperty.Create(nameof(iOSKeyboardScroll), typeof(bool), typeof(CustomContentPage), false);

        public static readonly BindableProperty ShowStatusBarProperty =
            BindableProperty.Create(nameof(ShowStatusBar), typeof(bool), typeof(CustomContentPage), true);

        public bool iOSKeyboardScroll
        {
			get { return (bool)GetValue(iOSKeyboardScrollProperty); }
			set { SetValue(iOSKeyboardScrollProperty, value); }
        }

        public bool ShowStatusBar
        {
            get { return (bool)GetValue(ShowStatusBarProperty); }
            set { SetValue(ShowStatusBarProperty, value); }
        }

        public CustomContentPage()
        {
            CustomNavigationView.SetGradientColors(this, new Tuple<Color, Color>(Color.FromHex("#2C9FC9"), Color.FromHex("#4E72C1")));
            CustomNavigationView.SetGradientDirection(this, CustomNavigationView.GradientDirection.LeftToRight);
            CustomNavigationView.SetHasShadow(this, false);
            InitializeComponent();
        }
    }
}
