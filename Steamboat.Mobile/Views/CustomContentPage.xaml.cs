using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Steamboat.Mobile.Views
{
    public partial class CustomContentPage : ContentPage
    {
        public CustomContentPage()
        {
            CustomNavigationView.SetImageSource(this, "icLogo.png");
            CustomNavigationView.SetImagePosition(this, CustomNavigationView.ImageAlignment.Center);
            CustomNavigationView.SetImageMargin(this, 0);
            CustomNavigationView.SetGradientColors(this, new Tuple<Color, Color>(Color.FromHex("#2C9FC9"), Color.FromHex("#4E72C1")));
            CustomNavigationView.SetGradientDirection(this, CustomNavigationView.GradientDirection.LeftToRight);
            CustomNavigationView.SetHasShadow(this, false);
            InitializeComponent();
        }
    }
}
