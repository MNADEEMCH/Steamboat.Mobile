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
using Steamboat.Mobile.UnitTest.TestHelpers;

namespace Steamboat.Mobile.UnitTest.ViewModels
{
    [TestFixture]
    public class StepperViewModelTest
    {
        
        [Test]
        public async Task StepperViewModel_Initialize_NullParam()
        {
            var stepperViewModel = new StepperViewModel();
            await stepperViewModel.InitializeAsync(null);

            Assert.AreEqual(0, stepperViewModel.InitializeExcecute);
        }

        [Test]
        public async Task StepperViewModel_GetSteps_3Steps()
        {
            var stepperViewModel = new StepperViewModel();
            var status = StatusHelper.CreateStatus();
            status.Dashboard.SurveyStep.Status = StatusEnum.None;
            var stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(3, stepperViewModel.Steps);
        }

        [Test]
        public async Task StepperViewModel_GetSteps_4Steps()
        {
            var stepperViewModel = new StepperViewModel();
            var status = StatusHelper.CreateStatus();
            status.Dashboard.SurveyStep.Status = StatusEnum.Pending;
            var stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(4, stepperViewModel.Steps);
        }


        [Test]
        public async Task StepperViewModel_GetCurrentStep_3StepsSchedulingAsCurrent()
        {
            var stepperViewModel = new StepperViewModel();
            var status = StatusHelper.Status_3Step_Scheduling();
            var stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(1, stepperViewModel.CurrentStep);
        }

        [Test]
        public async Task StepperViewModel_GetCurrentStep_3StepsScreeningAsCurrent()
        {
            var stepperViewModel = new StepperViewModel();
            var status = StatusHelper.Status_3Step_Screening();
            var stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(2, stepperViewModel.CurrentStep);
        }

        [Test]
        public async Task StepperViewModel_GetCurrentSteps_3StepsReportAsCurrent()
        {
            var stepperViewModel = new StepperViewModel();
            var status = StatusHelper.Status_3Step_Report();
            var stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(3, stepperViewModel.CurrentStep);
        }


        [Test]
        public async Task StepperViewModel_GetCurrentStep_4StepsInterviewAsCurrent()
        {
            var stepperViewModel = new StepperViewModel();
            var status = StatusHelper.Status_4Step_Interview();
            var stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(1, stepperViewModel.CurrentStep);
        }

        [Test]
        public async Task StepperViewModel_GetCurrentStep_4StepsSchedulingAsCurrent()
        {
            var stepperViewModel = new StepperViewModel();
            var status = StatusHelper.Status_4Step_Scheduling();
            var stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(2, stepperViewModel.CurrentStep);
        }

        [Test]
        public async Task StepperViewModel_GetCurrentStep_4StepsScreeningAsCurrent()
        {
            var stepperViewModel = new StepperViewModel();
            var status = StatusHelper.Status_4Step_Screening();
            var stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(3, stepperViewModel.CurrentStep);
        }

        [Test]
        public async Task StepperViewModel_GetCurrentSteps_4StepsReportAsCurrent()
        {
            var stepperViewModel = new StepperViewModel();
            var status = StatusHelper.Status_4Step_Report();
            var stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(4, stepperViewModel.CurrentStep);
        }

        [Test]
        public async Task StepperViewModel_GetCurrentSteps_4StepsAllStepsCompleted()
        {
            var stepperViewModel = new StepperViewModel();
            var status = StatusHelper.Status_4Step_AllCompleted();
            var stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(4, stepperViewModel.CurrentStep);
        }


        [Test]
        public async Task StepperViewModel_Refresh()
        {
            var stepperViewModel = new StepperViewModel();

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
            var stepperViewModel = new StepperViewModel();
            var status = StatusHelper.Status_4Step_Interview();
            var stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            status = StatusHelper.Status_4Step_Report();
            stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(1, stepperViewModel.PreviousStep);
            Assert.AreEqual(4, stepperViewModel.CurrentStep);

        }

        [Test]
        public async Task StepperViewModel_Initialize_AlreadyInitializedMoveBack()
        {
            var stepperViewModel = new StepperViewModel();
            var status = StatusHelper.Status_4Step_Report();
            var stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            status = StatusHelper.Status_4Step_Interview();
            stepperParam = DashboardHelper.GetStepperParameter(status);

            await stepperViewModel.InitializeAsync(stepperParam);

            Assert.AreEqual(4, stepperViewModel.PreviousStep);
            Assert.AreEqual(1, stepperViewModel.CurrentStep);

        }


        
    }
}
