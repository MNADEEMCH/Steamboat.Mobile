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

namespace Steamboat.Mobile.ViewModels
{
    public abstract class DispositionViewModelBase : ViewModelBase
    {
        #region properties

        private StepperViewModel _stepperViewModel;

        public ICommand LogoutCommand { get; set; }
        public ICommand MoreInfoCommand { get; set; }
        public ICommand MainActionCommand { get; set; }

        private string _iconSource;
        public string IconSource
        {
            get { return _iconSource; }
            set { SetPropertyValue(ref _iconSource, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetPropertyValue(ref _title, value); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetPropertyValue(ref _message, value); }
        }

        private string _steps;
        public string Steps
        {
            get { return _steps; }
            set { SetPropertyValue(ref _steps, value); }
        }

        private ModalParam _modalMoreInfo;
        public ModalParam ModalMoreInfo
        {
            get { return _modalMoreInfo; }
            set { SetPropertyValue(ref _modalMoreInfo, value); }
        }
        #endregion

        public DispositionViewModelBase(StepperViewModel stepperViewModel = null)
        {

            _stepperViewModel = stepperViewModel ?? DependencyContainer.Resolve<StepperViewModel>();
            LogoutCommand = new Command(async () => await Logout());
            MoreInfoCommand = new Command(async () => await MoreInfo());
            MainActionCommand = new Command(async () => await MainAction());
        }

        public async override Task InitializeAsync(object parameter)
        {
            try
            {

                Status status = parameter as Status;
                IsStatusValid(status);
                DispositionStep dispositionStep = DashboardHelper.GetDispositionStep(status);
                IsDispositionStepValid(dispositionStep);

                await InitializeDispositionStep(status, dispositionStep);

            }
            catch (Exception ex)
            {
                await this.DialogService.ShowAlertAsync(ex.Message, "Error", "OK");
            }
            finally
            {
                IsLoading = false;
            }

        }

        private void IsStatusValid(Status status)
        {
            if (status == null || status.Dashboard == null)
                throw new Exception("Error loading status");

        }

        private void IsDispositionStepValid(DispositionStep dispositionStep)
        {
            if (dispositionStep==null)
                throw new Exception("Error loading disposition step");
        }

        private async Task InitializeDispositionStep(Status status, DispositionStep dispositionStep)
        {

            StepperParam stepperParam = await InitializeStepper(status);

            Title = dispositionStep.Title;
            Message = dispositionStep.Message;
            Steps = String.Format("STEP  {0}  OF  {1}", stepperParam.CurrentStep, stepperParam.Steps);

            InitializeSpecificStep(status);

            InitializeModalMoreInfo(dispositionStep);

            ShowNotifications(status);

        }

        private async Task<StepperParam> InitializeStepper(Status status)
        {
            StepperParam stepperParam = DashboardHelper.GetStepperParameter(status);
            await _stepperViewModel.InitializeAsync(stepperParam);
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

        private void ShowNotifications(Status status)
        {
            ModalParam modalParamToShowMessage = IsAnyNotificationToShow(status);
            if (modalParamToShowMessage != null)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await ModalService.PushAsync<WelcomeModalViewModel>(modalParamToShowMessage);
                });
            }
        }
        private ModalParam IsAnyNotificationToShow(Status status)
        {
            ModalParam modalParam = null;

            if (status != null
                && status.Dashboard != null
                && status.Dashboard.Notifications != null
                && status.Dashboard.Notifications.Count > 0)
            {
                Notification notification = status.Dashboard.Notifications.First();
                modalParam = new ModalParam()
                {
                    Title = notification.Title,
                    Message = notification.Message.Replace("<br/>", "\n"),
                    ButtonText = notification.ButtonText,
                };
            }

            return modalParam;
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

        protected async Task Logout()
        {
            IsLoading = true;

            try
            {
                await NavigationService.NavigateToAsync<LoginViewModel>("Logout");
            }
            catch (Exception e)
            {
                await DialogService.ShowAlertAsync(e.Message, "Error", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected abstract Task MainAction();

        protected virtual void InitializeSpecificStep(Status status)
        {

        }



    }
}
