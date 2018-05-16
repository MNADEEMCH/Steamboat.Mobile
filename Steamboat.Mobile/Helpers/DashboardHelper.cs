using System;
using System.Collections.Generic;
using Steamboat.Mobile.Models.Participant;
using Steamboat.Mobile.ViewModels;
using Steamboat.Mobile.Models.Stepper;
using Steamboat.Mobile.Models.Modal;
using Steamboat.Mobile.Models.Participant.DispositionSteps;
using System.Linq;

namespace Steamboat.Mobile.Helpers
{
    public class DashboardHelper
    {
        private const string _surveyStep = "Survey";
        private const string _schedulingStep = "Scheduling";
        private const string _screeningStep = "Screening";
        private const string _reportStep = "Report";

        public readonly static Dictionary<string, Type> StatusViewModelsDictionary = new Dictionary<string, Type>
        {
            { _surveyStep, typeof(InterviewViewModel)},
            { _schedulingStep , typeof(SchedulingViewModel)},
            { _screeningStep , typeof(ScreeningViewModel)},
            { _reportStep , typeof(ReportViewModel)}
        };

        public static string GetStepName(Type vmType)
        {
            var ret = StatusViewModelsDictionary.FirstOrDefault(x => x.Value.Equals(vmType)).Key;
            return ret.Equals(_surveyStep) ? "Interview" : ret;
        }

        public static Type GetViewModelForStatus(Status status)
        {
            if (!IsStatusValid(status))
                throw new Exception("Invalid status");

            string currentDispositionKey = GetCurrentDispositionKey(status);
            var viewModelType = StatusViewModelsDictionary[currentDispositionKey];

            return viewModelType;
        }

        public static DispositionStep GetDispositionStep(Status status)
        {
            if (!IsStatusValid(status))
                throw new Exception("Invalid status");

            DispositionStep dispositionStep = null;

            string currentDispositionKey = GetCurrentDispositionKey(status);

            switch (currentDispositionKey)
            {
                case _surveyStep:
                    dispositionStep = status.Dashboard.SurveyStep;
                    break;
                case _schedulingStep:
                    dispositionStep = status.Dashboard.SchedulingStep;
                    break;
                case _screeningStep:
                    dispositionStep = status.Dashboard.ScreeningStep;
                    break;
                default:
                    {
                        var reportReady = status.Dashboard.SurveyStep.Status.Equals(StatusEnum.Complete);
                        status.Dashboard.ReportStep.ReportReady = reportReady;
                        dispositionStep = status.Dashboard.ReportStep;
                        break;
                    }
            }

            return dispositionStep;
        }

        #region HandleStep

        public static StepperParam GetStepperParameter(Status status)
        {
            if (!IsStatusValid(status))
                throw new Exception("Invalid status");

            var stepperParam = new StepperParam();
            stepperParam.Steps = DashboardHelper.GetSteps(status);
            stepperParam.CurrentStep = DashboardHelper.GetCurrentStep(status, stepperParam.Steps);

            return stepperParam;
        }

        private static int GetCurrentStep(Status status, int steps)
        {
            int currentStep = -1;
            bool threeSteps = steps == 3;

            string currentDispositionKey = GetCurrentDispositionKey(status);

            switch (currentDispositionKey)
            {
                case _surveyStep:
                    currentStep = 1;
                    break;
                case _schedulingStep:
                    currentStep = threeSteps ? 1 : 2;
                    break;
                case _screeningStep:
                    currentStep = threeSteps ? 2 : 3;
                    break;
                case _reportStep:
                    currentStep = threeSteps ? 3 : 4;
                    break;
            }

            return currentStep;
        }

        private static int GetSteps(Status status)
        {
            return status.Dashboard.SurveyStep.Status.Equals(StatusEnum.None) ? 3 : 4;
        }

        #endregion

        private static string GetCurrentDispositionKey(Status status)
        {
            var dispositionKey = _reportStep;

            var surveyPending = status.Dashboard.SurveyStep.Status.Equals(StatusEnum.Pending);
            var schedulingPending = status.Dashboard.SchedulingStep.Status.Equals(StatusEnum.Pending);
            var screeningPending = status.Dashboard.ScreeningStep.Status.Equals(StatusEnum.Pending);
            var reportPending = status.Dashboard.ReportStep.Status.Equals(StatusEnum.Pending);

            if (surveyPending){
                if (reportPending)
                    dispositionKey = _reportStep;
                else
                    dispositionKey = _surveyStep;
            }
            else if (schedulingPending)
                dispositionKey = _schedulingStep;
            else if (screeningPending)
                dispositionKey = _screeningStep;

            return dispositionKey;
        }

        private static bool IsStatusValid(Status status)
        {
            return status != null && status.Dashboard != null &&
                status.Dashboard.SurveyStep != null &&
                status.Dashboard.SchedulingStep != null &&
                status.Dashboard.ScreeningStep != null &&
                status.Dashboard.ReportStep != null;
        }
    }
}
