using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Steamboat.Mobile.Helpers.Settings;

namespace Steamboat.Mobile.Services.RequestProvider
{
    public class AuthenticatedHttpClient : HttpClientHandler
    {
		private ISettings _settings;

		public AuthenticatedHttpClient(ISettings settings = null)
        {
            UseCookies = false;
			_settings = settings ?? DependencyContainer.Resolve<ISettings>();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Cookie", $"ASP.NET_SessionId={Mobile.App.SessionID}");
            request.Headers.Add("Momentum-Api-Session", Mobile.App.SessionID);
            request.Headers.Add("Momentum-Api", "true");
			request.Headers.Add("Momentum-Api-Environment", _settings.RequestProviderApiEnvironment);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
