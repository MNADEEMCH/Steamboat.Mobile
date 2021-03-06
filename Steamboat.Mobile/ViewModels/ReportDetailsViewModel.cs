using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net;
using Xamarin.Forms;
using System.Collections.Generic;
using Steamboat.Mobile.Models.Application;
using Steamboat.Mobile.Helpers.Settings;
using Steamboat.Mobile.ViewModels.Interfaces;
using Steamboat.Mobile.Managers.Application;

namespace Steamboat.Mobile.ViewModels
{
	public class ReportDetailsViewModel : ViewModelBase, IHandleViewAppearing, IHandleViewDisappearing
    {
        #region Properties
        
		private IApplicationManager _applicationManager;
		private ISettings _settings;
        private readonly string _reportUri;
        private bool _webViewLoadedSucessfully = false;

        public bool WebViewLoadedSucessfully { set { SetPropertyValue(ref _webViewLoadedSucessfully, value); } get { return _webViewLoadedSucessfully; } }
        public ICommand LoadFinishedCommand { get; set; }
        public string WebViewReportUri { get; set; }
        public CookieContainer WebViewCookies { get; set; }
        public Dictionary<string, string> WebViewHeaders { get; set; }

        #endregion

		public ReportDetailsViewModel(IApplicationManager applicationManager = null, ISettings settings = null)
        {
            IsLoading = true;
			_applicationManager = applicationManager ?? DependencyContainer.Resolve<IApplicationManager>();
			_settings = settings ?? DependencyContainer.Resolve<ISettings>();
			_reportUri = _settings.BaseUrl + "participant/report";

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
			cookie.Domain = GetDomain(_reportUri);
            WebViewCookies.Add(new Uri(_reportUri), cookie);
        }

        private void SetWebViewHeaders()
        {
            WebViewHeaders = new Dictionary<string, string>();
            WebViewHeaders.Add("Momentum-Api", "true");
			WebViewHeaders.Add("Momentum-Api-Environment", _settings.RequestProviderApiEnvironment);
            WebViewHeaders.Add("Momentum-Api-Session", App.SessionID);
        }

		private string GetDomain(string fullUrl)
		{
			var ret = fullUrl.Replace("http://", "").Replace("https://","");
			return ret.Substring(0,ret.IndexOf("/"));
		}

        public async Task LoadFinished(bool loadedSuccessfully)
        {
            IsLoading = false;
            WebViewLoadedSucessfully = loadedSuccessfully;

            if (!loadedSuccessfully)
            {
                await DialogService.ShowAlertAsync("Loaded with errors", "Error", "OK");
            }
            else
                await _applicationManager.ChangeMenuOrder();

            await Task.FromResult(true);
        }

		public async Task OnViewAppearingAsync(VisualElement view)
		{
			_applicationManager.IncreaseTimer();
			await Task.FromResult(true);
		}

		public async Task OnViewDisappearingAsync(VisualElement view)
		{
			_applicationManager.RestartTimer();
			_applicationManager.DecreaseTimer();
			await Task.FromResult(true);
		}
	}
}
