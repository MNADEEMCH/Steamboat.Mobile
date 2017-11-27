using System;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.Account;
using Steamboat.Mobile.Services.RequestProvider;

namespace Steamboat.Mobile.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IRequestProvider _requestProvider;
        private const string ApiUrlBase = "account/login";

        public AccountService(IRequestProvider requestProvider = null)
        {
            _requestProvider = requestProvider ?? DependencyContainer.Resolve<IRequestProvider>();
        }

        public async Task<AccountInfo> AccountLogin(AccountLogin loginCredentials)
        {
            string url = "https://dev.momentumhealth.co/account/login";

            try
            {
                var result = await _requestProvider.PostAsync<AccountInfo, AccountLogin>(url, loginCredentials);
                return result;
            } 
            catch(Exception) {
                return new AccountInfo();
            }
        }
    }
}
