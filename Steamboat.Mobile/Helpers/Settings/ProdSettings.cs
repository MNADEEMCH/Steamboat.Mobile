using System;
namespace Steamboat.Mobile.Helpers.Settings
{
	public class ProdSettings : ISettings
	{
		public string BaseUrl => "https://momentumhealth.co/";

		public int TimeoutLimit => 15;

		public string RequestProviderCookieUrl => "momentumhealth.co";

        public string RequestProviderApiEnvironment => "F5752008-E484-4691-B58A-3338A90F80AA";
	}
}
