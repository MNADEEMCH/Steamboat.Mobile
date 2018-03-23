using System;
using Android.App;
using Firebase.Iid;
using Steamboat.Mobile.Services.Notification;
using Xamarin.ShortcutBadger;

namespace Steamboat.Mobile.Droid.Services
{
    public class NotificationService : INotificationService
    {
        public string GetToken()
        {
            return FirebaseInstanceId.Instance.Token;
        }

        public bool IsValidToken()
        {
            return FirebaseInstanceId.Instance != null && !String.IsNullOrEmpty(FirebaseInstanceId.Instance.Token);
        }

        public void SetNotificationBadge(int badge=0)
        {
            ShortcutBadger.ApplyCount(MainActivity.getContext(), badge);
        }

    }
}
