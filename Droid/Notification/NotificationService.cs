using System;
using Firebase.Iid;
using Steamboat.Mobile.Services.Notification;

namespace Steamboat.Mobile.Droid.Notification
{
    public class NotificationService:INotificationService
    {
        public static int notificationCount = 0;

        public string GetToken()
        {
            return FirebaseInstanceId.Instance.Token;
        }

        public bool IsValidToken()
        {
            return FirebaseInstanceId.Instance!=null && !String.IsNullOrEmpty(FirebaseInstanceId.Instance.Token);
        }

        public void HandleNotificationBadge()
        {
            //ShortcutBadger.ApplyCount(this.ApplicationContext, 0);
        }
    }
}
