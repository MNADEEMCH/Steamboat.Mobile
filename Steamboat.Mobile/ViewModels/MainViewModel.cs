using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Managers.Application;
using Steamboat.Mobile.Managers.Participant;
using Steamboat.Mobile.Models.NavigationParameters;
using Steamboat.Mobile.Models.Notification;
using Steamboat.Mobile.Models.Participant.Photojournaling;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties

        private IParticipantManager _participantManager;
        private IApplicationManager _applicationManager;
        private MenuViewModel _menuViewModel;
        public ICommand ChangeMenuIconCommand { get; set; }
        #endregion

        public MainViewModel(IParticipantManager participantManager = null,
                             IApplicationManager applicationManager = null,
                             MenuViewModel menuViewModel = null
                            )
        {
            _participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();
            _applicationManager = applicationManager ?? DependencyContainer.Resolve<IApplicationManager>();
            _menuViewModel = menuViewModel ?? DependencyContainer.Resolve<MenuViewModel>();
        }

        private void OnShowNotifications(object sender, bool showNotificationBadge)
        {
            if (showNotificationBadge)
                ShowNotification();
            else
                RemoveNotification();
        }

        public override Task InitializeAsync(object parameter)
        {
            _menuViewModel.OnShowNotifications += OnShowNotifications;

            RemoveNotification();
            var status = parameter as Models.Participant.Status;
            var unreadMessageCount = status.Dashboard.UnreadMessageCount;
            var dispositionViewModelType = DashboardHelper.GetViewModelForStatus(status);
            var startViewModelType = GetInitialViewModel(status.Dashboard.Nutrition, dispositionViewModelType);
            var pendingViewModelType = _applicationManager.GetPendingViewModelType();
            var currentViewModelType = pendingViewModelType != null ? pendingViewModelType : startViewModelType;
            var navigatingToDispositionStep = pendingViewModelType == null && currentViewModelType != typeof(PhotojournalingViewModel);

            var menuViewModelInitParameter = new MainViewModelInitParameter()
            {
                DispositionStepType = dispositionViewModelType,
                NavigatingToDispositionStep = navigatingToDispositionStep,
                UnreadMessageCount = unreadMessageCount,
                NutritionPlan = status.Dashboard.Nutrition
            };

            return Task.WhenAll
                (
                   _menuViewModel.InitializeAsync(menuViewModelInitParameter),
                   NavigationService.NavigateToAsync(currentViewModelType, status, mainPage: true)
                );
        }

        public void ShowNotification()
        {
            Device.BeginInvokeOnMainThread(() => ChangeMenuIconCommand.Execute(true));
        }

        public void RemoveNotification()
        {
            Device.BeginInvokeOnMainThread(() => ChangeMenuIconCommand.Execute(false));
        }

        public void Reset()
        {
            _menuViewModel.OnShowNotifications -= OnShowNotifications;
        }

        private Type GetInitialViewModel(Nutrition nutrition, Type dispositionViewModelType)
        {
            if (nutrition != null && nutrition.IsEnabled && nutrition.IsActivePhase)
            {
                return typeof(PhotojournalingViewModel);
            }
            return dispositionViewModelType;
        }
    }
}
