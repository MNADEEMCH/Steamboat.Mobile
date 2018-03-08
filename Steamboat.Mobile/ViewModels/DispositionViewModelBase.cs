﻿using Steamboat.Mobile.Helpers;
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
using Badge.Plugin;

namespace Steamboat.Mobile.ViewModels
{
    public abstract class DispositionViewModelBase : ViewModelBase
    {
        #region properties

        private IAccountManager _accountManager;
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

        public DispositionViewModelBase(StepperViewModel stepperViewModel = null, AccountManager accountManager = null)
        {
            _accountManager = accountManager ?? DependencyContainer.Resolve<IAccountManager>();
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
               
                CrossBadge.Current.SetBadge(0);

            }
            catch (Exception e)
            {
                //await this.DialogService.ShowAlertAsync(e.Message, "Error", "OK");
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

            await ShowAndStoreAlert(status);

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

        private async Task ShowAndStoreAlert(Status status)
        {
            Alert alertToShow = await IsAnyAlertToShow(status);

            if (alertToShow != null)
            {
                ModalParam modalToShow = CreateModalParam(alertToShow);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await ModalService.PushAsync<WelcomeModalViewModel>(modalToShow);
                });
                await _accountManager.AddUserAlert(App.CurrentUser.Email,alertToShow.ID);
            }
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
        private async Task<bool> UserAlreadySawTheAlert(Alert alert){

            bool userAlreadySawTheAlert = false;

            try
            {
                UserAlerts userAlerts = await _accountManager.GetUserAlerts(App.CurrentUser.Email);
                userAlreadySawTheAlert = userAlerts != null && userAlerts.AlertsIds.Contains(alert.ID);
            }
            catch (Exception e)
            {
                await this.DialogService.ShowAlertAsync(e.Message, "Error", "OK");
            }

            return userAlreadySawTheAlert;
        }
        private ModalParam CreateModalParam(Alert alert){

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
