using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Steamboat.Mobile.Views
{
    public partial class CameraView : CustomContentPage
    {
        public CameraView()
        {
            InitializeComponent();

            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        protected override void OnDisappearing()
        {
            Camera.CloseCamera();
            base.OnDisappearing();
        }

        protected override void OnAppearing()
        {
            Camera.StartCamera();
            base.OnAppearing();
        }
    }
}
