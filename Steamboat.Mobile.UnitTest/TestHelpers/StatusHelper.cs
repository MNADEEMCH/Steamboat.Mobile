using System;
using Steamboat.Mobile.Models.Participant;
using Steamboat.Mobile.Models.Participant.DispositionSteps;

namespace Steamboat.Mobile.UnitTest.TestHelpers
{
    public class StatusHelper
    {
        public static Status CreateStatus()
        {
            var status = new Status();
            status.Dashboard = new Dashboard();
            var dashboard = status.Dashboard;
            dashboard.SurveyStep = new SurveyStep();
            dashboard.SchedulingStep = new SchedulingStep();
            dashboard.ScreeningStep = new ScreeningStep();
            dashboard.ReportStep = new ReportStep();

            return status;
        }

        public static Status CreateWrongStatus()
        {

            var status = new Status();

            return status;
        }

        public static Status Status_3Step_Scheduling()
        {

            var status = CreateStatus();
            status.Dashboard.SurveyStep.Status = StatusEnum.None;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Pending;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Disabled;
            status.Dashboard.ReportStep.Status = StatusEnum.Disabled;

            return status;
        }
        public static Status Status_3Step_Screening()
        {
            var status = CreateStatus();
            status.Dashboard.SurveyStep.Status = StatusEnum.None;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Pending;
            status.Dashboard.ReportStep.Status = StatusEnum.Disabled;

            return status;
        }
        public static Status Status_3Step_Report()
        {
            var status = CreateStatus();
            status.Dashboard.SurveyStep.Status = StatusEnum.None;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Complete;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;

            return status;
        }
        public static Status Status_3Step_AllCompleted()
        {
            var status = CreateStatus();
            status.Dashboard.SurveyStep.Status = StatusEnum.None;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Complete;
            status.Dashboard.ReportStep.Status = StatusEnum.Complete;

            return status;
        }

        public static Status Status_4Step_Interview()
        {

            var status = CreateStatus();
            status.Dashboard.SurveyStep.Status = StatusEnum.Pending;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Disabled;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Disabled;
            status.Dashboard.ReportStep.Status = StatusEnum.Disabled;

            return status;
        }
        public static Status Status_4Step_Scheduling()
        {

            var status = CreateStatus();
            status.Dashboard.SurveyStep.Status = StatusEnum.Complete;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Pending;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Disabled;
            status.Dashboard.ReportStep.Status = StatusEnum.Disabled;

            return status;
        }
        public static Status Status_4Step_Screening()
        {
            var status = CreateStatus();
            status.Dashboard.SurveyStep.Status = StatusEnum.Complete;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Pending;
            status.Dashboard.ReportStep.Status = StatusEnum.Disabled;

            return status;
        }
        public static Status Status_4Step_Report()
        {
            var status = CreateStatus();
            status.Dashboard.SurveyStep.Status = StatusEnum.Complete;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Complete;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;

            return status;
        }
        public static Status Status_4Step_Report_ParticularScenario(){
            
            var status = CreateStatus();
            status.Dashboard.SurveyStep.Status = StatusEnum.Pending;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Complete;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;

            return status;
        }
        public static Status Status_4Step_AllCompleted()
        {
            var status = CreateStatus();
            status.Dashboard.SurveyStep.Status = StatusEnum.Complete;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Complete;
            status.Dashboard.ReportStep.Status = StatusEnum.Complete;

            return status;
        }

    }
}
