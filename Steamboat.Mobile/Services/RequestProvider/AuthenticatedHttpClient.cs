using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Steamboat.Mobile.Services.RequestProvider
{
    public class AuthenticatedHttpClient : HttpClientHandler
    {
        public AuthenticatedHttpClient()
        {
            UseCookies = false;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Cookie", $"ASP.NET_SessionId={Mobile.App.SessionID}");
            request.Headers.Add("Momentum-Api-Session", Mobile.App.SessionID);
            request.Headers.Add("Momentum-Api", "true");
            request.Headers.Add("Momentum-Api-Environment", "F5752008-E484-4691-B58A-3338A90F80AA");

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
