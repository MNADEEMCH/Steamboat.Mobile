using System;
namespace Steamboat.Mobile.Helpers.Settings
{
	public class DevSettings : ISettings
	{
		public string BaseUrl => "https://dev.momentumhealth.co/";

		public int TimeoutLimit => 5;

		public string RequestProviderCookieUrl => "momentumhealth.co";

		public string RequestProviderApiEnvironment => "F5752008-E484-4691-B58A-3338A90F80AA";
	}
}
