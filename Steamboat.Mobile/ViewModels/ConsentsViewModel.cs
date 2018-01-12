using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Managers.Participant;
using Steamboat.Mobile.Models.Participant;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class ConsentsViewModel : ViewModelBase
    {
        #region Properties

        private string _title;
        private string _body;
        private string _affirmationText;
        private string _rejectionText;
        private string _consentNumber;
        private bool _isBusy;
        private bool _isFirstCheck;
        private bool _isEnabled;
        private bool _isAccepted;
        private bool _isRejected;
        private IParticipantManager _participantManager;
        private List<Consent> _consents;
        private Consent _currentConsent;

        public ICommand ContinueCommand { get; set; }
        public ICommand AffirmationCommand { get; set; }
        public ICommand RejectionCommand { get; set; }
        public string Title { set { SetPropertyValue(ref _title, value); } get { return _title; } }
        public string Body { set { SetPropertyValue(ref _body, value); } get { return _body; } }
        public string AffirmationText { set { SetPropertyValue(ref _affirmationText, value); } get { return _affirmationText; } }
        public string RejectionText { set { SetPropertyValue(ref _rejectionText, value); } get { return _rejectionText; } }
        public string ConsentNumber { set { SetPropertyValue(ref _consentNumber, value); } get { return _consentNumber; } }
        public bool IsBusy { set { SetPropertyValue(ref _isBusy, value); } get { return _isBusy; } }
        public bool IsFirstCheck { set { SetPropertyValue(ref _isFirstCheck, value); } get { return _isFirstCheck; } }
        public bool IsEnabled { set { SetPropertyValue(ref _isEnabled, value); } get { return _isEnabled; } }
        public bool IsAccepted { set { SetPropertyValue(ref _isAccepted, value); } get { return _isAccepted; } }
        public bool IsRejected { set { SetPropertyValue(ref _isRejected, value); } get { return _isRejected; } }

        #endregion

        public ConsentsViewModel(IParticipantManager participantManager = null)
        {
            IsLoading = true;
            IsFirstCheck = false;
            _participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();

            ContinueCommand = new Command(async () => await this.OpenConsent());
            AffirmationCommand = new Command(() => this.AffirmationTap());
            RejectionCommand = new Command(() => this.RejectionTap());
            Title = Body = AffirmationText = RejectionText = string.Empty;
            IsAccepted = IsRejected = false;
        }

        public async override Task InitializeAsync(object parameter)
        {
            try
            {
                if (parameter == null)
                    _consents = await _participantManager.GetConsents();
                else
                    _consents = parameter as List<Consent>;

                _currentConsent = _consents.Where(p => !p.IsCompleted).FirstOrDefault();

                if (_currentConsent != null)
                {
                    Title = _currentConsent.Title;
                    Body = _currentConsent.Body;
                    AffirmationText = _currentConsent.Affirmation ?? string.Empty;
                    RejectionText = _currentConsent.Rejection ?? string.Empty;
                    ConsentNumber = GetConsentNumberFormat(_consents);
                }
                else
                {
                    await NavigateToStatusView();    
                }
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

        private async Task OpenConsent()
        {
            StartBusy();

            try
            {
                if (ValidateAnswer())
                {
                    SetCompleted();
                    if (!_consents.Any(p => !p.IsCompleted))
                    {
                        var remainingConsents = await _participantManager.SendConsents(_consents);
                        if(remainingConsents.Any())
                        {
                            await NavigationService.NavigateToAsync<ConsentsViewModel>(remainingConsents);
                        }
                        else
                        {
                            await NavigateToStatusView();
                        }
                    }
                    else
                        await NavigationService.NavigateToAsync<ConsentsViewModel>(_consents);
                }
                else
                {
                    await DialogService.ShowAlertAsync("Your consent is required to continue.", "Error", "OK");
                }
            }
            catch (Exception e)
            {
                await DialogService.ShowAlertAsync(e.Message, "Error", "OK");
            }
            finally
            {
                StopBusy();
            }
        }

        private async Task NavigateToStatusView()
        {
            var status = await _participantManager.GetStatus();
            var viewModelType = DashboardStatusHelper.GetViewModelForStatus(status);
            await NavigationService.NavigateToAsync(viewModelType, status, mainPage: true);
        }

        private void AffirmationTap()
        {
            if (!IsFirstCheck)
                IsFirstCheck = true;

            if (IsRejected)
                IsRejected = false;

            IsEnabled = IsAccepted || IsRejected;
        }

        private void RejectionTap()
        {
            if (!IsFirstCheck)
                IsFirstCheck = true;

            if (IsAccepted)
                IsAccepted = false;

            IsEnabled = IsAccepted || IsRejected;
        }

        private void StartBusy()
        {
            IsBusy = true;
            IsFirstCheck = false;
        }

        private void StopBusy()
        {
            IsBusy = false;
            IsFirstCheck = true;
        }

        private void SetCompleted()
        {
            _currentConsent.IsCompleted = true;
            _currentConsent.IsAccepted = IsAccepted;
            _currentConsent.IsRejected = IsRejected;
            var nextConsent = _consents.Where(p => p.ConsentID > _currentConsent.ConsentID).FirstOrDefault();
            if (nextConsent != null)
                nextConsent.IsCompleted = false;
        }

        private bool ValidateAnswer()
        {
            return (_currentConsent.IsAffirmationRequired && IsAccepted) || (_currentConsent.IsRejectionAllowed && (IsAccepted || IsRejected));
        }

        private string GetConsentNumberFormat(List<Consent> consents)
        {
            var current = consents.Where(c => c.IsCompleted).Count() + 1;
            var total = consents.Count();
            return string.Format("CONSENT {0} OF {1}", current, total);
        }
    }
}
