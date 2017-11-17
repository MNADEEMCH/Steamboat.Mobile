using System;
using Splat;
using Steamboat.Mobile.Managers.Account;
using Steamboat.Mobile.Services.Account;
using Steamboat.Mobile.Services.RequestProvider;
using Steamboat.Mobile.ViewModels;

namespace Steamboat.Mobile
{
    public class DependencyContainer
    {
        public static void RegisterDependencies()
        {
            //Locator.CurrentMutable.RegisterConstant(new NavigationService(), typeof(INavigationService));

            Locator.CurrentMutable.RegisterConstant(new RequestProvider(), typeof(IRequestProvider));
            Locator.CurrentMutable.RegisterConstant(new AccountService(Locator.CurrentMutable.GetService<IRequestProvider>()), typeof(IAccountService));

            Locator.CurrentMutable.RegisterConstant(new AccountManager(Locator.CurrentMutable.GetService<IAccountService>()), typeof(IAccountManager));

            Locator.CurrentMutable.RegisterConstant(new LoginViewModel(Locator.CurrentMutable.GetService<IAccountManager>()) ,typeof(LoginViewModel));                       
        }

        public static T Resolve<T>()
        {
            return Locator.CurrentMutable.GetService<T>();
        }

        internal static object Resolve(Type viewModelType)
        {
            return Locator.CurrentMutable.GetService(viewModelType);
        }
    }
}
