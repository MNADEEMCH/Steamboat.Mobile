using Steamboat.Mobile.Models.Stepper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Steamboat.Mobile.ViewModels
{
    public class StepperViewModel : ViewModelBase
    {
        #region Properties

        private bool _stepperInitialized = false;
        private int _currentStep;

        public ICommand DrawStepperCommand { get; set; }

        #endregion

        public override async Task InitializeAsync(object parameter)
        {
            
            StepperParam stepperParam = parameter as StepperParam;
            if (stepperParam != null)
            {
                if (!_stepperInitialized)
                    InitializeStepper(stepperParam);
                else
                    RefreshStepper(stepperParam);
            }

            await base.InitializeAsync(true);
        }

        private void InitializeStepper(StepperParam stepperParam)
        {
            stepperParam.PreviousStep = stepperParam.CurrentStep;
            _currentStep = stepperParam.CurrentStep;
            _stepperInitialized = true;
            DrawStepperCommand.Execute(stepperParam);

        }

        private void RefreshStepper(StepperParam stepperParam)
        {
            stepperParam.PreviousStep = _currentStep;
            _currentStep = stepperParam.CurrentStep;
            DrawStepperCommand.Execute(stepperParam);
        }

        public override async Task Refresh()
        {
            await base.Refresh();

            _currentStep = -1;
            _stepperInitialized = false;
        }

    }
}
