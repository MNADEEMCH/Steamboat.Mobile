using System;
using System.IO;
using SQLite;
using Steamboat.Mobile.Repositories.Database;

namespace Steamboat.Mobile.Droid.Utilities
{
    public class SQLiteHelper : IConnectionHelper
    {
        public SQLiteAsyncConnection GetConnection(string path)
        {
            var sqliteFilename = path;
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var fullPath = Path.Combine(documentsPath, sqliteFilename);
            var conn = new SQLiteAsyncConnection(fullPath);
            return conn;
        }
    }
}
