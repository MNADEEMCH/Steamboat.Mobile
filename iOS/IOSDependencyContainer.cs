using System;
using Splat;
using Steamboat.Mobile.iOS.Utilities;
using Steamboat.Mobile.Repositories.Database;

namespace Steamboat.Mobile.iOS
{
    public class IOSDependencyContainer
    {
        public static void RegisterDependencies()
        {
            Locator.CurrentMutable.RegisterConstant(new SQLiteHelper(), typeof(IConnectionHelper));
        }
    }
}
