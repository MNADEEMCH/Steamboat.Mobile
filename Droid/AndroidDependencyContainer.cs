using System;
using Splat;
using Steamboat.Mobile.Droid.Helpers;
using Steamboat.Mobile.Droid.Services;
using Steamboat.Mobile.Droid.Services.Orientation;
using Steamboat.Mobile.Droid.Utilities;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Repositories.Database;
using Steamboat.Mobile.Services.Notification;
using Steamboat.Mobile.Services.Orientation;

namespace Steamboat.Mobile.Droid
{
    public class AndroidDependencyContainer
    {
        public static void RegisterDependencies()
        {
            Locator.CurrentMutable.RegisterConstant(new SQLiteHelper(), typeof(IConnectionHelper));
            Locator.CurrentMutable.RegisterConstant(new NotificationService(), typeof(INotificationService));
            Locator.CurrentMutable.RegisterLazySingleton(() => new DeviceOrientationService(), typeof(IDeviceOrientationService));
            Locator.CurrentMutable.RegisterConstant<IDeviceInfo>(new DeviceInfo());
        }

        public static T Resolve<T>()
        {
            return Locator.CurrentMutable.GetService<T>();
        }
    }
}
