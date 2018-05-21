using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels.Interfaces
{
	public interface IHandleViewAppearing
    {
        Task OnViewAppearingAsync(VisualElement view);
    }

    public interface IHandleViewDisappearing
    {
        Task OnViewDisappearingAsync(VisualElement view);
    }
}