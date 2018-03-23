using System;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.Application;
using Steamboat.Mobile.Services.RequestProvider;

namespace Steamboat.Mobile.Services.App
{
    public class ApplicationService : IApplicationService
    {
        private readonly IRequestProvider _requestProvider;
        private const string ApiUrlBase = "https://dev.momentumhealth.co/device/";

        public ApplicationService(IRequestProvider requestProvider = null)
        {
            _requestProvider = requestProvider ?? DependencyContainer.Resolve<IRequestProvider>();
        }

        public async Task<ApplicationDeviceInfo> SendToken(ApplicationDeviceInfo applicationDeviceInfo,string sessionId)
        {
            string url = string.Format(ApiUrlBase + "{0}", "register");

            return await _requestProvider.PostAsync<ApplicationDeviceInfo>(url, applicationDeviceInfo, sessionId);
        }
    }
}
