using System;
using Splat;
using Steamboat.Mobile.Droid.Utilities;
using Steamboat.Mobile.Repositories.Database;

namespace Steamboat.Mobile.Droid
{
    public class AndroidDependencyContainer
    {
        public static void RegisterDependencies()
        {
            Locator.CurrentMutable.RegisterConstant(new SQLiteHelper(), typeof(IConnectionHelper));
        }
    }
}
