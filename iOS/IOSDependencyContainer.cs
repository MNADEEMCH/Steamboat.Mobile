using System;
using Splat;
using Steamboat.Mobile.iOS.Notification;
using Steamboat.Mobile.iOS.Utilities;
using Steamboat.Mobile.Repositories.Database;
using Steamboat.Mobile.Services.Notification;

namespace Steamboat.Mobile.iOS
{
    public class IOSDependencyContainer
    {
        public static void RegisterDependencies()
        {
            Locator.CurrentMutable.RegisterConstant(new SQLiteHelper(), typeof(IConnectionHelper));
            Locator.CurrentMutable.RegisterConstant(new NotificationService(), typeof(INotificationService));
        }
    }
}
