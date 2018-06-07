using System;
using Android.App;
using Firebase.Iid;
using Steamboat.Mobile.Droid.CustomRenderers;
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
            ShortcutBadger.ApplyCount(MainActivity.Context, badge);
            if (badge == 0)
                RemoveAllNotificationBubbles();
        }

        private void RemoveAllNotificationBubbles()
        {
            NotificationManager notifManager = (NotificationManager)MainActivity.Context.GetSystemService(Java.Lang.Class.FromType(typeof(Android.App.NotificationManager)));
            notifManager.CancelAll();
           
        }

        public void SetMasterDetailMenuIcon(string menuIcon)
        {
            menuIcon=System.IO.Path.GetFileNameWithoutExtension(menuIcon).ToLower();
            var menuIconId = MainActivity.Context.Resources.GetIdentifier(menuIcon, "drawable", MainActivity.Context.PackageName);
            IconMasterDetailPageRenderer.ChangeMenuIcon(menuIconId);
        }
    }
}
