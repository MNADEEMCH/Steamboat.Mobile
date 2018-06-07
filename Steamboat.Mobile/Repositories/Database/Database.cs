using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SQLite;
using Steamboat.Mobile.Models;

namespace Steamboat.Mobile.Repositories.Database
{
    public class Database<T> : IDatabase<T> where T : EntityBase, new()
    {
        private IConnectionHelper _connectionHelper;
        protected SQLiteAsyncConnection database;

        public Database(IConnectionHelper connectionHelper = null)
        {
            _connectionHelper = connectionHelper ?? DependencyContainer.Resolve<IConnectionHelper>();
            database = _connectionHelper.GetConnection("steamboat_db.db3");
            Task.Run(() => { database.CreateTableAsync<T>(); });
        }

        public async Task<int> Insert(T item)
        {
            return await database.InsertAsync(item);
        }

        public async Task<int> Delete(T item)
        {
            return await database.DeleteAsync(item);
        }

        public async Task<int> Update(T item)
        {
            var list = new List<T>() { item };
            return await database.UpdateAllAsync(list);
        }

        public async Task<T> GetFirst(Expression<Func<T, bool>> predicate = null)
        {
            var tb = database.Table<T>();
            if(predicate!=null)
                return await tb.Where(predicate).FirstOrDefaultAsync();
            else
                return await tb.FirstOrDefaultAsync();
        }

        public async Task<List<T>> Select(Expression<Func<T, bool>> predicate=null)
        {
            var tb = database.Table<T>();

            if (predicate != null)
                return await tb.Where(predicate).ToListAsync();
            else
                return await tb.ToListAsync();
        }

        public async Task<T> Select(Guid identifier)
        {
            AsyncTableQuery<T> tb = database.Table<T>();
            var item = tb.Where(x => x.Id == identifier);
            return await item.FirstAsync();
        }

        public async Task<T> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return await database.Table<T>().Where(predicate).FirstOrDefaultAsync();
        }

    }
}
