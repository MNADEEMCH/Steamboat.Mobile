using System;
using System.Collections.Generic;
using Android.Content;
using Steamboat.Mobile.Models.Notification;
using System.Linq;
using Firebase.Messaging;
using Newtonsoft.Json;
using Steamboat.Mobile.Helpers;

namespace Steamboat.Mobile.Droid.Helpers
{
    public class NotificationHelper
    {
        public static PushNotification TryGetPushNotification(Intent intent)
        {

            PushNotification pushNotification = null;

            if (intent != null && intent.Extras != null && intent.HasExtra(NotificationDataHelper.Flag)){

                var serializedPushNot=intent.GetStringExtra(NotificationDataHelper.Flag);
                pushNotification = JsonConvert.DeserializeObject<PushNotification>(serializedPushNot);
            }

            return pushNotification;
        }

        public static PushNotification TryGetPushNotification(RemoteMessage message)
        {

            PushNotification pushNotification = null;

            if (message.Data != null && message.Data.ContainsKey(NotificationDataHelper.Flag))
            {
                var notification = message.Data;

                var badge = 0;
                var type=PushNotificationType.Unknown;
                var title = "";
                var body = "";

                if (message.Data.ContainsKey(NotificationDataHelper.Badge))
                    Int32.TryParse(message.Data[NotificationDataHelper.Badge], out badge);

                if (message.Data.ContainsKey(NotificationDataHelper.Type))
                    Enum.TryParse<PushNotificationType>(message.Data[NotificationDataHelper.Type], out type);

                if (message.Data.ContainsKey(NotificationDataHelper.Title))
                    title = message.Data[NotificationDataHelper.Title];

                if (message.Data.ContainsKey(NotificationDataHelper.Body))
                    body = message.Data[NotificationDataHelper.Body];

                pushNotification = new PushNotification() { Title = title, Body = body, Badge = badge, Type = type};
            }

            return pushNotification;
        }

    }
}
