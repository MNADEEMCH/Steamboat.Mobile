using System;
using System.Threading.Tasks;
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
        public static CurrentUser CurrentUser;
        public static string SessionID;

        public static IApplicationManager _applicationManager;

        public App(PushNotification pushNotification=null)
        {
            InitializeComponent();

            _applicationManager = _applicationManager ?? DependencyContainer.Resolve<IApplicationManager>();
            _applicationManager.InitializeApplication(pushNotification);
        }

        public static async Task HandlePushNotification(PushNotification pushNotification)
        {

            await _applicationManager.HandlePushNotification(pushNotification);
        }

        public static async Task PushTokenRefreshed()
        {
            try{
                await _applicationManager.TrySendToken();
            }
            catch(Exception ex){
                
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
