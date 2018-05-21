using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net;
using Xamarin.Forms;
using System.Collections.Generic;
using Steamboat.Mobile.Models.Application;

namespace Steamboat.Mobile.ViewModels
{
    public class ReportDetailsViewModel : ViewModelBase
    {
        #region Properties

        private string _reportUri = "https://dev.momentumhealth.co/participant/report";
        private bool _webViewLoadedSucessfully = false;

        public bool WebViewLoadedSucessfully { set { SetPropertyValue(ref _webViewLoadedSucessfully, value); } get { return _webViewLoadedSucessfully; } }
        public ICommand LoadFinishedCommand { get; set; }
        public string WebViewReportUri { get; set; }
        public CookieContainer WebViewCookies { get; set; }
        public Dictionary<string, string> WebViewHeaders { get; set; }

        #endregion

        public ReportDetailsViewModel()
        {
            IsLoading = true;
            SetWebView();
        }

        public async override Task InitializeAsync(object parameter)
        {
            await base.InitializeAsync(parameter);
        }

        private void SetWebView()
        {
            try
            {
                WebViewLoadedSucessfully = false;
                WebViewReportUri = _reportUri;
                SetWebViewCookies();
                SetWebViewHeaders();
                LoadFinishedCommand = new Command(async (loadedSuccessfully) => await LoadFinished((bool)loadedSuccessfully));
            }
            catch (Exception ex)
            {
                Task.Run(async () => await LoadFinished(false));
            }

        }

        private void SetWebViewCookies()
        {
            WebViewCookies = new CookieContainer();
            var cookie = new Cookie();
            cookie.Name = "ASP.NET_SessionId";
            cookie.Value = App.SessionID;
            cookie.Path = "/";
            cookie.Domain = "dev.momentumhealth.co";
            WebViewCookies.Add(new Uri(_reportUri), cookie);
        }

        private void SetWebViewHeaders()
        {
            WebViewHeaders = new Dictionary<string, string>();
            WebViewHeaders.Add("Momentum-Api", "true");
            WebViewHeaders.Add("Momentum-Api-Environment", "F5752008-E484-4691-B58A-3338A90F80AA");
            WebViewHeaders.Add("Momentum-Api-Session", App.SessionID);
        }

        public async Task LoadFinished(bool loadedSuccessfully)
        {
            IsLoading = false;
            WebViewLoadedSucessfully = loadedSuccessfully;

            if (!loadedSuccessfully)
            {
                await DialogService.ShowAlertAsync("Loaded with errors", "Error", "OK");
            }

            await Task.FromResult(true);
        }
    }
}
