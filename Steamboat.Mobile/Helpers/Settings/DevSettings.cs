using System;
namespace Steamboat.Mobile.Helpers.Settings
{
    public class DevSettings : ISettings
    {
        public string BaseUrl => "https://dev.momentumhealth.co/";

        public int TimeoutLimit => 5;

        public int ReportTimeoutLimit => 30;

        public string RequestProviderCookieUrl => "momentumhealth.co";

        public string RequestProviderApiEnvironment => "F5752008-E484-4691-B58A-3338A90F80AA";

        public string iOSAppCenter => "e9e36903-17f9-4ca4-af87-912056d5ac42";

        public string AndroidAppCenter => "b55b3c0b-224d-4d81-b0c9-80715df0d6eb";
    }
}
