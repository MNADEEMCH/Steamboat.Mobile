using System;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.User;

namespace Steamboat.Mobile.Repositories.User
{
    public interface IUserRepository
    {
        Task<CurrentUser> AddUser(string email);

        Task<CurrentUser> UpdateUser(Guid id, string email);

        Task<CurrentUser> GetCurrentUser();
    }
}
