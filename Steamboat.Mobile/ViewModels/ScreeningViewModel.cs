using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Models.Participant;
using Steamboat.Mobile.Models.Stepper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class ScreeningViewModel : ViewModelBase
    {
        #region Properties

        public ICommand LogoutCommand { get; set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetPropertyValue(ref _title, value); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetPropertyValue(ref _message, value); }
        }

        private string _steps;
        public string Steps
        {
            get { return _steps; }
            set { SetPropertyValue(ref _steps, value); }
        }

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

        public ScreeningViewModel()
        {
            IsLoading = true;
            LogoutCommand = new Command(async () => await Logout());
        }

        public async override Task InitializeAsync(object parameter)
        {
            Status status = parameter as Status;
            if (ValidateStatus(status))
            {
                ScreeningStep screeningStep = status.Dashboard.ScreeningStep;
                StepperParam stepperParam = DashboardStatusHelper.GetStepperParameter(status);

                Title = screeningStep.Title;
                Message = screeningStep.Message;
                Steps = String.Format("STEP  {0}  OF  {1}", stepperParam.CurrentStep, stepperParam.Steps);
                ShowDetails = screeningStep.Detail != null;
                if (ShowDetails) { 
                    Date = screeningStep.Detail.Timeslot.Date;
                    Time = screeningStep.Detail.Timeslot.Time;
                    Address = screeningStep.Detail.Timeslot.Location.Replace("<br/>","\n");
                }
                await DependencyContainer.Resolve<StepperViewModel>().InitializeAsync(stepperParam);
            }
            else
            {
                //TODO: Improve handle error
                await this.DialogService.ShowAlertAsync("Error loading", "Error", "OK");
            }
            IsLoading = false;
        }

        private bool ValidateStatus(Status status)
        {
            return status != null
                    && status.Dashboard != null
                    && status.Dashboard.ScreeningStep != null;
        }

        private async Task Logout()
        {
            IsLoading = true;

            try
            {
                await NavigationService.NavigateToAsync<LoginViewModel>("Logout");
            }
            catch (Exception e)
            {
                await DialogService.ShowAlertAsync(e.Message, "Error", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

    }
}
