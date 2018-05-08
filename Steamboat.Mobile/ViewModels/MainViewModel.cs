using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Managers.Participant;

namespace Steamboat.Mobile.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties

        private IParticipantManager _participantManager;
        private MenuViewModel _menuViewModel;

        #endregion

        public MainViewModel(IParticipantManager participantManager = null, MenuViewModel menuViewModel = null)
        {
            _participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();
            _menuViewModel = menuViewModel ?? DependencyContainer.Resolve<MenuViewModel>();
        }

        public override Task InitializeAsync(object parameter)
        {
            var status = parameter as Models.Participant.Status;
            var viewModelType = DashboardHelper.GetViewModelForStatus(status);
            return Task.WhenAll
                (
                   _menuViewModel.InitializeAsync(viewModelType),
                   NavigationService.NavigateToAsync(viewModelType, status, mainPage: true)
                );
        }
    }
}
