using System;
using Splat;
using Steamboat.Mobile.Managers.Account;
using Steamboat.Mobile.Models.User;
using Steamboat.Mobile.Repositories.Database;
using Steamboat.Mobile.Repositories.User;
using Steamboat.Mobile.Services.Account;
using Steamboat.Mobile.Services.Dialog;
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
            Locator.CurrentMutable.RegisterConstant(new DialogService(), typeof(IDialogService));

            //Services
            Locator.CurrentMutable.RegisterLazySingleton(() => new AccountService(), typeof(IAccountService));

            //Repositories
            Locator.CurrentMutable.RegisterLazySingleton(() => new UserRepository(), typeof(IUserRepository));

            //Database
            Locator.CurrentMutable.RegisterConstant(new Database<CurrentUser>(), typeof(IDatabase<CurrentUser>));

            //Managers
            Locator.CurrentMutable.RegisterLazySingleton(() => new AccountManager(), typeof(IAccountManager));

            //ViewModels
            Locator.CurrentMutable.RegisterLazySingleton(() => new LoginViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() => new StatusViewModel());
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
