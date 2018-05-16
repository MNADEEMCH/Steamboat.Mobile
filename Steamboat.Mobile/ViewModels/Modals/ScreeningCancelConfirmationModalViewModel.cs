using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Exceptions;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Managers.Participant;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels.Modals
{
    public class ScreeningCancelConfirmationModalViewModel : ModalViewModelBase
    {
        #region Properties

        private Func<Task> AfterCloseModal;
        private IParticipantManager _participantManager;

        public ICommand CancelAppointmentCommand { get; set; }

        #endregion

        public ScreeningCancelConfirmationModalViewModel(IParticipantManager participantManager = null) : base()
        {
            IsLoading = true;
            _participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();
            CancelAppointmentCommand = new Command(async () =>
            {
                await CancelAppointmentAndCloseModal();

            });
        }

        public override Task InitializeAsync(object parameter)
        {
            AfterCloseModal = parameter as Func<Task>;
            IsLoading = false;
            return base.InitializeAsync(parameter);
        }

        private async Task CancelAppointmentAndCloseModal()
        {
            try
            {
                IsBusy = true;
                await CancelAppointment();
                await CloseModal();
                await AfterCloseModal();
            }
            catch (Exception e)
            {
                if (!(e is SessionExpiredException))
                    await DialogService.ShowAlertAsync("Error when trying to cancel event", "Error", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task CancelAppointment()
        {
            try
            {
                await _participantManager.CancelEvent();
            }
            catch (Exception e)
            {
                throw e;
            }

        }

    }
}
