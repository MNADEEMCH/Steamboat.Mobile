using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Models.Modal;
using Steamboat.Mobile.Models.Participant;
using Steamboat.Mobile.Models.Stepper;
using Steamboat.Mobile.ViewModels.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Steamboat.Mobile.Models.Participant.DispositionSteps;
using Steamboat.Mobile.Managers.Account;
using Steamboat.Mobile.Models.User;

namespace Steamboat.Mobile.ViewModels
{
    public abstract class DispositionViewModelBase : ViewModelBase
    {
        #region properties

        private IAccountManager _accountManager;
        private StepperViewModel _stepperViewModel;
        private string _iconSource;
        private string _title;
        private string _message;
        private string _mainActionButtonText;
        private bool _mainButtonVisible;
        private string _steps;
        private ModalParam _modalMoreInfo;

        public ICommand MoreInfoCommand { get; set; }
        public ICommand MainActionCommand { get; set; }
        public string IconSource { get { return _iconSource; } set { SetPropertyValue(ref _iconSource, value); } }
        public string Title { get { return _title; } set { SetPropertyValue(ref _title, value); } }
        public string Message { get { return _message; } set { SetPropertyValue(ref _message, value); } }
        public string MainActionButtonText { get { return _mainActionButtonText; } set { SetPropertyValue(ref _mainActionButtonText, value); } }
        public bool MainButtonVisible { set { SetPropertyValue(ref _mainButtonVisible, value); } get { return _mainButtonVisible; } }
        public string Steps { get { return _steps; } set { SetPropertyValue(ref _steps, value); } }
        public ModalParam ModalMoreInfo { get { return _modalMoreInfo; } set { SetPropertyValue(ref _modalMoreInfo, value); } }

        #endregion

        public DispositionViewModelBase(StepperViewModel stepperViewModel = null, AccountManager accountManager = null)
        {
            _accountManager = accountManager ?? DependencyContainer.Resolve<IAccountManager>();
            _stepperViewModel = stepperViewModel ?? DependencyContainer.Resolve<StepperViewModel>();
            MoreInfoCommand = new Command(async () => await MoreInfo());
            MainActionCommand = new Command(async () => await MainAction());
        }

        public async override Task InitializeAsync(object parameter)
        {
            await TryExecute(async () =>
            {
                Status status = parameter as Status;
                DispositionStep dispositionStep = DashboardHelper.GetDispositionStep(status);
                IsDispositionStepValid(dispositionStep);

                await InitializeDispositionStep(status, dispositionStep);

            }, null, () => IsLoading = false);
        }

        private void IsDispositionStepValid(DispositionStep dispositionStep)
        {
            if (dispositionStep == null)
                throw new Exception("Error loading disposition step");
        }

        private async Task InitializeDispositionStep(Status status, DispositionStep dispositionStep)
        {
            StepperParam stepperParam = await InitializeStepper(status);

            Title = dispositionStep.Title;
            Message = dispositionStep.Message;
            MainButtonVisible = !String.IsNullOrEmpty(dispositionStep.ButtonText);
            MainActionButtonText = dispositionStep.ButtonText.ToUpper();
            Steps = String.Format("STEP  {0}  OF  {1}", stepperParam.CurrentStep, stepperParam.Steps);

            InitializeSpecificStep(status);

            InitializeModalMoreInfo(dispositionStep);

            await ShowAndStoreAlert(status);
        }

        private async Task<StepperParam> InitializeStepper(Status status)
        {
            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);
            Device.BeginInvokeOnMainThread(async () =>
            {
                await _stepperViewModel.InitializeAsync(stepperParam);
            });
            return stepperParam;
        }

        private void InitializeModalMoreInfo(DispositionStep dispositionStep)
        {
            ModalMoreInfo = new ModalParam()
            {
                IconSource = IconSource,
                Title = dispositionStep.InformationTitle,
                Message = dispositionStep.InformationMessage.Replace("<br/>", "\n")
            };
        }

        private async Task ShowAndStoreAlert(Status status)
        {
            await TryExecute(async () =>
            {
                Alert alertToShow = await IsAnyAlertToShow(status);

                if (alertToShow != null)
                {
                    ModalParam modalToShow = CreateModalParam(alertToShow);
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await ModalService.PushAsync<WelcomeModalViewModel>(modalToShow);
                    });
                    await _accountManager.AddUserAlert(App.CurrentUser.Email, alertToShow.ID);
                }
            });
        }

        private async Task<Alert> IsAnyAlertToShow(Status status)
        {
            Alert alertToShow = null;
            bool isAnyAlertOnStatusDashboard = status != null &&
                                               status.Dashboard != null &&
                                               status.Dashboard.Alert != null;

            if (isAnyAlertOnStatusDashboard)
            {
                Alert alert = status.Dashboard.Alert;
                bool userAlreadySawTheAlert = await UserAlreadySawTheAlert(alert);
                if (!userAlreadySawTheAlert)
                    alertToShow = alert;

            }

            return alertToShow;
        }
        private async Task<bool> UserAlreadySawTheAlert(Alert alert)
        {
            return await TryExecute<bool>(async () =>
            {
                bool userAlreadySawTheAlert = false;
                var userAlerts = await _accountManager.GetUserAlerts(App.CurrentUser.Email);
                userAlreadySawTheAlert = userAlerts != null && userAlerts.AlertsIds.Contains(alert.ID);
                return userAlreadySawTheAlert;
            });
        }

        private ModalParam CreateModalParam(Alert alert)
        {

            return new ModalParam()
            {
                Title = alert.Title,
                Message = alert.Message.Replace("<br/>", "\n"),
                ButtonText = alert.ButtonText,
            };
        }

        private async Task<FormattedString> HandleTextFormat(string text)
        {   //TODO: Spans not styling well
            FormattedString formattedString = new FormattedString();

            try
            {
                bool boldDetected = false;
                bool textAlreadySplitted = false;
                string tagBoldOpen = "<b>";
                string tagBoldClose = "</b>";
                string textToCut = text;
                await Task.Run(() =>
                {
                    while (!textAlreadySplitted)
                    {
                        var pieces = textToCut.Split(new string[] { boldDetected ? tagBoldClose : tagBoldOpen }, 2, StringSplitOptions.None);
                        textAlreadySplitted = pieces.Length == 1;
                        if (!textAlreadySplitted)
                        {
                            textToCut = pieces[1];
                            formattedString.Spans.Add(new Span()
                            {
                                Text = pieces[0],
                                FontAttributes = (boldDetected ? FontAttributes.Bold : FontAttributes.None)
                            });
                        }
                        else
                        {
                            formattedString.Spans.Add(new Span()
                            {
                                Text = pieces[0],
                                FontAttributes = FontAttributes.None
                            });
                        }
                        boldDetected = !boldDetected && pieces.Length == 2;
                    }
                });
            }
            catch (Exception ex)
            {
                formattedString = text;
            }
            return formattedString;
        }

        protected async Task MoreInfo()
        {
            await ModalService.PushAsync<DispositionMoreInfoModalViewModel>(ModalMoreInfo);
        }

        protected abstract Task MainAction();

        protected virtual void InitializeSpecificStep(Status status)
        {

        }
    }
}
