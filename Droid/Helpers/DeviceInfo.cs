using System;
using Steamboat.Mobile.Helpers;

namespace Steamboat.Mobile.Droid.Helpers
{
	public class DeviceInfo : IDeviceInfo
	{
		public bool IsAndroid => true;
		public string Model => throw new NotImplementedException();
	}
}
