using Steamboat.Mobile.ViewModels.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamboat.Mobile.Services.Modal
{
    public interface IModalService
    {
        Task PushAsync<TModalViewModel>() where TModalViewModel : ModalViewModelBase;
        Task PushAsync<TModalViewModel>(object parameter) where TModalViewModel : ModalViewModelBase;
        Task PopAsync();
    }
}
