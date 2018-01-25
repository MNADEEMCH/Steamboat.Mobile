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

namespace Steamboat.Mobile.ViewModels
{
    public class ScreeningViewModel : DispositionViewModelBase
    {
        #region Properties
        private bool _showDetails;
        public bool ShowDetails
        {
            get { return _showDetails; }
            set { SetPropertyValue(ref _showDetails, value); }
        }

        private string _date;
        public string Date
        {
            get { return _date; }
            set { SetPropertyValue(ref _date, value); }
        }

        private string _time;
        public string Time
        {
            get { return _time; }
            set { SetPropertyValue(ref _time, value); }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { SetPropertyValue(ref _address, value); }
        }
        #endregion

        public ScreeningViewModel(StepperViewModel stepperViewModel = null):base(stepperViewModel)
        {
            IsLoading = true;
            IconSource = "icScreening.png";
            LogoutCommand = new Command(async () => await Logout());
            MoreInfoCommand = new Command(async () => await MoreInfo());
        }

        protected override void InitializeSpecificStep(Status status)
        {
            ScreeningStep screeningStep = status.Dashboard.ScreeningStep;

            ShowDetails = screeningStep.Detail != null;
            if (ShowDetails)
            {
                Date = screeningStep.Detail.Timeslot.Date;
                Time = screeningStep.Detail.Timeslot.Time;
                Address = screeningStep.Detail.Timeslot.Location.Replace("<br/>", "\n");
            }
        }

    }
}
