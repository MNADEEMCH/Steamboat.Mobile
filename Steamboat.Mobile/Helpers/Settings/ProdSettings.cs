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

        public string iOSAppCenter => "8957dd54-f8c9-4b90-bf1c-bc379253fdee";

        public string AndroidAppCenter => "fb36f4b9-72b9-49f9-9e04-941d0482f9df";
    }
}
