using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V7.App;
using Firebase.Messaging;
using Steamboat.Mobile.Models.NavigationParameters;
using System.Linq;
using ME.Leolin.Shortcutbadger;

namespace Steamboat.Mobile.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";

        public override void OnMessageReceived(RemoteMessage message)
        {
            Android.Util.Log.Debug(TAG, "From: " + message.From);
            Android.Util.Log.Debug(TAG, "Notification Message Body: " + message.GetNotification().Body);
            ShortcutBadger.ApplyCount(this, 1);
            var pushNotificationParameter = new PushNotificationParameter() { PruebaPush = string.Join(",", message.Data.Keys.ToList()) };
            Task.Run(async()=>await App.HandlePushNotification(pushNotificationParameter));
            SendNotification(message.GetNotification().Body, message.Data);
        }

        public void SendNotification(string messageBody, IDictionary<string, string> data)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);

            foreach (string key in data.Keys)
            {
                intent.PutExtra(key, data[key]);
            }

            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.UpdateCurrent);

            var notificationBuilder = new NotificationCompat.Builder(this)
                .SetContentText(messageBody)
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.Notify(0, notificationBuilder.Build());


        }
    }
}
