using System;
using Android.App;
using Android.Content;
using Steamboat.Mobile.Helpers;

namespace Steamboat.Mobile.Droid.Helpers
{
	public class DeviceInfo : IDeviceInfo
	{
        private string _packageName => "com.MomentumHealth";
        public bool IsAndroid => true;
		public string Model => throw new NotImplementedException();

        public void OpenAppSettings()
        {
            var intent = new Intent(Android.Provider.Settings.ActionApplicationDetailsSettings);
            intent.AddFlags(ActivityFlags.NewTask);
            var uri = Android.Net.Uri.FromParts("package", _packageName, null);
            intent.SetData(uri);
            Application.Context.StartActivity(intent);
        }
    }
}
