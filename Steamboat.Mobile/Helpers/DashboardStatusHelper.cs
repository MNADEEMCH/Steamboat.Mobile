using System;
using System.Collections.Generic;
using Steamboat.Mobile.Models.Participant;
using Steamboat.Mobile.ViewModels;
using Steamboat.Mobile.Models.Stepper;

namespace Steamboat.Mobile.Helpers
{
    public class DashboardStatusHelper
    {
        public readonly static Dictionary<string, Type> StatusViewModelsDictionary = new Dictionary<string, Type>
        {
            { "Survey" , typeof(InterviewViewModel)},
            { "Scheduling" , typeof(SchedulingViewModel)},
            { "Screening" , typeof(ScreeningViewModel)},
            { "Report" , typeof(ReportViewModel)}
        };

        public static Type GetViewModelForStatus(Status status)
        {
            Type viewModelType = null;

            if (status.Dashboard.SurveyStep.Status.Equals(StatusEnum.Pending))
                viewModelType = StatusViewModelsDictionary["Survey"];
            else if (status.Dashboard.SchedulingStep.Status.Equals(StatusEnum.Pending))
                viewModelType = StatusViewModelsDictionary["Scheduling"];
            else if (status.Dashboard.ScreeningStep.Status.Equals(StatusEnum.Pending))
                viewModelType = StatusViewModelsDictionary["Screening"];
            else if (status.Dashboard.ReportStep.Status.Equals(StatusEnum.Pending))
                viewModelType = StatusViewModelsDictionary["Report"];
            else
                viewModelType = StatusViewModelsDictionary["Report"];

            return viewModelType;
        }

        public static StepperParam GetStepperParameter(Status status)
        {
            StepperParam stepperParam = null;
            if (DashboardStatusHelper.ValidateDashboard(status))
            {
                stepperParam = new StepperParam();
                stepperParam.Steps = DashboardStatusHelper.GetSteps(status);
                stepperParam.CurrentStep = DashboardStatusHelper.GetCurrentStep(status, stepperParam.Steps);
            }
            return stepperParam;
        }

        private static int GetCurrentStep(Status status, int steps)
        {
            int currentStep = -1;
            bool threeSteps = steps == 3;

            if (status.Dashboard.SurveyStep.Status.Equals(StatusEnum.Pending))
                currentStep = 1;
            else if (status.Dashboard.SchedulingStep.Status.Equals(StatusEnum.Pending))
                currentStep = threeSteps ? 1 : 2;
            else if (status.Dashboard.ScreeningStep.Status.Equals(StatusEnum.Pending))
                currentStep = threeSteps ? 2 : 3;
            else if (status.Dashboard.ReportStep.Status.Equals(StatusEnum.Pending))
                currentStep = threeSteps ? 3 : 4;
            else
                currentStep = threeSteps ? 3 : 4;

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
    }
}
