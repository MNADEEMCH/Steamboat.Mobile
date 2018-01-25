using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using System.Reflection;
using System.Globalization;
using Rg.Plugins.Popup.Services;
using Steamboat.Mobile.ViewModels.Modals;

namespace Steamboat.Mobile.Services.Modal
{
    public class ModalService : IModalService
    {
        public Task PushAsync<TModalViewModel>() where TModalViewModel : ModalViewModelBase
        {
            return InternalPushAsync(typeof(TModalViewModel));
        }

        public Task PushAsync<TModalViewModel>(object parameter) where TModalViewModel : ModalViewModelBase
        {
            return InternalPushAsync(typeof(TModalViewModel), parameter);
        }

        public async Task PopAsync()
        {
            await PopupNavigation.PopAsync();
        }

        private async Task InternalPushAsync(Type modalViewModelType, object parameter = null)
        {
            PopupPage popupPage = CreatePopupPage(modalViewModelType);
            await PopupNavigation.PushAsync(popupPage);
            await (popupPage.BindingContext as ModalViewModelBase).InitializeAsync(parameter);
        }

        private Type GetPageTypeForModalViewModel(Type modalViewModelType)
        {
            var modalViewName = modalViewModelType.FullName.Replace("Model", string.Empty);
            var modalViewModelAssemblyName = modalViewModelType.GetTypeInfo().Assembly.FullName;
            var modalViewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", modalViewName, modalViewModelAssemblyName);
            var modalViewType = Type.GetType(modalViewAssemblyName);
            return modalViewType;
        }

        private PopupPage CreatePopupPage(Type modalViewModelType)
        {
            Type popupPageType = GetPageTypeForModalViewModel(modalViewModelType);
            if (popupPageType == null)
            {
                throw new Exception($"Cannot locate page type for {modalViewModelType}");
            }

            PopupPage popupPage = Activator.CreateInstance(popupPageType) as PopupPage;
            return popupPage;
        }

    }
}
