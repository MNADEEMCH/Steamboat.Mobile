using System;
namespace Steamboat.Mobile.Models.Notification
{
    public class PushNotificationEventParam
    {
        public PushNotification PushNotification { get; set; }
        public bool NotificationOpenedByTouch { get; set; }
        public bool IsAppBackgrounded { get; set; }
    }
}
