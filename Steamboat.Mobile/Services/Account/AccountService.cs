using System;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.Account;
using Steamboat.Mobile.Services.RequestProvider;

namespace Steamboat.Mobile.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IRequestProvider _requestProvider;
        private const string ApiUrlBase = "https://dev.momentumhealth.co/account/";

        public AccountService(IRequestProvider requestProvider = null)
        {
            _requestProvider = requestProvider ?? DependencyContainer.Resolve<IRequestProvider>();
        }

        public async Task<AccountInfo> AccountLogin(AccountLogin loginCredentials)
        {
            string url = string.Format(ApiUrlBase + "{0}", "login");

            return await _requestProvider.PostAsync<AccountInfo, AccountLogin>(url, loginCredentials);
        }

        public async Task<AccountLogout> AccountLogout(string sessionId)
        {
            string url = string.Format(ApiUrlBase + "{0}", "logout");

            return await _requestProvider.PostAsync<AccountLogout>(url, sessionID:sessionId);
        }

        public async Task<AccountLogin> AccountInitPassword(AccountInitPassword passwords, string sessionId)
        {
            string url = string.Format(ApiUrlBase + "{0}", "changepassword");

            return await _requestProvider.PostAsync<AccountLogin, AccountInitPassword>(url, passwords, sessionId);
        }
    }
}
