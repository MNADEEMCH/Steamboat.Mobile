using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Managers.Participant;
using Steamboat.Mobile.Models.NavigationParameters;
using Steamboat.Mobile.Models.Participant;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class SchedulingTimeViewModel : ViewModelBase
    {
        #region Properties

        private bool _buttonEnabled;
        private string _eventDate;
        private ObservableCollection<ObservableCollection<EventTime>> _eventTimeList;
        private IParticipantManager _participantManager;
        private EventTime _prevEventTime;
        private EventParameter _eventParameter;

        public ICommand SelectTimeCommand { get; set; }
        public bool ButtonEnabled { set { SetPropertyValue(ref _buttonEnabled, value); } get { return _buttonEnabled; } }
        public string EventDate { set { SetPropertyValue(ref _eventDate, value); } get { return _eventDate; } }
        public ObservableCollection<ObservableCollection<EventTime>> EventTimeList { set { SetPropertyValue(ref _eventTimeList, value); } get { return _eventTimeList; } }

        #endregion

        public SchedulingTimeViewModel(IParticipantManager participantManager = null)
        {
            IsLoading = true;

            _participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();
            SelectTimeCommand = new Command(async (param) => await SelectTime(param));
        }

        public async override Task InitializeAsync(object parameter)
        {
            var selectedEvent = parameter as EventParameter;
            if (selectedEvent != null)
            {
                _eventParameter = selectedEvent;
                EventDate = _eventParameter.EventDate.Date;
                var eventId = selectedEvent.EventDate.Id;
                var list = await _participantManager.GetEventTimes(eventId);
                EventTimeList = EventTimeHelper.GetList(list);
            }

            IsLoading = false;
        }

        private async Task SelectTime(object param)
        {
            var selected = param as EventTime;
            if (selected != null)
            {
                if (_prevEventTime != null)
                {
                    _prevEventTime.IsActive = false;
                }
                selected.IsActive = true;
                _prevEventTime = selected;
                _eventParameter.EventTime = selected;
                await NavigationService.NavigateToAsync<SchedulingConfirmationViewModel>(_eventParameter);
            }
        }
    }
}
