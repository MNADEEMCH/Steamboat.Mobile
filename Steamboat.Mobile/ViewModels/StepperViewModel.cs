using Steamboat.Mobile.Models.Participant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamboat.Mobile.ViewModels
{
    public class StepperViewModel : ViewModelBase
    {
        #region Properties

        private bool _readyToInitialize;
        public bool ReadyToInitialize { set { SetPropertyValue(ref _readyToInitialize, value); } get { return _readyToInitialize; } }

        private int _previousStep;
        public int PreviousStep { set { SetPropertyValue(ref _previousStep, value); } get { return _previousStep; } }

        private int _currentStep;
        public int CurrentStep { set { SetPropertyValue(ref _currentStep, value); } get { return _currentStep; } }

        private int _steps;
        public int Steps { set { SetPropertyValue(ref _steps, value); } get { return _steps; } }

        private bool _stepperInitialized = false;

        #endregion

        public override async Task InitializeAsync(object parameter)
        {
            ReadyToInitialize = false;
            Status status = parameter as Status;
            if (ValidateStatus(status))
            {
                if (!_stepperInitialized)
                {
                    Steps = GetSteps(status);
                    CurrentStep = GetCurrentStep(status, Steps);
                    PreviousStep = CurrentStep;
                    _stepperInitialized = true;
                }
                else
                {
                    PreviousStep = CurrentStep;
                    CurrentStep = GetCurrentStep(status, Steps);
                }

                ReadyToInitialize = true;
            }
            await base.InitializeAsync(true);
        }

        public override async Task Refresh()
        {
            await base.Refresh();

            PreviousStep = 0;
            CurrentStep = 0;
            Steps = 0;
            _stepperInitialized = false;
            ReadyToInitialize = false;

        }

        private bool ValidateStatus(Status status)
        {
            return status != null && status.Dashboard != null &&
                status.Dashboard.SurveyStep != null &&
                status.Dashboard.SchedulingStep != null &&
                status.Dashboard.ScreeningStep != null &&
                status.Dashboard.ReportStep != null;
        }

        private int GetSteps(Status status)
        {
            return status.Dashboard.SurveyStep.Status.Equals(StatusEnum.None) ? 3 : 4;
        }

        private int GetCurrentStep(Status status,int steps)
        {
            int currentStep = -1;
            bool threeSteps = steps == 3;

            if (status.Dashboard.SurveyStep.Status.Equals(StatusEnum.Pending))
                currentStep= 1;
            else if (status.Dashboard.SchedulingStep.Status.Equals(StatusEnum.Pending))
                currentStep= threeSteps? 1 : 2;
            else if (status.Dashboard.ScreeningStep.Status.Equals(StatusEnum.Pending))
                currentStep= threeSteps ? 2 : 3;
            else if (status.Dashboard.ReportStep.Status.Equals(StatusEnum.Pending))
                currentStep= threeSteps ? 3 : 4;
            else
                currentStep = threeSteps ? 3 : 4;

            return currentStep;
        }
    }
}
