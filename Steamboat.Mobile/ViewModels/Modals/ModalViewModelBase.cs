using Steamboat.Mobile.Services.Modal;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Steamboat.Mobile.Services.Navigation;
using Steamboat.Mobile.Services.Dialog;

namespace Steamboat.Mobile.ViewModels.Modals
{
    public class ModalViewModelBase: INotifyPropertyChanged
    {
        #region Properties
        private bool isLoading;
        private bool isBusy;

        protected readonly IDialogService DialogService;
        protected readonly IModalService ModalService;

        public bool IsLoading { get { return isLoading; } set { SetPropertyValue(ref isLoading, value); } }
        public bool IsBusy { get { return isBusy; } set { SetPropertyValue(ref isBusy, value); } }
        public ICommand CloseModalCommand { get; set; }
        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

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

        public ModalViewModelBase()
        {
            IsLoading = true;
            ModalService = ModalService ?? DependencyContainer.Resolve<IModalService>();
            DialogService = DialogService ?? DependencyContainer.Resolve<IDialogService>();
            CloseModalCommand = new Command(async () => await CloseModal());
        }

        public virtual Task InitializeAsync(object parameter)
        {
            return Task.FromResult(false);
        }

        protected async Task CloseModal()
        {
            await ModalService.PopAsync();
        }
    }
}
