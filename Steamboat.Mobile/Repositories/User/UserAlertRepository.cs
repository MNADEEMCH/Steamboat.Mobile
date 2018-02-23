using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.User;
using Steamboat.Mobile.Repositories.Database;

namespace Steamboat.Mobile.Repositories.User
{
    public class UserAlertRepository:IUserAlertRepository
    {

        private IDatabase<UserAlert> _database;

        public UserAlertRepository(IDatabase<UserAlert> db = null)
        {
            _database = db ?? DependencyContainer.Resolve<IDatabase<UserAlert>>();
        }

        public async Task<int> AddUserAlert(UserAlert userAlert)
        {
            userAlert.Id = Guid.NewGuid();
            return await _database.Insert(userAlert);
        }

        public async Task<List<UserAlert>> GetUserAlert(string username)
        {
            return await _database.Select((x)=>(x.UserName.Equals(username)));
        }

    }
}
