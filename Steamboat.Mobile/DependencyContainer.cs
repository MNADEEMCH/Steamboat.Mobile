using System;
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
using Steamboat.Mobile.Managers.Application;
using Steamboat.Mobile.Services.App;
using Steamboat.Mobile.Helpers.Settings;
using System.Threading.Tasks;
using Steamboat.Mobile.Services.Logger;

namespace Steamboat.Mobile
{
    public class DependencyContainer
    {
        public static void RegisterDependencies()
        {
#if PRODUCTION
			Locator.CurrentMutable.RegisterConstant(new ProdSettings(), typeof(ISettings));
#else
            Locator.CurrentMutable.RegisterConstant(new DevSettings(), typeof(ISettings));
#endif

            Locator.CurrentMutable.RegisterConstant(new RequestProvider(), typeof(IRequestProvider));
            Locator.CurrentMutable.RegisterConstant(new NavigationService(), typeof(INavigationService));
            Locator.CurrentMutable.RegisterConstant(new ModalService(), typeof(IModalService));
            Locator.CurrentMutable.RegisterConstant(new DialogService(), typeof(IDialogService));

            //Services
            Locator.CurrentMutable.RegisterLazySingleton(() => new AccountService(), typeof(IAccountService));
            Locator.CurrentMutable.RegisterLazySingleton(() => new ParticipantService(), typeof(IParticipantService));
            Locator.CurrentMutable.RegisterLazySingleton(() => new ApplicationService(), typeof(IApplicationService));
            Locator.CurrentMutable.RegisterLazySingleton(() => new LoggerService(), typeof(ILoggerService));

            //Repositories
            Locator.CurrentMutable.RegisterLazySingleton(() => new UserRepository(), typeof(IUserRepository));
            Locator.CurrentMutable.RegisterLazySingleton(() => new UserAlertRepository(), typeof(IUserAlertRepository));

            //Database
            Locator.CurrentMutable.RegisterConstant(new Database<CurrentUser>(), typeof(IDatabase<CurrentUser>));
            Locator.CurrentMutable.RegisterConstant(new Database<UserAlert>(), typeof(IDatabase<UserAlert>));

            //ViewModels
            Locator.CurrentMutable.Register(() => new ReportDetailsViewModel());
            Locator.CurrentMutable.Register(() => new ConsentsViewModel());
            Locator.CurrentMutable.Register(() => new SchedulingEventTimeViewModel());
            Locator.CurrentMutable.Register(() => new SchedulingEventDateViewModel());
            Locator.CurrentMutable.Register(() => new SchedulingConfirmationViewModel());
            Locator.CurrentMutable.Register(() => new ScreeningInterviewViewModel());
            Locator.CurrentMutable.Register(() => new TermsConditionsViewModel());
            Locator.CurrentMutable.Register(() => new PhotojournalingViewModel());
            Locator.CurrentMutable.Register(() => new CameraViewModel());
            Locator.CurrentMutable.Register(() => new PhotoReviewViewModel());
            Locator.CurrentMutable.Register(() => new PhotoDetailsViewModel());
            RegisterSingletonViewModels();

            //ModalViewModels
            Locator.CurrentMutable.Register(() => new DispositionMoreInfoModalViewModel());
            Locator.CurrentMutable.Register(() => new WelcomeModalViewModel());
            Locator.CurrentMutable.Register(() => new ScreeningCancelConfirmationModalViewModel());
            Locator.CurrentMutable.Register(() => new InterviewEditQuestionModalViewModel());
            Locator.CurrentMutable.Register(() => new PhotojournalingMoreInfoModalViewModel());
            Locator.CurrentMutable.Register(() => new CameraAccessModalViewModel());
            Locator.CurrentMutable.Register(() => new PhotoDetailsModalViewModel());
            Locator.CurrentMutable.Register(() => new PhotojournalingGoalViewModel());

            //Managers
            Locator.CurrentMutable.RegisterConstant(new ApplicationManager(), typeof(IApplicationManager));
            Locator.CurrentMutable.RegisterConstant(new AccountManager(), typeof(IAccountManager));
            Locator.CurrentMutable.RegisterConstant(new ParticipantManager(), typeof(IParticipantManager));
        }

        public async static Task RefreshDependencies()
        {
            Resolve<MainViewModel>().Reset();
            Resolve<MenuViewModel>().Reset();
            await Task.Run(() => RegisterSingletonViewModels());
        }

        public static void RegisterSingletonViewModels()
        {
            Locator.CurrentMutable.RegisterLazySingleton(() => new LoginViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() => new MainViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() => new MenuViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() => new InitPasswordViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() => new InterviewViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() => new SchedulingViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() => new ScreeningViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() => new ReportViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() => new StepperViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() => new MessagingViewModel());
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
