using System;
using Steamboat.Mobile.Services.Notification;
using Firebase;
using Foundation;

namespace Steamboat.Mobile.iOS.Notification
{
    public class NotificationService : INotificationService
    {
        public static int notificationCount = 0;

        public string GetToken()
        {   
            return NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken"); 
        }

        public bool IsValidToken()
        {
            return NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken") != null;
        }

        public void HandleNotificationBadge()
        {
            //TO MODIFY BADGE FROM APP
            //UIApplication.SharedApplication.ApplicationIconBadgeNumber = badge;
        }
    }
}
