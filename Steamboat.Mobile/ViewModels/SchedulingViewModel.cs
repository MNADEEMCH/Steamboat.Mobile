using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Models.Modal;
using Steamboat.Mobile.Models.Participant;
using Steamboat.Mobile.Models.Stepper;
using Steamboat.Mobile.ViewModels.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class SchedulingViewModel : DispositionViewModelBase
    {
        #region Properties

        public ICommand OpenSchedulingCommand { get; set; }

        #endregion

        public SchedulingViewModel(StepperViewModel stepperViewModel = null) : base(stepperViewModel)
        {
            IsLoading = true;
            //TODO: Change image source to XAML
            IconSource = "icScheduling.png";
            OpenSchedulingCommand = new Command(async () => await OpenScheduling());
        }

        private async Task OpenScheduling()
        {
            await NavigationService.NavigateToAsync<SchedulingEventDateViewModel>();
        }
    }
}
