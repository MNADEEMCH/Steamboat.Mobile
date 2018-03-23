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

                var title = "";//Here I can not ready the push title
                var msg = "";//Here I can not ready the push body
                var badge = 0;
                var navigateTo = "";

                if (intent.HasExtra(NotificationDataHelper.Badge))
                    Int32.TryParse(intent.GetStringExtra(NotificationDataHelper.Badge), out badge);
                
                if(intent.HasExtra(NotificationDataHelper.NavigateTo))
                    navigateTo=intent.GetStringExtra(NotificationDataHelper.NavigateTo);

                pushNotification = CreatePushNotification(title,msg,
                                                         badge, navigateTo);
            }

            return pushNotification;
        }

        public static PushNotification TryGetPushNotification(RemoteMessage message)
        {

            PushNotification pushNotification = null;

            if (message.GetNotification()!=null && message.Data != null && message.Data.ContainsKey(NotificationDataHelper.Flag))
            {
                var notification = message.GetNotification();

                var badge = 0;
                var navigateTo = "";

                if (message.Data.ContainsKey(NotificationDataHelper.Badge))
                    Int32.TryParse(message.Data[NotificationDataHelper.Badge], out badge);

                if (message.Data.ContainsKey(NotificationDataHelper.NavigateTo))
                    navigateTo = message.Data[NotificationDataHelper.NavigateTo];


                pushNotification = CreatePushNotification(notification.Title,
                                                          notification.Body,
                                                          badge, navigateTo);
            }

            return pushNotification;
        }

        private static PushNotification CreatePushNotification(string title, string msg, int badge, string navigateTo){

            Dictionary<string, string> data = new Dictionary<string, string>();

            if(!String.IsNullOrEmpty(navigateTo)){
                data.Add(NotificationDataHelper.NavigateTo, navigateTo);
            }

            return new PushNotification() { Title = title, Message = msg, Badge = badge, Data = data };

        }
    }
}
