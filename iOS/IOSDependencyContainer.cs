using System;
using Splat;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.iOS.Helpers;
using Steamboat.Mobile.iOS.Services;
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
			Locator.CurrentMutable.RegisterConstant<IDeviceInfo>(new DeviceInfo());
        }
    }
}
