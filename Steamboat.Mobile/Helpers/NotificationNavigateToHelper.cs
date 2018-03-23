using System;
using System.Collections.Generic;
using Steamboat.Mobile.ViewModels;

namespace Steamboat.Mobile.Helpers
{
    public class NotificationNavigateToHelper
    {
        public static readonly Dictionary<string, Type> NotificationNavigateToDictionary = new Dictionary<string, Type>
        {
            {"ReportReady", null},
            {"CoachMessage", null},
            {"AppointmentReminder", null}
        };
    }
}
