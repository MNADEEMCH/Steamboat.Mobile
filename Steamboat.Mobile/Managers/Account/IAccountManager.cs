using System;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.Account;
using Steamboat.Mobile.Models.User;

namespace Steamboat.Mobile.Managers.Account
{
    public interface IAccountManager
    {
        Task<AccountInfo> Login(string username, string password);
        Task<CurrentUser> GetLocalUser();
        Task<bool> Logout(bool callBackend=true);
        Task<AccountLogin> InitPassword(string password, string confirm);

        Task<int> AddUserAlert(string username,int alertId);
        Task<UserAlerts> GetUserAlerts(string username);
    }
}
