using System;
using System.Collections.Generic;

namespace Steamboat.Mobile.Models.Notification
{
    public class PushNotification
    {   
        public string Title{get;set;}
        public string Message { get; set; }
        public int Badge { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}
