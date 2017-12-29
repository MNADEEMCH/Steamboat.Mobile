using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Steamboat.Mobile.ViewModels;
using Steamboat.Mobile.Views;
using Xamarin.Forms;

namespace Steamboat.Mobile.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        public ViewModelBase PreviousPageViewModel
        {
            get
            {
                var mainPage = Application.Current.MainPage as CustomNavigationView;
                var viewModel = mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2].BindingContext;
                return viewModel as ViewModelBase;
            }
        }

        public Task InitializeAsync()
        {     
            return NavigateToAsync<LoginViewModel>();
        }

        public Task NavigateToAsync<TViewModel>(bool mainPage = false) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), null, mainPage);
        }

        public Task NavigateToAsync<TViewModel>(object parameter, bool mainPage = false) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter, mainPage);
        }

        public Task NavigateToAsync(Type vm, bool mainPage = false)
        {
            return InternalNavigateToAsync(vm, null, mainPage);
        }

        public Task NavigateToAsync(Type vm, object parameter, bool mainPage = false)
        {
            return InternalNavigateToAsync(vm, parameter, mainPage);
        }

        public Task RemoveLastFromBackStackAsync()
        {
            var mainPage = Application.Current.MainPage as CustomNavigationView;

            if (mainPage != null)
            {
                mainPage.Navigation.RemovePage(
                    mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2]);
            }

            return Task.FromResult(true);
        }

        public Task RemoveBackStackAsync()
        {
            var mainPage = Application.Current.MainPage as CustomNavigationView;

            if (mainPage != null)
            {
                while (mainPage.Navigation.NavigationStack.Count > 1)
                {
                    var page = mainPage.Navigation.NavigationStack[0];
                    mainPage.Navigation.RemovePage(page);
                }
            }

            return Task.FromResult(true);
        }

        private async Task InternalNavigateToAsync(Type viewModelType, object parameter, bool mainPage)
        {
            Page page = CreatePage(viewModelType, parameter);
            NavigationPage.SetBackButtonTitle(page, string.Empty);

            if (page is LoginView)
            {
                Application.Current.MainPage = new CustomNavigationView(page);
            }
            else
            {
                var navigationPage = Application.Current.MainPage as CustomNavigationView;
                if (IsMainPage(navigationPage, mainPage))
                {
                    Application.Current.MainPage = new CustomNavigationView(page);
                }
                else
                {
                    await navigationPage.PushAsync(page, true);
                }
            }

            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("Model", string.Empty);
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }

        private Page CreatePage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }

            Page page = Activator.CreateInstance(pageType) as Page;
            return page;
        }

        private bool IsMainPage(CustomNavigationView page, bool mainPage){
            return page != null && mainPage;
        }
             
    }
}
