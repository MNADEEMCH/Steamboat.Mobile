using System;
using Foundation;
using Steamboat.Mobile.Helpers;
using UIKit;

namespace Steamboat.Mobile.iOS.Helpers
{
	public class DeviceInfo : IDeviceInfo
	{
        private string _appBundleId => "com.MomentumHealth";
        public bool IsAndroid => false;
		public string Model => Xamarin.iOS.DeviceHardware.Model;

        public void OpenAppSettings()
        {
            var url = new NSUrl($"app-settings:{_appBundleId}");
            UIApplication.SharedApplication.OpenUrl(url);
        }
    }
}
