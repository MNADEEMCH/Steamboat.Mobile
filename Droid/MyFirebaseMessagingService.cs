using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V7.App;
using Firebase.Messaging;

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
            SendNotification(message.GetNotification().Body, message.Data);
        }

        void SendNotification(string messageBody, IDictionary<string, string> data)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            intent.PutExtra("saracatanga", "siseor");
            /*foreach (string key in data.Keys)
            {
                intent.PutExtra(key, data[key]);
            }*/

            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(this)
                .SetContentTitle("FCM Message")
                .SetContentText(messageBody)
                .SetAutoCancel(true)
                
                .SetNumber(1)                                             
                .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.Notify(0, notificationBuilder.Build());
        }
    }
}
