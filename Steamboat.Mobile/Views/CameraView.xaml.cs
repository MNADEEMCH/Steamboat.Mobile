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

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            Camera.StartRecording();
        }

        void Handle_Clicked_1(object sender, System.EventArgs e)
        {
            Camera.ToggleFlash();
        }

        void Handle_Clicked_2(object sender, System.EventArgs e)
        {
            Camera.SwapCamera();
        }
    }
}
