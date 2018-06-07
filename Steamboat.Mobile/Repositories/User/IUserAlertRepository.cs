using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.User;

namespace Steamboat.Mobile.Repositories.User
{
    public interface IUserAlertRepository
    {
        Task<int> AddUserAlert(UserAlert userAlert);

        Task<List<UserAlert>> GetUserAlert(string username);
    }
}
