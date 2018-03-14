using System;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.NavigationParameters;
using Steamboat.Mobile.Services.Dialog;
using Steamboat.Mobile.Services.Navigation;

namespace Steamboat.Mobile.Managers
{
    public class ApplicationManager:IApplicationManager
    {
        protected readonly IDialogService DialogService;
        private static bool IsInitialized=false;

        public ApplicationManager(IDialogService dialogService=null)
        {
            DialogService = dialogService ?? DependencyContainer.Resolve<IDialogService>();
        }

        public async Task InitializeApplication(PushNotificationParameter pushNotificationParameter = null)
        {
            var navigationService = DependencyContainer.Resolve<INavigationService>();
            await navigationService.InitializeAsync();
            await this.DialogService.ShowAlertAsync(pushNotificationParameter.PruebaPush, "PUSH-INI", "OK");
            //ApplicationManager.IsInitialized = true;
        }

        public async Task HandlePushNotification(PushNotificationParameter pushNotificationParameter)
        {   
            //if(ApplicationManager.IsInitialized)
            await this.DialogService.ShowAlertAsync(pushNotificationParameter.PruebaPush, "PUSH-HANDLE", "OK");
        }

        public async Task TokenRefreshed(string token,string sessionID){

            //call backend
        }


    }
}
