using Steamboat.Mobile.Models.Participant;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Steamboat.Mobile.Managers.Participant;
using Xamarin.Forms;
using System.Windows.Input;
using Steamboat.Mobile.Models.NavigationParameters;

namespace Steamboat.Mobile.ViewModels
{
    public class SchedulingEventDateViewModel : ViewModelBase
    {
        #region Properties

        private IParticipantManager _participantManager;
        private EventDate _prevEventListItemSelected;
        private ObservableCollection<EventDate> _schedulingEventDate;

        public ObservableCollection<EventDate> SchedulingEventDate { get { return _schedulingEventDate; } set { SetPropertyValue(ref _schedulingEventDate, value); } }
        public ICommand CommandEventSelected { get; set; }

        #endregion

        public SchedulingEventDateViewModel(IParticipantManager participantManager = null)
        {
            IsLoading = true;

            _participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();
            CommandEventSelected = new Command(async (selectedevent) => await EventSelected(selectedevent));
        }

        private async Task EventSelected(object selectedevent)
        {
            var SelectedEvent = selectedevent as EventDate;
            if (SelectedEvent != null)
            {
                if (_prevEventListItemSelected != null)
                    _prevEventListItemSelected.IsActive = false;

                SelectedEvent.IsActive = true;
                _prevEventListItemSelected = SelectedEvent;

                var navigationParameter = new EventParameter();
                navigationParameter.EventDate = SelectedEvent;
                await NavigationService.NavigateToAsync<SchedulingTimeViewModel>(navigationParameter);
            }
        }

        public async override Task InitializeAsync(object parameter)
        {
            try
            {
                await LoadEvents();
            }
            catch (Exception ex)
            {
                await DialogService.ShowAlertAsync(ex.Message, "Error", "OK");
            }
            finally
            { 
                IsLoading = false; 
            }
        }

        private async Task LoadEvents()
        {
            var events = await _participantManager.GetEvents();

            SchedulingEventDate = new ObservableCollection<EventDate>();
            foreach (Event e in events)
            {
                var eventToAdd = ConvertEventToEventListItem(e);
                SchedulingEventDate.Add(eventToAdd);
            }
        }

        private EventDate ConvertEventToEventListItem(Event e)
        {
            return new EventDate()
            {
                Id = e.ID,
                FullAddress = e.FullAddress.Replace("<br/>", "\n"),
                Distance = String.Format("{0}, miles", e.Distance.ToString("0.0")),
                Date = e.Start.ToString("dddd, MMMM d", CultureInfo.InvariantCulture),
                Time = String.Format("{0} to {1}", e.Start.ToString("h:mm tt", CultureInfo.InvariantCulture).ToLower(), 
                                     e.Finish.ToString("h:mm tt", CultureInfo.InvariantCulture).ToLower())
            };
        }


    }
}
