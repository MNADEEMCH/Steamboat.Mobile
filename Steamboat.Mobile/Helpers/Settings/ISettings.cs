using System;
namespace Steamboat.Mobile.Helpers.Settings
{
	public interface ISettings
	{
		string BaseUrl { get; }

		int TimeoutLimit { get; }

		string RequestProviderCookieUrl { get; }

		string RequestProviderApiEnvironment { get; }
	}
}
