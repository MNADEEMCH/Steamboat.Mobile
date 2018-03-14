using System;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.NavigationParameters;

namespace Steamboat.Mobile.Managers
{
    public interface IApplicationManager
    {
        Task InitializeApplication(PushNotificationParameter pushNotificationParameter = null);

        Task HandlePushNotification(PushNotificationParameter pushNotificationParameter);

        Task TokenRefreshed(string token, string sessionID);

    }

}
