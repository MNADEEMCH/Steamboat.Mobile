using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Models.Modal;
using Steamboat.Mobile.Models.Participant;
using Steamboat.Mobile.Models.Stepper;
using Steamboat.Mobile.ViewModels.Modals;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class InterviewViewModel : DispositionViewModelBase
    {
        public InterviewViewModel(StepperViewModel stepperViewModel = null) : base(stepperViewModel)
        {
            IsLoading = true;
            IconSource = "icInterview.png";
        }

        protected override async Task MainAction()
        {
            
        }

    }
}
