using System;
using Steamboat.Mobile.Helpers;

namespace Steamboat.Mobile.iOS.Helpers
{
	public class DeviceInfo : IDeviceInfo
	{
		public bool IsAndroid => false;
		public string Model => Xamarin.iOS.DeviceHardware.Model;
	}
}
