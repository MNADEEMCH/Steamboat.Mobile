using System;
using Firebase.Iid;
using Steamboat.Mobile.Services.Notification;

namespace Steamboat.Mobile.Droid.Notification
{
    public class NotificationService:INotificationService
    {
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
