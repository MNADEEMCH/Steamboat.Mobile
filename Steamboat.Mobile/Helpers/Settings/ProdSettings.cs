using System;
namespace Steamboat.Mobile.Helpers.Settings
{
	public class ProdSettings : ISettings
	{
		public string BaseUrl => "https://momentumhealth.co/";

		public int TimeoutLimit => 5;

		public int ReportTimeoutLimit => 30;

		public string RequestProviderCookieUrl => "momentumhealth.co";

		public string RequestProviderApiEnvironment => "AD78E2AD-0599-4653-BD97-251F675F636D";
	}
}
