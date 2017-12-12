using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Steamboat.Mobile.Services.Dialog;
using Steamboat.Mobile.Services.Navigation;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class ViewModelBase : BindableObject, INotifyPropertyChanged
    {
        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected readonly IDialogService DialogService;
        protected readonly INavigationService NavigationService;

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
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}
