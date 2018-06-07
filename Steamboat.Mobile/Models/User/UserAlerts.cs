using System;
using System.Collections.Generic;

namespace Steamboat.Mobile.Models.User
{
    public class UserAlerts
    {
        public string UserName{ get; set; }

        public List<int> AlertsIds { get; set; } 

        public UserAlerts()
        {
            
        }
    }
}
