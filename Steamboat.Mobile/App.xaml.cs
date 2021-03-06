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
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Steamboat.Mobile
{
    public partial class App : Application
    {
        
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
        }

        public static async Task HandlePushNotification(bool openedByTouchNotification, bool isAppBackgrounded, PushNotification pushNotification)
        {
            
            await _applicationManager.HandlePushNotification(openedByTouchNotification, isAppBackgrounded,pushNotification);
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
			_applicationManager.OnApplicationStart();

            AppCenter.Start(
                $"ios={_settings.iOSAppCenter}; android={_settings.AndroidAppCenter}",
                typeof(Analytics),
                typeof(Crashes));
        }

        protected override void OnSleep()
        {
            _applicationManager.OnApplicationSleep();
        }

        protected override void OnResume()
        {
            _applicationManager.OnApplicationResume();
        }

    }
}
