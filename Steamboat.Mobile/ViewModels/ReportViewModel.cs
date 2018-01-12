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
    public class ReportViewModel:ViewModelBase
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

        private string _detailPending;
        public string DetailPending
        {
            get { return _detailPending; }
            set { SetPropertyValue(ref _detailPending, value); }
        }

        private bool _reportReady;
        public bool ReportReady
        {
            get { return _reportReady; }
            set { SetPropertyValue(ref _reportReady, value); }
        }

        private string _steps;
        public string Steps
        {
            get { return _steps; }
            set { SetPropertyValue(ref _steps, value); }
        }
        #endregion

        public ReportViewModel()
        {
            IsLoading = true;
            LogoutCommand = new Command(async () => await Logout());
        }

        public async override Task InitializeAsync(object parameter)
        {
            Status status = parameter as Status;
            if (ValidateStatus(status))
            {
                ReportStep reportStep = status.Dashboard.ReportStep;
                StepperParam stepperParam = DashboardStatusHelper.GetStepperParameter(status);

                Title = reportStep.Title;
                Message = reportStep.Message;
                DetailPending = "We'll let you know as soon as its ready!";
                Steps = String.Format("STEP  {0}  OF  {1}", stepperParam.CurrentStep, stepperParam.Steps);
                ReportReady = status.Dashboard.ReportStep.Status.Equals(StatusEnum.Complete);
                
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
                    && status.Dashboard.ReportStep != null;
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
