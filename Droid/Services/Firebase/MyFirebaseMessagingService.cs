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
using Steamboat.Mobile.Helpers;
using XamarinShortcutBadger;

namespace Steamboat.Mobile.Droid.Services.Firebase
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
		public override void OnCreate()
		{
			base.OnCreate();
		}

		public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);

            var pushNotification = NotificationHelper.TryGetPushNotification(message);
            if (pushNotification != null)
            {
                if(!MainApplication.IsForeground())
                    ShowNotification(pushNotification);
                
                Task.Run(async () => await App.HandlePushNotification(false,MainActivity.IsAppBackgrounded,pushNotification));
            }

        }

        public void ShowNotification(PushNotification pushNotification)
        {
            var intent = new Intent(this, typeof(SplashActivity));//MainActivity
            intent.AddFlags(ActivityFlags.SingleTop|ActivityFlags.ClearTop);

            var jsonPushNotification = JsonConvert.SerializeObject(pushNotification);
            intent.PutExtra(NotificationDataHelper.Flag, jsonPushNotification);

            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.UpdateCurrent);

            var notificationBuilder = new NotificationCompat.Builder(this)
                .SetSmallIcon(Resource.Drawable.icon)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                .SetContentText(pushNotification.Body)
                .SetContentTitle(pushNotification.Title)
                .SetNumber(pushNotification.Badge)//for Android 8.0
                .SetContentIntent(pendingIntent)
                .SetAutoCancel(true);

            ShortcutBadger.ApplyCount(this.ApplicationContext, pushNotification.Badge);

            var notificationManager = NotificationManager.FromContext(this);
            var uniqueid = (int)((DateTime.Now.Ticks / 1000L) % Int32.MaxValue);
            notificationManager.Notify(uniqueid, notificationBuilder.Build());

        }


    }
}
