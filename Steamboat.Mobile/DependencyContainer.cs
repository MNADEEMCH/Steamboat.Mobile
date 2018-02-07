﻿using System;
using Splat;
using Steamboat.Mobile.Managers.Account;
using Steamboat.Mobile.Managers.Participant;
using Steamboat.Mobile.Models.User;
using Steamboat.Mobile.Repositories.Database;
using Steamboat.Mobile.Repositories.User;
using Steamboat.Mobile.Services.Account;
using Steamboat.Mobile.Services.Dialog;
using Steamboat.Mobile.Services.Navigation;
using Steamboat.Mobile.Services.Participant;
using Steamboat.Mobile.Services.RequestProvider;
using Steamboat.Mobile.ViewModels;
using Steamboat.Mobile.Services.Modal;
using Steamboat.Mobile.ViewModels.Modals;

namespace Steamboat.Mobile
{
    public class DependencyContainer
    {
        public static void RegisterDependencies()
        {
            Locator.CurrentMutable.RegisterConstant(new RequestProvider(), typeof(IRequestProvider));
            Locator.CurrentMutable.RegisterConstant(new NavigationService(), typeof(INavigationService));
            Locator.CurrentMutable.RegisterConstant(new ModalService(), typeof(IModalService));
            Locator.CurrentMutable.RegisterConstant(new DialogService(), typeof(IDialogService));

            //Services
            Locator.CurrentMutable.RegisterLazySingleton(() => new AccountService(), typeof(IAccountService));
            Locator.CurrentMutable.RegisterLazySingleton(() => new ParticipantService(), typeof(IParticipantService));

            //Repositories
            Locator.CurrentMutable.RegisterLazySingleton(() => new UserRepository(), typeof(IUserRepository));

            //Database
            Locator.CurrentMutable.RegisterConstant(new Database<CurrentUser>(), typeof(IDatabase<CurrentUser>));

            //Managers
            Locator.CurrentMutable.RegisterLazySingleton(() => new AccountManager(), typeof(IAccountManager));
            Locator.CurrentMutable.RegisterLazySingleton(() => new ParticipantManager(), typeof(IParticipantManager));

            //ViewModels
            Locator.CurrentMutable.RegisterLazySingleton(() => new LoginViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() => new InitPasswordViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() => new InterviewViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() => new SchedulingViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() => new ScreeningViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() => new ReportViewModel());
            Locator.CurrentMutable.Register(() => new ReportDetailsViewModel());
            Locator.CurrentMutable.Register(() => new ConsentsViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() => new StepperViewModel());
            Locator.CurrentMutable.Register(() => new SchedulingTimeViewModel());
            Locator.CurrentMutable.Register(() => new SchedulingEventDateViewModel());            
            Locator.CurrentMutable.Register(() => new SchedulingConfirmationViewModel()); 

            //ModalViewModels
            Locator.CurrentMutable.Register(() => new DispositionMoreInfoModalViewModel());
            Locator.CurrentMutable.Register(() => new WelcomeModalViewModel());
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
