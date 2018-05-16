using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Models.Modal;
using Steamboat.Mobile.Models.Participant;
using Steamboat.Mobile.Models.Stepper;
using Steamboat.Mobile.ViewModels.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Steamboat.Mobile.Models.Participant.DispositionSteps;
using System.Globalization;
using Steamboat.Mobile.Models.NavigationParameters;

namespace Steamboat.Mobile.ViewModels
{
    public class ScreeningViewModel : DispositionViewModelBase
    {
        #region Properties

        private bool _showDetails;
        private string _date;
        private string _time;
        private string _address;
        private string _eventId;
        private string _eventTimeId;
        private string _eventStartTime;

        public bool ShowDetails { get { return _showDetails; } set { SetPropertyValue(ref _showDetails, value); } }
        public string Date { get { return _date; } set { SetPropertyValue(ref _date, value); } }
        public string Time { get { return _time; } set { SetPropertyValue(ref _time, value); } }
        public string Address { get { return _address; } set { SetPropertyValue(ref _address, value); } }
        public ICommand RescheduleCommand { get; set; }

        #endregion

        public ScreeningViewModel(StepperViewModel stepperViewModel = null) : base(stepperViewModel)
        {
            IsLoading = true;
            IconSource = "icScreening.png";
            RescheduleCommand = new Command(async () => await Reschedule());
        }

        protected override void InitializeSpecificStep(Status status)
        {
            ScreeningStep screeningStep = status.Dashboard.ScreeningStep;

            ShowDetails = screeningStep.Detail != null;
            if (ShowDetails)
            {
                Date = screeningStep.Detail.Timeslot.Date.ToString("dddd, MMMM d", CultureInfo.InvariantCulture);
                Time = screeningStep.Detail.Timeslot.Time;
                Address = screeningStep.Detail.Timeslot.Location.Replace("<br/>", "\n");
                _eventTimeId = screeningStep.Detail.Timeslot.ID;
                _eventId = screeningStep.Detail.Timeslot.EventID;
                _eventStartTime = screeningStep.Detail.Timeslot.Time;
            }
        }

        private async Task Reschedule()
        {
            await TryExecute(async () =>
            {
                if (_eventId != null && _eventTimeId != null)
                {
                    var navigationParameter = new EventParameter();
                    navigationParameter.RescheduleEvent = new RescheduleEvent()
                    {
                        RescheduleEventId = _eventId,
                        RescheduleTimeId = _eventTimeId,
                        RescheduleStartTime = _eventStartTime
                    };

                    await NavigationService.NavigateToAsync<SchedulingEventDateViewModel>(navigationParameter);
                }
                else
                {
                    await DialogService.ShowAlertAsync("Reschedule not available", "Error", "OK");
                }
            });              
        }

        protected override async Task MainAction()
        {

        }

    }
}
