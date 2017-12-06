using System;
using System.Threading.Tasks;

namespace Steamboat.Mobile.Services.Dialog
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string buttonLabel);
    }
}
