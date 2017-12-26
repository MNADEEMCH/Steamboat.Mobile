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
        Task<bool> Logout();
        Task<AccountLogin> InitPassword(string password, string confirm);
    }
}
