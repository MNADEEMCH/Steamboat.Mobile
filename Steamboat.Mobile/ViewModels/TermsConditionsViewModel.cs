using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Helpers.Settings;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
	public class TermsConditionsViewModel : ViewModelBase
	{

		#region Properties

		private ISettings _settings;
		private readonly string _reportUri;
		private string _webViewUri;
		private bool _webViewLoadedSucessfully = false;

		public bool WebViewLoadedSucessfully { set { SetPropertyValue(ref _webViewLoadedSucessfully, value); } get { return _webViewLoadedSucessfully; } }
		public ICommand LoadFinishedCommand { get; set; }
		public string WebViewUri { set { SetPropertyValue(ref _webViewUri, value); } get { return _webViewUri; } }
		public CookieContainer WebViewCookies { get; set; }
		public Dictionary<string, string> WebViewHeaders { get; set; }

		#endregion

		public TermsConditionsViewModel(ISettings settings = null)
		{
			IsLoading = true;
			_settings = settings ?? DependencyContainer.Resolve<ISettings>();
			_reportUri = _settings.BaseUrl + "privacy";

			SetWebView();
		}

		public async override Task InitializeAsync(object parameter)
		{
			await base.InitializeAsync(parameter);
		}

		private void SetWebView()
		{
			WebViewLoadedSucessfully = false;
            WebViewUri = _reportUri;
			SetWebViewCookies();
            SetWebViewHeaders();

			TryExecute(async () =>
			{								
				LoadFinishedCommand = new Command(async (loadedSuccessfully) => await LoadFinished((bool)loadedSuccessfully));
			},
		    async ex =>
			{
				await LoadFinished(false);
			}, null);
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
			WebViewHeaders.Add("Momentum-Webpage-Header", "false");
		}

		private string GetDomain(string fullUrl)
		{
			var ret = fullUrl.Replace("http://", "").Replace("https://", "");
			return ret.Substring(0, ret.IndexOf("/"));
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
