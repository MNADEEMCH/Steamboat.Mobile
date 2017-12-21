using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Steamboat.Mobile.Models;

namespace Steamboat.Mobile.Repositories.Database
{
    public interface IDatabase<T> where T : EntityBase
    {
        Task<int> Insert(T item);

        Task<int> Delete(T item);

        Task<List<T>> Select();

        Task<T> Select(Guid identifier);

        Task<T> GetFirst();

        Task<int> Update(T item);
    }
}
