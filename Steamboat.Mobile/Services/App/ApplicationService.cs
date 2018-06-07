using System;
using System.Threading.Tasks;
using Steamboat.Mobile.Helpers.Settings;
using Steamboat.Mobile.Models.Application;
using Steamboat.Mobile.Services.RequestProvider;

namespace Steamboat.Mobile.Services.App
{
    public class ApplicationService : IApplicationService
    {
        private readonly IRequestProvider _requestProvider;
        private readonly string ApiUrlBase;

		public ApplicationService(IRequestProvider requestProvider = null, ISettings settings = null)
        {
            _requestProvider = requestProvider ?? DependencyContainer.Resolve<IRequestProvider>();
			var _settings = settings ?? DependencyContainer.Resolve<ISettings>();
			ApiUrlBase = _settings.BaseUrl + "device/";
        }

        public async Task<ApplicationDeviceInfo> SendToken(ApplicationDeviceInfo applicationDeviceInfo,string sessionId)
        {
            string url = string.Format(ApiUrlBase + "{0}", "register");

            return await _requestProvider.PostAsync<ApplicationDeviceInfo>(url, applicationDeviceInfo, sessionId);
        }
    }
}
