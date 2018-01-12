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
    public class SchedulingViewModel : ViewModelBase
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
        #endregion

        public SchedulingViewModel()
        {
            IsLoading = true;
            LogoutCommand = new Command(async () => await Logout());
        }

        public async override Task InitializeAsync(object parameter)
        {
            Status status = parameter as Status;
            if (ValidateStatus(status))
            {
                SchedulingStep schedulingStep = status.Dashboard.SchedulingStep;
                StepperParam stepperParam = DashboardStatusHelper.GetStepperParameter(status);

                Title = schedulingStep.Title;
                Message = schedulingStep.Message;
                Steps = String.Format("STEP  {0}  OF  {1}", stepperParam.CurrentStep, stepperParam.Steps);
  
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
                    && status.Dashboard.SchedulingStep != null;
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
