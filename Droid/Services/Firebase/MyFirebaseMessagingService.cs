using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V7.App;
using Firebase.Messaging;
using System.Linq;
using Steamboat.Mobile.Models.Notification;
using Steamboat.Mobile.Droid.Helpers;
using Newtonsoft.Json;
using Steamboat.Mobile.Services.Notification;

namespace Steamboat.Mobile.Droid.Services.Firebase
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
		public override void OnCreate()
		{
			base.OnCreate();
            Xamarin.ShortcutBadger.ShortcutBadger.ApplyCount(this.ApplicationContext, 1);
		}

		public override void OnMessageReceived(RemoteMessage message)
        {

            var pushNotification = NotificationHelper.TryGetPushNotification(message);
            if (pushNotification != null)
            {
                Task.Run(async () => await App.HandlePushNotification(pushNotification));
                SendNotification(pushNotification);
            }

        }

        public void SendNotification(PushNotification pushNotification)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.SingleTop|ActivityFlags.ClearTop);
           
            //TODO: the extras doesnt get refreshed, they should be refreshed when PendingIntentFlags.UpdateCurrent is present
            //var jsonPushNotification = JsonConvert.SerializeObject(pushNotification);
            //intent.PutExtra("momentum_health", jsonPushNotification);

            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.UpdateCurrent);

            var notificationBuilder = new NotificationCompat.Builder(this)
                .SetContentText(pushNotification.Message)
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent)
                .SetNumber(pushNotification.Badge);//for Android 8.0

            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.Notify(0, notificationBuilder.Build());


        }
    }
}
