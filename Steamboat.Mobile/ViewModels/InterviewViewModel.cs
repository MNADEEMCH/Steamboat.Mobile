﻿using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Models.Participant;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class InterviewViewModel : ViewModelBase
    {

        #region Properties

        public ICommand LogoutCommand { get; set; }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetPropertyValue(ref _description, value); }
        }

        #endregion

        public InterviewViewModel()
        {
            IsLoading = true;
            LogoutCommand = new Command(async () => await Logout());
        }

        public async override Task InitializeAsync(object navigationData)
        {
            Status status = navigationData as Status;
            if (ValidateStatus(status))
            {
                SurveyStep surveyStep = status.Dashboard.SurveyStep;
                Description = surveyStep.Message;

            }
            else
            {
                //TODO: Improve handle error
                await this.DialogService.ShowAlertAsync("Error loading", "Error", "OK");             

            }

            IsLoading = false;
        }
        private bool ValidateStatus(Status status)
        {
            return status != null
                    && status.Dashboard != null
                    && status.Dashboard.SurveyStep != null;
        }

        private async Task Logout()
        {
            IsLoading = true;

            try
            {
                //var res = await _accountManager.Logout();
                //if (res)
                //{
                    await NavigationService.NavigateToAsync<LoginViewModel>();
                //}
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
    }
}
