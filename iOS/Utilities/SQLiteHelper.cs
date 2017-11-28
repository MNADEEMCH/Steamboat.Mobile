using System;
using System.IO;
using SQLite;
using Steamboat.Mobile.Repositories.Database;

namespace Steamboat.Mobile.iOS.Utilities
{
    public class SQLiteHelper : IConnectionHelper
    {
        public SQLiteAsyncConnection GetConnection(string filename)
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }
            string fileFolder = Path.Combine(libFolder, filename);
            return new SQLiteAsyncConnection(fileFolder);
        }
    }
}
