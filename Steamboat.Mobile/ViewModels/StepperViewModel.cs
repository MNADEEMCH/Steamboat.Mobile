using Steamboat.Mobile.Models.Stepper;
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

        private int _initializeExcecute;
        public int InitializeExcecute { set { SetPropertyValue(ref _initializeExcecute, value); } get { return _initializeExcecute; } }

        private bool _animateProgress;
        public bool AnimateProgress { set { SetPropertyValue(ref _animateProgress, value); } get { return _animateProgress; } }

        private int _previousStep;
        public int PreviousStep { set { SetPropertyValue(ref _previousStep, value); } get { return _previousStep; } }

        private int _currentStep;
        public int CurrentStep { set { SetPropertyValue(ref _currentStep, value); } get { return _currentStep; } }

        private int _steps;
        public int Steps { set { SetPropertyValue(ref _steps, value); } get { return _steps; } }

        private bool _stepperInitialized = false;

        #endregion

        public StepperViewModel():base()
        {
            AnimateProgress = true;
        }

        public override async Task InitializeAsync(object parameter)
        {
            StepperParam stepperParam = parameter as StepperParam;
            if (stepperParam!=null)
            {

                if (!_stepperInitialized)
                {
                    InitializeStepper(stepperParam.Steps, stepperParam.CurrentStep);
                }
                else
                {
                    RefreshStepper(stepperParam.CurrentStep);
                }

            }
            else
            {
                //TODO: see how to handle when the stepper wont be loaded 
            }
            await base.InitializeAsync(true);
        }

        private void InitializeStepper(int steps, int currentStep){
            Steps = steps;
            CurrentStep = currentStep;
            PreviousStep = CurrentStep;
            _stepperInitialized = true;
            InitializeExcecute++;
        }

        private void RefreshStepper(int currentStep){
            PreviousStep = CurrentStep;
            CurrentStep = currentStep;
            InitializeExcecute++;
        }

        public override async Task Refresh()
        {
            await base.Refresh();

            PreviousStep = 0;
            CurrentStep = 0;
            Steps = 0;
            _stepperInitialized = false;
            InitializeExcecute = 0;
        }

    }
}
