using System;
using System.Threading.Tasks;
using Steamboat.Mobile.Helpers.Settings;
using Steamboat.Mobile.Managers.Application;
using Steamboat.Mobile.Models.Notification;
using Steamboat.Mobile.Models.User;
using Steamboat.Mobile.Services.Navigation;
using Steamboat.Mobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Steamboat.Mobile
{
    public partial class App : Application
    {
        private TimeSpan _inactivityTimeStamp;
        private readonly int _timeoutLimit;

        public static CurrentUser CurrentUser;
        public static string SessionID;

        public static IApplicationManager _applicationManager;
		private ISettings _settings;

		public App(PushNotification pushNotification = null, ISettings settings = null)
        {
            InitializeComponent();

            _applicationManager = _applicationManager ?? DependencyContainer.Resolve<IApplicationManager>();
			_settings = settings ?? DependencyContainer.Resolve<ISettings>();
            _applicationManager.InitializeApplication(pushNotification);
			_timeoutLimit = _settings.TimeoutLimit;
        }

        public static async Task HandlePushNotification(PushNotification pushNotification)
        {

            await _applicationManager.HandlePushNotification(pushNotification);
        }

        public static async Task PushTokenRefreshed()
        {
            try
            {
                await _applicationManager.TrySendToken();
            }
            catch (Exception ex)
            {

            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            StartTimingInactivity();
        }

        protected override void OnResume()
        {
            CheckInactivity();
        }

        private void StartTimingInactivity(){
            if (SessionID != null)
                _inactivityTimeStamp = new TimeSpan(DateTime.Now.Ticks);
        }

        private void CheckInactivity()
        {
            if (SessionID != null)
            {
                var currentTimeSpan = new TimeSpan(DateTime.Now.Ticks);
                var difference = currentTimeSpan - _inactivityTimeStamp;

                if (difference.Minutes >= _timeoutLimit)
                    _applicationManager.SessionExpired();
            }
        }
    }
}
