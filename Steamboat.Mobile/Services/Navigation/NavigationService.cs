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
            return NavigateToAsync<LoginViewModel>(true);
        }

        public Task NavigateToAsync<TViewModel>(bool mainPage = false, bool animate = true) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), null, mainPage, animate);
        }

        public Task NavigateToAsync<TViewModel>(object parameter, bool mainPage = false, bool animate = true) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter, mainPage, animate);
        }

        public Task NavigateToAsync(Type vm, bool mainPage = false, bool animate = true)
        {
            return InternalNavigateToAsync(vm, null, mainPage, animate);
        }

        public Task NavigateToAsync(Type vm, object parameter, bool mainPage = false, bool animate = true)
        {
            return InternalNavigateToAsync(vm, parameter, mainPage, animate);
        }

        public async Task PopAsync(object pages, bool animate = true)
        {
            var navigationPage = GetCurrentNavigationPage();

            if (pages != null)
            {
                int numberOfPages = 0;
                if (int.TryParse(pages.ToString(), out numberOfPages))
                {
                    for (int i = 0; i < numberOfPages; i++)
                    {
                        if (i == numberOfPages - 1)
                            Device.BeginInvokeOnMainThread(async () => await navigationPage.PopAsync(animate));
                        else
                            Device.BeginInvokeOnMainThread(async () => await RemoveLastFromBackStackAsync());
                    }
                }
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () => await navigationPage.PopAsync());
            }
        }

        public Task RemoveLastFromBackStackAsync()
        {
            var navigationPage = GetCurrentNavigationPage();

            navigationPage.Navigation.RemovePage(
                navigationPage.Navigation.NavigationStack[navigationPage.Navigation.NavigationStack.Count - 2]);

            return Task.FromResult(true);
        }

        public Task RemoveBackStackAsync()
        {
            var navigationPage = GetCurrentNavigationPage();

            while (navigationPage.Navigation.NavigationStack.Count > 1)
            {
                var page = navigationPage.Navigation.NavigationStack[0];
                navigationPage.Navigation.RemovePage(page);
            }

            return Task.FromResult(true);
        }

        private async Task InternalNavigateToAsync(Type viewModelType, object parameter, bool mainPage, bool animate)
        {
            Page page = CreatePage(viewModelType, parameter);
            NavigationPage.SetBackButtonTitle(page, string.Empty);

            if (page is MainView)
            {
                Application.Current.MainPage = page;
            }
            else if (page is LoginView)
            {
                Application.Current.MainPage = new CustomNavigationView(page);
            }
            else if (Application.Current.MainPage is MainView)
            {
                var mPage = Application.Current.MainPage as MainView;
                var navigationPage = mPage.Detail as CustomNavigationView;

                mPage.IsPresented = false;

                if (navigationPage == null || mainPage)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        navigationPage = new CustomNavigationView(page);
                        mPage.Detail = navigationPage;
                    });
                }
                else
                {
                    var currentPage = navigationPage.CurrentPage;

                    if (currentPage.GetType() != page.GetType())
                    {
                        Device.BeginInvokeOnMainThread(async () => { await navigationPage.PushAsync(page, animate); });
                    }
                }
            }
            else
            {
                var navigationPage = Application.Current.MainPage as CustomNavigationView;
                if (IsMainPage(navigationPage, mainPage))
                {
                    Device.BeginInvokeOnMainThread(() => { Application.Current.MainPage = new CustomNavigationView(page); });

                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () => { await navigationPage.PushAsync(page, animate); });
                }
            }

            if (!(page is LoginView))
                (page.BindingContext as ViewModelBase).NavigateTimer();

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

        private bool IsMainPage(CustomNavigationView page, bool mainPage)
        {
            return page != null && mainPage;
        }

        private CustomNavigationView GetCurrentNavigationPage()
        {
            var navigationPage = Application.Current.MainPage as CustomNavigationView;
            if (navigationPage == null)
            {
                var mPage = Application.Current.MainPage as MainView;
                navigationPage = mPage.Detail as CustomNavigationView;
            }

            return navigationPage;
        }

    }
}
