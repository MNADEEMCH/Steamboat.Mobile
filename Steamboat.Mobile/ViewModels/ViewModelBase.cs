using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Steamboat.Mobile.Services.Dialog;
using Steamboat.Mobile.Services.Navigation;
using Xamarin.Forms;
using Steamboat.Mobile.Services.Modal;
using Steamboat.Mobile.Exceptions;
using Steamboat.Mobile.Managers.Account;
using Steamboat.Mobile.Managers.Application;

namespace Steamboat.Mobile.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected readonly IDialogService DialogService;
        protected readonly INavigationService NavigationService;
        protected readonly IModalService ModalService;
		private IApplicationManager _applicationManager;

        protected bool SetPropertyValue<T>(ref T storageField, T newValue, Expression<Func<T>> propExpr)
        {
            if (Equals(storageField, newValue))
            {
                return false;
            }

            storageField = newValue;
            var prop = (System.Reflection.PropertyInfo)((MemberExpression)propExpr.Body).Member;
            this.RaisePropertyChanged(prop.Name);

            return true;
        }

        protected bool SetPropertyValue<T>(ref T storageField, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (Equals(storageField, newValue))
            {
                return false;
            }

            storageField = newValue;
            this.RaisePropertyChanged(propertyName);

            return true;
        }

        protected void RaiseAllPropertiesChanged()
        {
            // By convention, an empty string indicates all properties are invalid.
            this.PropertyChanged(this, new PropertyChangedEventArgs(string.Empty));
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propExpr)
        {
            var prop = (System.Reflection.PropertyInfo)((MemberExpression)propExpr.Body).Member;
            this.RaisePropertyChanged(prop.Name);
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private bool isLoading;

        public bool IsLoading { get { return isLoading; } set { SetPropertyValue(ref isLoading, value); } }

        public ViewModelBase()
        {
            DialogService = DialogService ?? DependencyContainer.Resolve<IDialogService>();
            NavigationService = NavigationService ?? DependencyContainer.Resolve<INavigationService>();
            ModalService = ModalService ?? DependencyContainer.Resolve<IModalService>();
			_applicationManager = _applicationManager ?? DependencyContainer.Resolve<IApplicationManager>();
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }

        public void NavigateTimer()
		{
			_applicationManager.RestartTimer();
		}

        public virtual Task Refresh()
        {
            return Task.FromResult(false);
        }

        protected async Task TryExecute(Func<Task> onTry, Func<Exception, Task> onCatch = null, Action onFinally = null)
        {
            await Task.Run(async () =>
            {
                try
                {
                    await onTry();
                }
                catch(SessionExpiredException){
                    //this type of exception is handled on the manager layer
                }
                catch (Exception ex)
                {
                    if (onCatch != null)
                        await onCatch(ex);
                    else
                        await DialogService.ShowAlertAsync(ex.Message, "Error", "OK");
                }
                finally
                {
                    if (onFinally != null)
                        onFinally();
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
                catch (SessionExpiredException)
                {
                    //this type of exception is handled on the manager layer
                }
                catch (Exception ex)
                {
                    if (onCatch != null)
                        await onCatch(ex);
                    else
                        await DialogService.ShowAlertAsync(ex.Message, "Error", "OK");
                }
                finally
                {
                    if (onFinally != null)
                        result = await onFinally();
                }

                return result;
            });
        }
    }
}
