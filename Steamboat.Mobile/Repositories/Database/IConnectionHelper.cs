using System;
using SQLite;

namespace Steamboat.Mobile.Repositories.Database
{
    public interface IConnectionHelper
    {
        SQLiteAsyncConnection GetConnection(string path);
    }
}
