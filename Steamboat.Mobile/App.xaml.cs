using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;
using Steamboat.Mobile.Models.User;
using Steamboat.Mobile.Services.Navigation;
using Steamboat.Mobile.ViewModels;
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
        public static string pruebaPushNotMessage;

        public App(string fromPush="NO")
        {
            InitializeComponent();
            pruebaPushNotMessage = fromPush;
            //MainPage = new LoginView();
            var navigationService = DependencyContainer.Resolve<INavigationService>();
            navigationService.InitializeAsync();
        }

        protected override void OnStart()
        {
            Push.PushNotificationReceived += (sender, e) => {

                // Add the notification message and title to the message
                var summary = $"Push notification received:" +
                                    $"\n\tNotification title: {e.Title}" +
                                    $"\n\tMessage: {e.Message}";

                // If there is custom data associated with the notification,
                // print the entries
                if (e.CustomData != null)
                {
                    summary += "\n\tCustom data:\n";
                    foreach (var key in e.CustomData.Keys)
                    {
                        summary += $"\t\t{key} : {e.CustomData[key]}\n";
                    }
                }

                pruebaPushNotMessage = e.Message;
                var navigationService = DependencyContainer.Resolve<INavigationService>();
                navigationService.NavigateToAsync<ReportViewModel>();

                // Send the notification summary to debug output
                System.Diagnostics.Debug.WriteLine(summary);
            };

            AppCenter.Start("android=9296455d-1464-48bd-9e68-806e6df4a570;"+
                            "ios=fc7b539c-ea85-4448-b7d3-bdb479134d5a",
                  typeof(Push));

            var customProperty = new CustomProperties();
            customProperty.Set("App Version", AppCenter.SdkVersion);
            customProperty.Set("Empresa", "policia");

            AppCenter.SetCustomProperties(customProperty);
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
