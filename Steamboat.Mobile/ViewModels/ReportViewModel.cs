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
    public class ReportViewModel: DispositionViewModelBase
    {
        #region Properties
        private bool _reportReady;
        private string _mainActionButtonText;

        public bool ReportReady{get { return _reportReady; }set { SetPropertyValue(ref _reportReady, value); }}

        public string MainActionButtonText{get { return _mainActionButtonText; }set { SetPropertyValue(ref _mainActionButtonText, value); }}
        #endregion

        public ReportViewModel(StepperViewModel stepperViewModel = null) : base(stepperViewModel)
        {
            IsLoading = true;
            IconSource= "icReport.png";
        }

        protected override void InitializeSpecificStep(Status status)
        {
            ReportStep reportStep = status.Dashboard.ReportStep;
            //TODO: Remove this hardcoded value
            ReportReady = true;//reportStep.Status.Equals(StatusEnum.Complete);
            MainActionButtonText = ReportReady ? "VIEW REPORT" : "RESUME";
        }

        protected override async Task MainAction(){
            
            if (ReportReady)
                await NavigationService.NavigateToAsync<ReportDetailsViewModel>();

        }

    }
}
