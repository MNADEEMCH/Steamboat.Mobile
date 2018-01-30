using System;
using System.Threading.Tasks;
using Steamboat.Mobile.ViewModels;

namespace Steamboat.Mobile.Services.Navigation
{
    public interface INavigationService
    {
        ViewModelBase PreviousPageViewModel { get; }

        Task InitializeAsync();

        Task NavigateToAsync<TViewModel>(bool mainPage = false) where TViewModel : ViewModelBase;

        Task NavigateToAsync<TViewModel>(object parameter, bool mainPage = false) where TViewModel : ViewModelBase;

        Task NavigateToAsync(Type vm, bool mainPage = false);

        Task NavigateToAsync(Type vm, object parameter, bool mainPage = false);

        Task PopAsync();

        Task RemoveLastFromBackStackAsync();

        Task RemoveBackStackAsync();
    }
}
