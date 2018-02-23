using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Steamboat.Mobile.Models;

namespace Steamboat.Mobile.Repositories.Database
{
    public interface IDatabase<T> where T : EntityBase
    {
        Task<int> Insert(T item);

        Task<int> Delete(T item);

        Task<int> Update(T item);

        Task<T> GetFirst(Expression<Func<T, bool>> predicate=null);

        Task<List<T>> Select(Expression<Func<T, bool>> predicate=null);

        Task<T> Select(Guid identifier);

    }
}
