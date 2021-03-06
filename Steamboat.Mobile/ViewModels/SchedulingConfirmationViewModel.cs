using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Managers.Participant;
using Steamboat.Mobile.Models.NavigationParameters;
using Steamboat.Mobile.Models.Participant;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class SchedulingConfirmationViewModel : ViewModelBase
    {

        #region Properties

        private bool _isBusy;
        private IParticipantManager _participantManager;
        private string _date;
        private string _time;
        private string _address;
        private EventParameter _selectedEvent;

        public bool IsBusy { set { SetPropertyValue(ref _isBusy, value); } get { return _isBusy; } }
        public string Date { get { return _date; } set { SetPropertyValue(ref _date, value); } }
        public string Time { get { return _time; } set { SetPropertyValue(ref _time, value); } }
        public string Address { get { return _address; } set { SetPropertyValue(ref _address, value); } }
        public ICommand ConfirmEventCommand { get; set; }
        public ICommand NoEditCommand { get; set; }

        #endregion

        public SchedulingConfirmationViewModel(IParticipantManager participantManager = null)
        {
            IsLoading = true;

            _participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();
            ConfirmEventCommand = new Command(async () => await ConfirmEvent());
            NoEditCommand = new Command(async () => await Edit());
        }

        public async override Task InitializeAsync(object parameter)
        {
            var selectedEvent = parameter as EventParameter;
            if (selectedEvent != null)
            {
                _selectedEvent = selectedEvent;
                Date = selectedEvent.EventDate.Date;
                Time = selectedEvent.EventTime.Start.ToString("hh:mmtt", CultureInfo.InvariantCulture).ToLowerInvariant();
                Address = selectedEvent.EventDate.FullAddress.Replace("<br/>", "\n");
            }

            IsLoading = false;
        }

        private async Task ConfirmEvent()
        {
            IsBusy = true;

            await TryExecute(async () =>
            {
                var appointment = await _participantManager.ConfirmEvent(_selectedEvent.EventDate.Id, _selectedEvent.EventTime.ID);
                await NavigateToStatusView();
            }, null, () => IsBusy = false);
        }

        private async Task NavigateToStatusView()
        {
            await TryExecute(async () =>
            {
                var status = await _participantManager.GetStatus();
                var viewModelType = DashboardHelper.GetViewModelForStatus(status);
                await NavigationService.NavigateToAsync(viewModelType, status, mainPage: true);
                DependencyContainer.Resolve<MenuViewModel>().UpdateMenuItem(viewModelType);
            });
        }

        private async Task Edit()
        {
            await TryExecute(async () =>
            {
                await NavigationService.PopAsync(2);
            });
        }
    }
}
