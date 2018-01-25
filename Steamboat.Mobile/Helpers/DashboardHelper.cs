using System;
using System.Collections.Generic;
using Steamboat.Mobile.Models.Participant;
using Steamboat.Mobile.ViewModels;
using Steamboat.Mobile.Models.Stepper;
using Steamboat.Mobile.Models.Modal;
using Steamboat.Mobile.Models.Participant.DispositionSteps;

namespace Steamboat.Mobile.Helpers
{
    public class DashboardHelper
    {
        private const string surveyStep = "Survey";
        private const string schedulingStep = "Scheduling";
        private const string screeningStep = "Screening";
        private const string reportStep = "Report";

        public readonly static Dictionary<string, Type> StatusViewModelsDictionary = new Dictionary<string, Type>
        {
            { surveyStep, typeof(InterviewViewModel)},
            { schedulingStep , typeof(SchedulingViewModel)},
            { screeningStep , typeof(ScreeningViewModel)},
            { reportStep , typeof(ReportViewModel)}
        };

        private static string GetCurrentDispositionKey(Status status)
        {
            var dispositionKey = reportStep;

            if (status.Dashboard.SurveyStep.Status.Equals(StatusEnum.Pending))
                dispositionKey = surveyStep;
            else if (status.Dashboard.SchedulingStep.Status.Equals(StatusEnum.Pending))
                dispositionKey = schedulingStep;
            else if (status.Dashboard.ScreeningStep.Status.Equals(StatusEnum.Pending))
                dispositionKey = screeningStep;

            return dispositionKey;
        }

        public static Type GetViewModelForStatus(Status status)
        {
            string currentDispositionKey = GetCurrentDispositionKey(status);
            var viewModelType = StatusViewModelsDictionary[currentDispositionKey];

            return viewModelType;
        }

        public static DispositionStep GetDispositionStep(Status status)
        {
            DispositionStep dispositionStep = null;

            string currentDispositionKey = GetCurrentDispositionKey(status);

            switch (currentDispositionKey)
            {
                case surveyStep:
                    dispositionStep = status.Dashboard.SurveyStep;
                    break;
                case schedulingStep:
                    dispositionStep = status.Dashboard.SchedulingStep;
                    break;
                case screeningStep:
                    dispositionStep = status.Dashboard.ScreeningStep;
                    break;
                default: 
                    dispositionStep = status.Dashboard.ReportStep;
                    break;
            }
            return dispositionStep;
        }

        #region HandleStep

        public static StepperParam GetStepperParameter(Status status)
        {
            StepperParam stepperParam = null;
            if (DashboardHelper.ValidateDashboard(status))
            {
                stepperParam = new StepperParam();
                stepperParam.Steps = DashboardHelper.GetSteps(status);
                stepperParam.CurrentStep = DashboardHelper.GetCurrentStep(status, stepperParam.Steps);
            }
            return stepperParam;
        }

        private static int GetCurrentStep(Status status, int steps)
        {
            int currentStep = -1;
            bool threeSteps = steps == 3;

            string currentDispositionKey = GetCurrentDispositionKey(status);

            switch (currentDispositionKey)
            {
                case surveyStep:
                    currentStep = 1;
                    break;
                case schedulingStep:
                    currentStep = threeSteps ? 1 : 2;
                    break;
                case screeningStep:
                    currentStep = threeSteps ? 2 : 3;
                    break;
                case reportStep:
                    currentStep = threeSteps ? 3 : 4;
                    break;
            }

            return currentStep;
        }

        private static int GetSteps(Status status)
        {
            return status.Dashboard.SurveyStep.Status.Equals(StatusEnum.None) ? 3 : 4;
        }

        private static bool ValidateDashboard(Status status)
        {
            return status != null && status.Dashboard != null &&
                status.Dashboard.SurveyStep != null &&
                status.Dashboard.SchedulingStep != null &&
                status.Dashboard.ScreeningStep != null &&
                status.Dashboard.ReportStep != null;
        }

        #endregion
    }
}
