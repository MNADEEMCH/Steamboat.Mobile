using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;
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

        public App()
        {
            InitializeComponent();


            //MainPage = new LoginView();
            var navigationService = DependencyContainer.Resolve<INavigationService>();
            navigationService.InitializeAsync();
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=9296455d-1464-48bd-9e68-806e6df4a570;"+
                            "ios=fc7b539c-ea85-4448-b7d3-bdb479134d5a",
                  typeof(Push));


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
