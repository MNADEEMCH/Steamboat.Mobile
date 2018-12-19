using System;

namespace Steamboat.Mobile.Helpers
{
	public interface IDeviceInfo
	{
		bool IsAndroid { get; }
		string Model { get; }

        void OpenAppSettings();
    }
}
