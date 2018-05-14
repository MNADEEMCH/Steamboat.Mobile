using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Managers.Participant;
using Steamboat.Mobile.ViewModels.Modals;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class SchedulingEventAppointmentModelBase : ViewModelBase
    {
        #region Properties

        private string _schedullingEventTitle;
        protected IParticipantManager _participantManager;
        protected bool _showCancelAppointment;

        public string SchedullingEventTitle { get { return _schedullingEventTitle; } set { SetPropertyValue(ref _schedullingEventTitle, value); } }
        public ICommand CancelAppointmentConfirmCommand { get; set; }
        public bool ShowCancelAppointment { get { return _showCancelAppointment; } set { SetPropertyValue(ref _showCancelAppointment, value); } }

        #endregion

        public SchedulingEventAppointmentModelBase(IParticipantManager participantManager = null)
        {
            IsLoading = true;

            _participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();
            CancelAppointmentConfirmCommand = new Command(async () => await CancelAppointmentConfirm());
        }

        private async Task CancelAppointmentConfirm()
        {
            Func<Task> afterCloseModalFunction = AfterCloseModal;
            await ModalService.PushAsync<ScreeningCancelConfirmationModalViewModel>(afterCloseModalFunction);
        }

        private async Task AfterCloseModal()
        {
            await TryExecute(async () =>
            {
            IsLoading = true;
            await NavigateToStatusView();
            });
        }

        private async Task NavigateToStatusView()
        {
            var status = await _participantManager.GetStatus();
            var viewModelType = DashboardHelper.GetViewModelForStatus(status);
            await NavigationService.NavigateToAsync(viewModelType, status, mainPage: true);
            DependencyContainer.Resolve<MenuViewModel>().UpdateMenuItem(viewModelType);
        }
    }
}
