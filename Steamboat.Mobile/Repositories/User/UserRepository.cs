using System;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.User;
using Steamboat.Mobile.Repositories.Database;

namespace Steamboat.Mobile.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private IDatabase<CurrentUser> _database;

        public UserRepository(IDatabase<CurrentUser> db = null)
        {
            _database = db ?? DependencyContainer.Resolve<IDatabase<CurrentUser>>();
        }

        public async Task<CurrentUser> AddUser(string email, string avatarUrl)
        {
            CurrentUser user = new CurrentUser();
            user.Id = Guid.NewGuid();
            user.Email = email;
            user.AvatarUrl = avatarUrl;
            var all = await _database.Select();
            var ins = await _database.Insert(user);

            //TODO: Check ins value and throw
            return user;
        }

        public async Task<CurrentUser> GetCurrentUser()
        {
            return await _database.GetFirst();
        }

        public async Task<CurrentUser> UpdateUser(Guid id, string email, string avatarUrl)
        {
            CurrentUser user = new CurrentUser();
            user.Id = id;
            user.Email = email;
            user.AvatarUrl = avatarUrl;
            var upd = await _database.Update(user);

            //TODO: Check upd value and throw
            return user;
        }
    }
}
