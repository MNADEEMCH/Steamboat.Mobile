using System;
using Splat;
using Steamboat.Mobile.Managers.Account;
using Steamboat.Mobile.Services.Account;
using Steamboat.Mobile.Services.Navigation;
using Steamboat.Mobile.Services.RequestProvider;
using Steamboat.Mobile.ViewModels;

namespace Steamboat.Mobile
{
    public class DependencyContainer
    {
        public static void RegisterDependencies()
        {
            Locator.CurrentMutable.RegisterConstant(new RequestProvider(), typeof(IRequestProvider));
            Locator.CurrentMutable.RegisterConstant(new NavigationService(), typeof(INavigationService));

            Locator.CurrentMutable.RegisterLazySingleton(() => new AccountService(), typeof(IAccountService));
            Locator.CurrentMutable.RegisterLazySingleton(() => new AccountManager(), typeof(IAccountManager));

            Locator.CurrentMutable.RegisterLazySingleton(() =>new LoginViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() =>new StatusViewModel());
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
