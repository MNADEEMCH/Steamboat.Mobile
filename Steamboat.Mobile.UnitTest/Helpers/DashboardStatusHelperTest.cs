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

namespace Steamboat.Mobile.UnitTest.Helpers
{
    [TestFixture]
    public class DashboardStatusHelperTest
    {
        #region GetViewModelForStatus

        [Test]
        public void DashboardStatusHelper_GetViewModelForStatus_WrongStatus()
        {

            var status = StatusHelper.CreateWrongStatus();

            Assert.Throws<Exception>(() => DashboardHelper.GetViewModelForStatus(status));
        }

        [Test]
        public void DashboardStatusHelper_GetViewModelForStatus_3StepsSchedulingViewModel()
        {

            var status = StatusHelper.Status_3Step_Scheduling();

            var viewModelType = DashboardHelper.GetViewModelForStatus(status);

            Assert.AreEqual(typeof(SchedulingViewModel), viewModelType);
        }

        [Test]
        public void DashboardStatusHelper_GetViewModelForStatus_3StepsScreeningViewModel()
        {
            var status = StatusHelper.Status_3Step_Screening();

            var viewModelType = DashboardHelper.GetViewModelForStatus(status);

            Assert.AreEqual(typeof(ScreeningViewModel), viewModelType);
        }

        [Test]
        public void DashboardStatusHelper_GetViewModelForStatus_3StepsReportViewModel()
        {
            var status = StatusHelper.Status_3Step_Report();

            var viewModelType = DashboardHelper.GetViewModelForStatus(status);

            Assert.AreEqual(typeof(ReportViewModel), viewModelType);
        }

        [Test]
        public void DashboardStatusHelper_GetViewModelForStatus_3StepsAllStepsCompleted()
        {

            var status = StatusHelper.Status_3Step_AllCompleted();

            var viewModelType = DashboardHelper.GetViewModelForStatus(status);

            Assert.AreEqual(typeof(ReportViewModel), viewModelType);
        }


        [Test]
        public void DashboardStatusHelper_GetViewModelForStatus_4StepsInterviewViewModel()
        {
            var status = StatusHelper.Status_4Step_Interview();

            var viewModelType = DashboardHelper.GetViewModelForStatus(status);

            Assert.AreEqual(typeof(InterviewViewModel), viewModelType);
        }

        [Test]
        public void DashboardStatusHelper_GetViewModelForStatus_4StepsSchedulingViewModel()
        {
            var status = StatusHelper.Status_4Step_Scheduling();

            var viewModelType = DashboardHelper.GetViewModelForStatus(status);

            Assert.AreEqual(typeof(SchedulingViewModel), viewModelType);
        }

        [Test]
        public void DashboardStatusHelper_GetViewModelForStatus_4StepsScreeningViewModel()
        {
            var status = StatusHelper.Status_4Step_Screening();

            var viewModelType = DashboardHelper.GetViewModelForStatus(status);

            Assert.AreEqual(typeof(ScreeningViewModel), viewModelType);
        }

        [Test]
        public void DashboardStatusHelper_GetViewModelForStatus_4StepsReportViewModel()
        {
            var status = StatusHelper.Status_4Step_Report();

            var viewModelType = DashboardHelper.GetViewModelForStatus(status);

            Assert.AreEqual(typeof(ReportViewModel), viewModelType);
        }

        [Test]
        public void DashboardStatusHelper_GetViewModelForStatus_4StepsReportParticularScenario()
        {
            var status = StatusHelper.Status_4Step_Report_ParticularScenario();

            var viewModelType = DashboardHelper.GetViewModelForStatus(status);

            Assert.AreEqual(typeof(ReportViewModel), viewModelType);
        }

        [Test]
        public void DashboardStatusHelper_GetViewModelForStatus_4StepsAllStepsCompleted()
        {

            var status = StatusHelper.Status_4Step_AllCompleted();

            var viewModelType = DashboardHelper.GetViewModelForStatus(status);

            Assert.AreEqual(typeof(ReportViewModel), viewModelType);
        }

        #endregion

        #region GetDispositionStep

        [Test]
        public void DashboardStatusHelper_GetDispositionStep_WrongStatus()
        {

            var status = StatusHelper.CreateWrongStatus();

            Assert.Throws<Exception>(() => DashboardHelper.GetDispositionStep(status));
        }

        [Test]
        public void DashboardStatusHelper_GetDispositionStep_3StepsSchedulingDispositionStep()
        {

            var status = StatusHelper.Status_3Step_Scheduling();

            var dispositionStep = DashboardHelper.GetDispositionStep(status);

            Assert.IsInstanceOf(typeof(SchedulingStep), dispositionStep);
        }

        [Test]
        public void DashboardStatusHelper_GetDispositionStep_3StepsScreeningDispositionStep()
        {
            var status = StatusHelper.Status_3Step_Screening();

            var dispositionStep = DashboardHelper.GetDispositionStep(status);

            Assert.IsInstanceOf(typeof(ScreeningStep), dispositionStep);
        }

        [Test]
        public void DashboardStatusHelper_GetDispositionStep_3StepsReportDispositionStep()
        {
            var status = StatusHelper.Status_3Step_Report();

            var dispositionStep = DashboardHelper.GetDispositionStep(status);

            Assert.IsInstanceOf(typeof(ReportStep), dispositionStep);
        }

        [Test]
        public void DashboardStatusHelper_GetDispositionStep_3StepsAllStepsCompleted()
        {

            var status = StatusHelper.Status_3Step_AllCompleted();

            var dispositionStep = DashboardHelper.GetDispositionStep(status);

            Assert.IsInstanceOf(typeof(ReportStep), dispositionStep);
        }


