using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
                await LoadEventTimeslots(selectedEvent);
            }

            IsLoading = false;
        }

        private async Task LoadEventTimeslots(EventParameter selectedEvent)
        {
            _eventParameter = selectedEvent;
            EventDate = _eventParameter.EventDate.Date;
            var eventId = selectedEvent.EventDate.Id;
            var list = await _participantManager.GetEventTimes(eventId);

            if (_eventParameter.RescheduleEvent != null)
            {
                SetSelectedItem(selectedEvent, list);
            }

            EventTimeList = EventTimeHelper.GetList(list);
        }

        private void SetSelectedItem(EventParameter selectedEvent, List<EventTime> list)
        {
            var selectedElement = list.FirstOrDefault(t => t.ID.ToString() == _eventParameter.RescheduleEvent.RescheduleTimeId
                                || CompareStartTimesDifferentEvents(t.Start, _eventParameter.RescheduleEvent.RescheduleStartTime,
                                                                    selectedEvent.EventDate.Id, selectedEvent.RescheduleEvent.RescheduleEventId));
            if (selectedElement != null)
            {
                selectedElement.IsActive = true;
                selectedElement.IsAvailable = false;
                _prevEventTime = selectedElement;
            }
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

        private bool CompareStartTimesDifferentEvents(DateTime start, string rescheduleStartTime, int selectedId, string rescheduleId)
        {
            return DateTime.Parse(rescheduleStartTime).TimeOfDay.Equals(start.TimeOfDay) && selectedId.ToString().Equals(rescheduleId);
        }
    }
}
