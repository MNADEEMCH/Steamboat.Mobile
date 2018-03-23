using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.DeviceInfo;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Managers.Participant;
using Steamboat.Mobile.Models.Application;
using Steamboat.Mobile.Models.Notification;
using Steamboat.Mobile.Services.App;
using Steamboat.Mobile.Services.Dialog;
using Steamboat.Mobile.Services.Navigation;
using Steamboat.Mobile.Services.Notification;
using Xamarin.Forms;

namespace Steamboat.Mobile.Managers.Application
{
    public class ApplicationManager : IApplicationManager
    {
        protected readonly INavigationService _navigationService;
        protected readonly IApplicationService _applicationService;
        protected readonly INotificationService _notificationService;
        protected readonly IParticipantManager _participantManager;

        private PushNotification _pushNotification;
        public PushNotification PushNotification { get{return _pushNotification;} set {_pushNotification=value; }}

        public ApplicationManager(INavigationService navigationService = null,
                                  IApplicationService applicationService = null,
                                  INotificationService notificationService=null,
                                  IParticipantManager participantManager = null)
        {
            _navigationService = navigationService ?? DependencyContainer.Resolve<INavigationService>();
            _applicationService = applicationService ?? DependencyContainer.Resolve<IApplicationService>();
            _notificationService= notificationService ?? DependencyContainer.Resolve<INotificationService>();
            _participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();
        }

        public async Task InitializeApplication(PushNotification pushNotification = null)
        {
            PushNotification = pushNotification;

            if (pushNotification != null)
                UpdateNotificationBadge(0);

            await _navigationService.InitializeAsync();
        }

        public async Task HandlePushNotification(PushNotification pushNotification)
        {

            PushNotification = pushNotification;

            var isUserLoggedIn = App.SessionID != null;
            if(isUserLoggedIn){
                if (pushNotification.Data.ContainsKey(NotificationDataHelper.NavigateTo))
                    await HandlePushNotificationNavigatTo(pushNotification.Data[NotificationDataHelper.NavigateTo]);
                
                UpdateNotificationBadge(pushNotification.Badge);
            }
            else{
                UpdateNotificationBadge(0);
            }
        }

        public void UpdateNotificationBadge(int notificationsOpened){

            int updatedBadge = GetBadgeToSet(notificationsOpened);

            _notificationService.SetNotificationBadge(updatedBadge);
        }

        private int GetBadgeToSet(int notificationsOpened){
            var currentBadge = PushNotification != null ? PushNotification.Badge : 0;

            var updatedBadge = 0;

            if (currentBadge >= notificationsOpened)
                updatedBadge = currentBadge - notificationsOpened;

            return updatedBadge;
        }

        private async Task HandlePushNotificationNavigatTo(string navigateToKey){

            if(NotificationNavigateToHelper.NotificationNavigateToDictionary.ContainsKey(navigateToKey)){
                
                var viewModelType = NotificationNavigateToHelper.NotificationNavigateToDictionary[navigateToKey];
                if (viewModelType != null)
                    await NavigateToView(viewModelType);
                else
                    await NavigateToStatusView();
            }
        }

        private async Task NavigateToView(Type viewModelType)
        {
            await _navigationService.NavigateToAsync(viewModelType, null, mainPage: true);
        }

        private async Task NavigateToStatusView()
        {
            var status = await _participantManager.GetStatus();
            var viewModelType = DashboardHelper.GetViewModelForStatus(status);

            Device.BeginInvokeOnMainThread(async () =>
            {
                await _navigationService.NavigateToAsync(viewModelType, status, mainPage: true);
            });

        }

        public async Task TrySendToken()
        {
            try
            {
                var isUserLoggedIn = App.SessionID != null;
                var readyToSendToken = _notificationService.IsValidToken() && isUserLoggedIn;
                if (readyToSendToken)
                {
                    var devicePlatform = CrossDeviceInfo.Current.Platform.ToString();
                    var deviceModel = CrossDeviceInfo.Current.Model;
                    var deviceLocalID = _notificationService.GetToken();

                    var applicationDeviceInfo = new ApplicationDeviceInfo()
                    {
                        Platform = devicePlatform,
                        Model = deviceModel,
                        LocalID = deviceLocalID
                    };

                    await _applicationService.SendToken(applicationDeviceInfo, App.SessionID);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in login: {ex}");
                throw ex;
            }
        }
    }
}
