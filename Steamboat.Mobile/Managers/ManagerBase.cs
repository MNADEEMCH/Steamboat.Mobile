using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Steamboat.Mobile.Exceptions;
using Steamboat.Mobile.Models.Application;
using Steamboat.Mobile.Services.Dialog;
using Steamboat.Mobile.Services.Modal;
using Steamboat.Mobile.Services.Navigation;
using Xamarin.Forms;

namespace Steamboat.Mobile.Managers
{
    public class ManagerBase
    {
        protected INavigationService _navigationService;
        protected IDialogService _dialogService;
        protected IModalService _modalService;

        public ManagerBase(INavigationService navigationService = null,
                           IDialogService dialogService = null,
                           IModalService modalService = null){

            _navigationService = navigationService ?? DependencyContainer.Resolve<INavigationService>();
            _dialogService = dialogService ?? DependencyContainer.Resolve<IDialogService>();
            _modalService = modalService ?? DependencyContainer.Resolve<IModalService>();

        }

        protected async Task TryExecute(Func<Task> onTry, Func<Exception, Task> onCatch = null, Func<Task> onFinally = null)
        {
            await Task.Run(async () =>
            {
                try
                {
                    await onTry();
                }
                catch (Exception ex)
                {
                    LogException(ex);

                    if(ex is SessionExpiredException){
                        await SessionExpired();
                        throw ex;
                    }
                    else if (onCatch != null)
                        await onCatch(ex);
                    else
                        throw ex;
                }
                finally
                {
                    if (onFinally != null) 
                        await onFinally();
                }
            });
        }

        protected async Task<T> TryExecute<T>(Func<Task<T>> onTry, Func<Exception, Task> onCatch = null, Func<Task<T>> onFinally = null)
        {
            return await Task.Run(async () =>
            {
                var result = default(T);

                try
                {
                    result = await onTry();
                }
                catch (Exception ex)
                {
                    LogException(ex);

                    if (ex is SessionExpiredException)
                    {
                        await SessionExpired();
                        throw ex;
                    }
                    else if(onCatch != null)
                        await onCatch(ex);
                    else
                        throw ex;
                }
                finally
                {
                    if (onFinally != null) 
                        result = await onFinally();   
                }

                return result;
            });
        }

        private void LogException(Exception ex){
            Debug.WriteLine($"Error: {ex}");
        }
    
        protected async Task SessionExpired()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
				await _dialogService.ShowAlertAsync("Your session has expired due to inactivity. Please log in again to restore your session.", "Session Timeout", "OK");
                await _modalService.PopAllAsync();
                await _navigationService.NavigateToAsync<ViewModels.LoginViewModel>(new Logout(){CallBackend=false});
            });
        }

    }
}
