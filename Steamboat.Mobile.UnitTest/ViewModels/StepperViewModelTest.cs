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

namespace Steamboat.Mobile.UnitTest.ViewModels
{
    [TestFixture]
    public class StepperViewModelTest
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
        public async Task StepperViewModel_Initialize_NullParam()
        {
            StepperViewModel stepperViewModel = new StepperViewModel();
            await stepperViewModel.InitializeAsync(null);

            Assert.AreEqual(0, stepperViewModel.InitializeExcecute);
        }

        [Test]
        public async Task StepperViewModel_GetSteps_3Steps()
        {
            StepperViewModel stepperViewModel = new StepperViewModel();

            Status status = new Status();
            status.Dashboard = CreateDashBoard();
            status.Dashboard.SurveyStep.Status = StatusEnum.None;
            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(3, stepperViewModel.Steps);
        }

        [Test]
        public async Task StepperViewModel_GetSteps_4Steps()
        {
            StepperViewModel stepperViewModel = new StepperViewModel();

            Status status = new Status();
            status.Dashboard = CreateDashBoard();
            status.Dashboard.SurveyStep.Status = StatusEnum.Pending;
            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(4, stepperViewModel.Steps);
        }


        [Test]
        public async Task StepperViewModel_GetCurrentStep_3StepsSchedulingAsCurrent()
        {
            StepperViewModel stepperViewModel = new StepperViewModel();

            Status status = new Status();
            status.Dashboard = CreateDashBoard();
            status.Dashboard.SurveyStep.Status = StatusEnum.None;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Pending;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Pending;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;
            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(1, stepperViewModel.CurrentStep);
        }

        [Test]
        public async Task StepperViewModel_GetCurrentStep_3StepsScreeningAsCurrent()
        {
            StepperViewModel stepperViewModel = new StepperViewModel();

            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.None;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Pending;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;
            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(2, stepperViewModel.CurrentStep);
        }

        [Test]
        public async Task StepperViewModel_GetCurrentSteps_3StepsReportAsCurrent()
        {
            StepperViewModel stepperViewModel = new StepperViewModel();

            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.None;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Complete;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;
            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(3, stepperViewModel.CurrentStep);
        }


        [Test]
        public async Task StepperViewModel_GetCurrentStep_4StepsInterviewAsCurrent()
        {
            StepperViewModel stepperViewModel = new StepperViewModel();

            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.Pending;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Pending;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Pending;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;
            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(1, stepperViewModel.CurrentStep);
        }

        [Test]
        public async Task StepperViewModel_GetCurrentStep_4StepsSchedulingAsCurrent()
        {
            StepperViewModel stepperViewModel = new StepperViewModel();

            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.Complete;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Pending;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Pending;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;
            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(2, stepperViewModel.CurrentStep);
        }

        [Test]
        public async Task StepperViewModel_GetCurrentStep_4StepsScreeningAsCurrent()
        {
            StepperViewModel stepperViewModel = new StepperViewModel();

            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.Complete;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Pending;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;
            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(3, stepperViewModel.CurrentStep);
        }

        [Test]
        public async Task StepperViewModel_GetCurrentSteps_4StepsReportAsCurrent()
        {
            StepperViewModel stepperViewModel = new StepperViewModel();

            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.Complete;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Complete;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;
            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(4, stepperViewModel.CurrentStep);
        }

        [Test]
        public async Task StepperViewModel_GetCurrentSteps_4StepsAllStepsCompleted()
        {
            StepperViewModel stepperViewModel = new StepperViewModel();

            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.Complete;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Complete;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;
            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(4, stepperViewModel.CurrentStep);
        }


        [Test]
        public async Task StepperViewModel_Refresh()
        {
            StepperViewModel stepperViewModel = new StepperViewModel();

            stepperViewModel.Steps = 4;
            stepperViewModel.PreviousStep = 3;
            stepperViewModel.CurrentStep = 4;
            stepperViewModel.InitializeExcecute = 1;

            await stepperViewModel.Refresh();

            Assert.AreEqual(0, stepperViewModel.Steps);
            Assert.AreEqual(0, stepperViewModel.PreviousStep);
            Assert.AreEqual(0, stepperViewModel.CurrentStep);
            Assert.AreEqual(0, stepperViewModel.InitializeExcecute);
        }

        [Test]
        public async Task StepperViewModel_Initialize_AlreadyInitializedMoveForwards()
        {
            StepperViewModel stepperViewModel = new StepperViewModel();

            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.Pending;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Pending;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Pending;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;
            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.Complete;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Complete;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;
            stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(1, stepperViewModel.PreviousStep);
            Assert.AreEqual(4, stepperViewModel.CurrentStep);

        }

        [Test]
        public async Task StepperViewModel_Initialize_AlreadyInitializedMoveBack()
        {
            StepperViewModel stepperViewModel = new StepperViewModel();

            Status status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.Complete;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Complete;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Complete;
            status.Dashboard.ReportStep.Status = StatusEnum.Complete;
            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            status = new Status();
            status.Dashboard = CreateDashBoard();

            status.Dashboard.SurveyStep.Status = StatusEnum.Pending;
            status.Dashboard.SchedulingStep.Status = StatusEnum.Pending;
            status.Dashboard.ScreeningStep.Status = StatusEnum.Pending;
            status.Dashboard.ReportStep.Status = StatusEnum.Pending;
            stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(4, stepperViewModel.PreviousStep);
            Assert.AreEqual(1, stepperViewModel.CurrentStep);

        }


        
    }
}
