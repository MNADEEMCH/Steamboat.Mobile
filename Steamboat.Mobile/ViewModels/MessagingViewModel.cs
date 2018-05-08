using System;
using System.Threading.Tasks;

namespace Steamboat.Mobile.ViewModels
{
    public class MessagingViewModel : ViewModelBase
    {
        public MessagingViewModel()
        {
        }

        public override Task InitializeAsync(object parameter)
        {
            return base.InitializeAsync(true);
        }
    }
}
