using System;
using System.Threading.Tasks;
using Steamboat.Mobile.ViewModels;

namespace Steamboat.Mobile.Services.Navigation
{
    public interface INavigationService
    {
        ViewModelBase PreviousPageViewModel { get; }

        Task InitializeAsync();

        Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase;

        Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase;

        Task NavigateToAsync(Type vm);

        Task NavigateToAsync(Type vm, object parameter);

        Task RemoveLastFromBackStackAsync();

        Task RemoveBackStackAsync();
    }
}
