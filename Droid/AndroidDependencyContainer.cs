using System;
using Splat;
using Steamboat.Mobile.Droid.Services;
using Steamboat.Mobile.Droid.Utilities;
using Steamboat.Mobile.Repositories.Database;
using Steamboat.Mobile.Services.Notification;

namespace Steamboat.Mobile.Droid
{
    public class AndroidDependencyContainer
    {
        public static void RegisterDependencies()
        {
            Locator.CurrentMutable.RegisterConstant(new SQLiteHelper(), typeof(IConnectionHelper));
            Locator.CurrentMutable.RegisterConstant(new NotificationService(), typeof(INotificationService));
        }
    }
}
