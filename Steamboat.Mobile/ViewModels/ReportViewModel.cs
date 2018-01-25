﻿using Steamboat.Mobile.Helpers;
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
        public bool ReportReady
        {
            get { return _reportReady; }
            set { SetPropertyValue(ref _reportReady, value); }
        }

        public string _goButtonText;
        public string GoButtonText
        {
            get { return _goButtonText; }
            set { SetPropertyValue(ref _goButtonText, value); }
        }
        #endregion

        public ReportViewModel(StepperViewModel stepperViewModel = null) : base(stepperViewModel)
        {
            IsLoading = true;
            IconSource= "icReport.png";
            LogoutCommand = new Command(async () => await Logout());
            MoreInfoCommand = new Command(async () => await MoreInfo());
        }

        protected override void InitializeSpecificStep(Status status)
        {
            ReportStep reportStep = status.Dashboard.ReportStep;
            ReportReady = reportStep.Status.Equals(StatusEnum.Complete);
            GoButtonText = ReportReady ? "VIEW REPORT" : "RESUME";
        }

    }
}
