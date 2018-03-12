using System.Threading.Tasks;
using Steamboat.Mobile.Managers;
using Steamboat.Mobile.Models.NavigationParameters;
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

        public App(PushNotificationParameter pushNotificationParameter=null)
        {
            InitializeComponent();

            _applicationManager = _applicationManager ?? DependencyContainer.Resolve<IApplicationManager>();
            _applicationManager.InitializeApplication(pushNotificationParameter);
        }

        public static async Task HandlePushNotification(PushNotificationParameter pushNotificationParameter){

            await _applicationManager.HandlePushNotification(pushNotificationParameter);
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
