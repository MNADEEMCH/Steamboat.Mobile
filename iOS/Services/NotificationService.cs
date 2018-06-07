using System;
using Foundation;
using Steamboat.Mobile.Services.Notification;
using UIKit;

namespace Steamboat.Mobile.iOS.Services
{
    public class NotificationService : INotificationService
    {
        public string GetToken()
        {
            return NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");
        }

        public bool IsValidToken()
        {
            return NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken") != null;
        }

        public void SetNotificationBadge(int badge)
        {
            //TO MODIFY BADGE FROM APP
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = badge;
        }

        public void SetMasterDetailMenuIcon(string menuIcon)
        {
            
        }

    }
}
