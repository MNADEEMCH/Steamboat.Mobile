using System;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.Account;

namespace Steamboat.Mobile.Services.Account
{
    public interface IAccountService
    {
        Task<AccountInfo> AccountLogin(AccountLogin loginCredentials);

        Task<AccountLogout> AccountLogout(string sessionId);

        Task<AccountLogin> AccountInitPassword(AccountInitPassword passwords);
    }
}
