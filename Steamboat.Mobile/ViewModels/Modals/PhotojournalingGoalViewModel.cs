using System;
using System.Threading.Tasks;

namespace Steamboat.Mobile.ViewModels.Modals
{
    public class PhotojournalingGoalViewModel : ModalViewModelBase
    {
        public PhotojournalingGoalViewModel()
        {
        }

        public override Task InitializeAsync(object parameter)
        {
            IsLoading = false;

            return base.InitializeAsync(parameter);
        }
    }
}
