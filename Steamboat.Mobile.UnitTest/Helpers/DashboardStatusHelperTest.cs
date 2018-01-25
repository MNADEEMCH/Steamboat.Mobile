using NUnit.Framework;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Models.Participant;
using Steamboat.Mobile.Models.Stepper;
using Steamboat.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.Participant.DispositionSteps;

namespace Steamboat.Mobile.UnitTest.Helpers
{
    [TestFixture]
    public class DashboardStatusHelperTest
    {
        private Dashboard CreateDashBoard()
        {
            Dashboard dashboard = new Dashboard();
            dashboard.SurveyStep = new SurveyStep();
            dashboard.SchedulingStep = new SchedulingStep();
            dashboard.ScreeningStep = new ScreeningStep();
            dashboard.ReportStep = new ReportStep();

            return dashboard;
        }

        [Test]
        public void DashboardStatusHelper_GetViewModelForStatus_3StepsSchedulingViewModel()
        {

            Status status = new Status();
            status.Dashboard = CreateDashBoard();
            status.Dashboard.SurveyStep.Status = StatusEnum.None;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Pending;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Pending;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;

            Type viewModelType = DashboardHelper.GetViewModelForStatus(status);

            Assert.AreEqual(typeof(SchedulingViewModel), viewModelType);
        }

        [Test]
        public void DashboardStatusHelper_GetViewModelForStatus_3StepsScreeningViewModel()
        {
            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.None;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Pending;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;

            Type viewModelType = DashboardHelper.GetViewModelForStatus(status);

            Assert.AreEqual(typeof(ScreeningViewModel), viewModelType);
        }

        [Test]
        public void DashboardStatusHelper_GetViewModelForStatus_3StepsReportViewModel()
        {
            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.None;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Complete;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;

            Type viewModelType = DashboardHelper.GetViewModelForStatus(status);

            Assert.AreEqual(typeof(ReportViewModel), viewModelType);
        }

        [Test]
        public void DashboardStatusHelper_GetViewModelForStatus_3StepsAllStepsCompleted()
        {

            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.None;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Complete;
            status.Dashboard.ReportStep.Status = StatusEnum.Complete;

            Type viewModelType = DashboardHelper.GetViewModelForStatus(status);

            Assert.AreEqual(typeof(ReportViewModel), viewModelType);
        }


        [Test]
        public void DashboardStatusHelper_GetViewModelForStatus_4StepsInterviewViewModel()
        {
            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.Pending;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Pending;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Pending;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;

            Type viewModelType = DashboardHelper.GetViewModelForStatus(status);

            Assert.AreEqual(typeof(InterviewViewModel), viewModelType);
        }

        [Test]
        public void DashboardStatusHelper_GetViewModelForStatus_4StepsSchedulingViewModel()
        {
            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.Complete;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Pending;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Pending;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;

            Type viewModelType = DashboardHelper.GetViewModelForStatus(status);

            Assert.AreEqual(typeof(SchedulingViewModel), viewModelType);
        }

        [Test]
        public void DashboardStatusHelper_GetViewModelForStatus_4StepsScreeningViewModel()
        {
            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.Complete;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Pending;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;

            Type viewModelType = DashboardHelper.GetViewModelForStatus(status);

            Assert.AreEqual(typeof(ScreeningViewModel), viewModelType);
        }

        [Test]
        public void DashboardStatusHelper_GetViewModelForStatus_4StepsReportViewModel()
        {
            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.Complete;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Complete;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;

            Type viewModelType = DashboardHelper.GetViewModelForStatus(status);

            Assert.AreEqual(typeof(ReportViewModel), viewModelType);
        }

        [Test]
        public void DashboardStatusHelper_GetViewModelForStatus_4StepsAllStepsCompleted()
        {

            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.Complete;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Complete;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;

            Type viewModelType = DashboardHelper.GetViewModelForStatus(status);

            Assert.AreEqual(typeof(ReportViewModel), viewModelType);
        }


        [Test]
        public void DashboardStatusHelper_GetSteps_3Steps()
        {
           
            Status status = new Status();
            status.Dashboard = CreateDashBoard();
            status.Dashboard.SurveyStep.Status = StatusEnum.None;

            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);

            Assert.AreEqual(3, stepperParam.Steps);
        }

        [Test]
        public void DashboardStatusHelper_GetSteps_4Steps()
        {
            Status status = new Status();
            status.Dashboard = CreateDashBoard();
            status.Dashboard.SurveyStep.Status = StatusEnum.Pending;

            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);

            Assert.AreEqual(4, stepperParam.Steps);
        }


        [Test]
        public void DashboardStatusHelper_GetCurrentStep_3StepsSchedulingAsCurrent()
        {

            Status status = new Status();
            status.Dashboard = CreateDashBoard();
            status.Dashboard.SurveyStep.Status = StatusEnum.None;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Pending;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Pending;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;

            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);

            Assert.AreEqual(1, stepperParam.CurrentStep);
        }

        [Test]
        public void DashboardStatusHelper_GetCurrentStep_3StepsScreeningAsCurrent()
        {
            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.None;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Pending;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;

            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);

            Assert.AreEqual(2, stepperParam.CurrentStep);
        }

        [Test]
        public void DashboardStatusHelper_GetCurrentSteps_3StepsReportAsCurrent()
        {
            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.None;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Complete;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;

            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);

            Assert.AreEqual(3, stepperParam.CurrentStep);
        }


        [Test]
        public void DashboardStatusHelper_GetCurrentStep_4StepsInterviewAsCurrent()
        {
            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.Pending;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Pending;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Pending;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;

            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);

            Assert.AreEqual(1, stepperParam.CurrentStep);
        }

        [Test]
        public void DashboardStatusHelper_GetCurrentStep_4StepsSchedulingAsCurrent()
        {
            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.Complete;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Pending;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Pending;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;

            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);

            Assert.AreEqual(2, stepperParam.CurrentStep);
        }

        [Test]
        public void DashboardStatusHelper_GetCurrentStep_4StepsScreeningAsCurrent()
        {
            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.Complete;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Pending;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;

            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);

            Assert.AreEqual(3, stepperParam.CurrentStep);
        }

        [Test]
        public void DashboardStatusHelper_GetCurrentSteps_4StepsReportAsCurrent()
        {
            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.Complete;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Complete;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;

            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);

            Assert.AreEqual(4, stepperParam.CurrentStep);
        }

        [Test]
        public void DashboardStatusHelper_GetCurrentSteps_4StepsAllStepsCompleted()
        {
            
            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.Complete;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Complete;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;

            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);

            Assert.AreEqual(4, stepperParam.CurrentStep);
        }


    }
}
