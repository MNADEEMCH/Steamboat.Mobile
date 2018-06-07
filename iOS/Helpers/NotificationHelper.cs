using System;
using Foundation;
using Steamboat.Mobile.Models.Notification;
using System.Linq;
using System.Collections.Generic;
using Steamboat.Mobile.Helpers;

namespace Steamboat.Mobile.iOS.Helpers
{
    public class NotificationHelper
    {
        public static PushNotification TryGetPushNotificationWhenIsClosed(NSDictionary options)
        {
            PushNotification pushNotification = null;
            if (options != null && options.Keys != null && options.Keys.Count() != 0 && options.ContainsKey(new NSString("UIApplicationLaunchOptionsRemoteNotificationKey")))
            {
                NSDictionary UIApplicationLaunchOptionsRemoteNotificationKey = options.ObjectForKey(new NSString("UIApplicationLaunchOptionsRemoteNotificationKey")) as NSDictionary;

                pushNotification = TryGetPushNotification(UIApplicationLaunchOptionsRemoteNotificationKey);
            }

            return pushNotification;
        }
        /// <summary>
        /// An NSDictionary option example
        /*{  
              "aps":{  
                 "alert":{  
                    "body":"PRUEBA",
                    "title":"PRUEBA"
                 }
                 “badge”:1,
                 “sound”:”default”
              },
              "gcm.message_id":"0:1521053339960041%487bf7cc487bf7cc",
              "gcm.n.e":"1",
              "google.c.a.c_id":"7104613562731940122",
              "google.c.a.e":"1",
              "google.c.a.ts":"1521053339",
              "google.c.a.udt":"0",
              "momentum_health”:”true”
            }*/
        /// </summary>
        /// <returns>The get push notification.</returns>
        /// <param name="options">Options.</param>
        public static PushNotification TryGetPushNotification(NSDictionary options)
        {
            PushNotification pushNotification = null;
            if (options.ObjectForKey(new NSString(NotificationDataHelper.Flag)) != null)
            {

                var title="";
                var body="";
                var badge=0;
                var type = PushNotificationType.Unknown;
                var isContentAvailablePresent = false;

                var aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;
                if(aps!=null){

                    var contentAvailable=aps.ObjectForKey(new NSString("content-available"));
                    isContentAvailablePresent = contentAvailable != null;

                    var alert = aps.ObjectForKey(new NSString("alert")) as NSDictionary;
                    if (alert != null)
                    {
                        title = alert.ObjectForKey(new NSString("title")).ToString();
                        body = alert.ObjectForKey(new NSString("body")).ToString();
                    }

                    if (aps.ObjectForKey(new NSString(NotificationDataHelper.Badge)) != null) 
                        Int32.TryParse(aps.ObjectForKey(new NSString(NotificationDataHelper.Badge)).ToString(),out badge);

                }

                if (options.ObjectForKey(new NSString(NotificationDataHelper.Type)) != null)
                    Enum.TryParse<PushNotificationType>(options.ObjectForKey(new NSString(NotificationDataHelper.Type)).ToString(), out type);

                pushNotification = new PushNotification()
                {
                    Title = title,
                    Body = body,
                    Badge = badge,
                    Type = type,
                    IsContentAvailablePresent = isContentAvailablePresent
                };
            }

            return pushNotification;
        }

    }
}