        [Test]
        public void DashboardStatusHelper_GetDispositionStep_4StepsInterviewDispositionStep()
        {
            var status = StatusHelper.Status_4Step_Interview();

            var dispositionStep = DashboardHelper.GetDispositionStep(status);

            Assert.IsInstanceOf(typeof(SurveyStep), dispositionStep);
        }

        [Test]
        public void DashboardStatusHelper_GetDispositionStep_4StepsSchedulingDispositionStep()
        {
            var status = StatusHelper.Status_4Step_Scheduling();

            var dispositionStep = DashboardHelper.GetDispositionStep(status);

            Assert.IsInstanceOf(typeof(SchedulingStep), dispositionStep);
        }

        [Test]
        public void DashboardStatusHelper_GetDispositionStep_4StepsScreeningDispositionStep()
        {
            var status = StatusHelper.Status_4Step_Screening();

            var dispositionStep = DashboardHelper.GetDispositionStep(status);

            Assert.IsInstanceOf(typeof(ScreeningStep), dispositionStep);
        }

        [Test]
        public void DashboardStatusHelper_GetDispositionStep_4StepsReportDispositionStep()
        {
            var status = StatusHelper.Status_4Step_Report();

            var dispositionStep = DashboardHelper.GetDispositionStep(status);

            Assert.IsInstanceOf(typeof(ReportStep), dispositionStep);
            var reportStep = dispositionStep as ReportStep;
            Assert.IsTrue(reportStep.ReportReady);
        }

        [Test]
        public void DashboardStatusHelper_GetDispositionStep_4StepsReportParticularScenario()
        {
            var status = StatusHelper.Status_4Step_Report_ParticularScenario();

            var dispositionStep = DashboardHelper.GetDispositionStep(status);

            Assert.IsInstanceOf(typeof(ReportStep), dispositionStep);
            var reportStep = dispositionStep as ReportStep;
            Assert.IsFalse(reportStep.ReportReady);
        }


        [Test]
        public void DashboardStatusHelper_GetDispositionStep_4StepsAllStepsCompleted()
        {

            var status = StatusHelper.Status_4Step_AllCompleted();

            var dispositionStep = DashboardHelper.GetDispositionStep(status);

            Assert.IsInstanceOf(typeof(ReportStep), dispositionStep);
        }


        #endregion

        #region GetStepperParameter

        [Test]
        public void DashboardStatusHelper_GetStepperParameter_WrongStatus()
        {

            var status = StatusHelper.CreateWrongStatus();

            Assert.Throws<Exception>(() => DashboardHelper.GetDispositionStep(status));
        }

        [Test]
        public void DashboardStatusHelper_GetSteps_3Steps()
        {

            var status = StatusHelper.CreateStatus();
            status.Dashboard.SurveyStep.Status = StatusEnum.None;

            var stepperParam = DashboardHelper.GetStepperParameter(status);

            Assert.AreEqual(3, stepperParam.Steps);
        }

        [Test]
        public void DashboardStatusHelper_GetSteps_4Steps()
        {
            var status = StatusHelper.CreateStatus();
            status.Dashboard.SurveyStep.Status = StatusEnum.Pending;

            var stepperParam = DashboardHelper.GetStepperParameter(status);

            Assert.AreEqual(4, stepperParam.Steps);
        }


        [Test]
        public void DashboardStatusHelper_GetCurrentStep_3StepsSchedulingAsCurrent()
        {

            var status = StatusHelper.Status_3Step_Scheduling();

            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);

            Assert.AreEqual(1, stepperParam.CurrentStep);
        }

        [Test]
        public void DashboardStatusHelper_GetCurrentStep_3StepsScreeningAsCurrent()
        {
            var status = StatusHelper.Status_3Step_Screening();

            var stepperParam = DashboardHelper.GetStepperParameter(status);

            Assert.AreEqual(2, stepperParam.CurrentStep);
        }

        [Test]
        public void DashboardStatusHelper_GetCurrentSteps_3StepsReportAsCurrent()
        {
            var status = StatusHelper.Status_3Step_Report();

            var stepperParam = DashboardHelper.GetStepperParameter(status);

            Assert.AreEqual(3, stepperParam.CurrentStep);
        }


        [Test]
        public void DashboardStatusHelper_GetCurrentStep_4StepsInterviewAsCurrent()
        {
            var status = StatusHelper.Status_4Step_Interview();

            var stepperParam = DashboardHelper.GetStepperParameter(status);

            Assert.AreEqual(1, stepperParam.CurrentStep);
        }

        [Test]
        public void DashboardStatusHelper_GetCurrentStep_4StepsSchedulingAsCurrent()
        {
            var status = StatusHelper.Status_4Step_Scheduling();

            var stepperParam = DashboardHelper.GetStepperParameter(status);

            Assert.AreEqual(2, stepperParam.CurrentStep);
        }

        [Test]
        public void DashboardStatusHelper_GetCurrentStep_4StepsScreeningAsCurrent()
        {
            var status = StatusHelper.Status_4Step_Screening();

            var stepperParam = DashboardHelper.GetStepperParameter(status);

            Assert.AreEqual(3, stepperParam.CurrentStep);
        }

        [Test]
        public void DashboardStatusHelper_GetCurrentSteps_4StepsReportAsCurrent()
        {
            var status = StatusHelper.Status_4Step_Report();

            var stepperParam = DashboardHelper.GetStepperParameter(status);

            Assert.AreEqual(4, stepperParam.CurrentStep);
        }

        [Test]
        public void DashboardStatusHelper_GetCurrentSteps_4StepsAllStepsCompleted()
        {

            var status = StatusHelper.Status_4Step_AllCompleted();

            var stepperParam = DashboardHelper.GetStepperParameter(status);

            Assert.AreEqual(4, stepperParam.CurrentStep);
        }

        #endregion
    }
}
