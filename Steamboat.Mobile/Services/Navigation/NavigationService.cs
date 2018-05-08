﻿using System;
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

        public async Task PopAsync(object pages)
        {
            var navigationPage = Application.Current.MainPage as CustomNavigationView;
            if (pages != null)
            {
                int numberOfPages = 0;
                if (int.TryParse(pages.ToString(), out numberOfPages))
                {
                    for (int i = 0; i < numberOfPages; i++)
                    {
                        if (i == numberOfPages - 1)
                            await navigationPage.PopAsync();
                        else
                            await RemoveLastFromBackStackAsync();
                    }
                }
            }
            else
            {
                await navigationPage.PopAsync();
            }
            //await (navigationPage.CurrentPage.BindingContext as ViewModelBase).Refresh();
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

                if (navigationPage == null || mainPage)
                {
                    navigationPage = new CustomNavigationView(page);
                    mPage.Detail = navigationPage;
                }
                else
                {
                    var currentPage = navigationPage.CurrentPage;

                    if (currentPage.GetType() != page.GetType())
                    {
                        await navigationPage.PushAsync(page);
                    }
                }

                mPage.IsPresented = false;
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

        private bool IsMainPage(CustomNavigationView page, bool mainPage)
        {
            return page != null && mainPage;
        }

    }
}
