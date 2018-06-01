using System;
using System.Collections.Generic;
using Steamboat.Mobile.Models.Notification;
using Steamboat.Mobile.ViewModels;

namespace Steamboat.Mobile.Helpers
{
    public class NotificationNavigateToHelper
    {
        /// <summary>
        /// There are three posibilities:
        /// 1)Key='X'A notification type 'X' is not present in the Dicionary.
        ///   It means that for 'X' notification type, we dont want to navigate anywhere.
        /// 2)Key='X',Value=null
        ///   It means that for 'X' notification type, we want to navigate to the status view
        /// 3)Key='X',Value='YViewModel '
        ///   It means that for 'X' notification type, we want to navigate to Y View
        /// </summary>
        
        public static readonly Dictionary<PushNotificationType, Type> NotificationNavigateToDictionary = new Dictionary<PushNotificationType, Type>
        {
            {PushNotificationType.ReportReady, null},
            {PushNotificationType.CoachMessage, typeof(MessagingViewModel)},
        };
    }
}
