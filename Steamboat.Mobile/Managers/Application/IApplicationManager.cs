using System;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.Notification;

namespace Steamboat.Mobile.Managers.Application
{
    public interface IApplicationManager
    {
        Task InitializeApplication(PushNotification pushNotification = null);

        Task HandlePushNotification(PushNotification pushNotification);

        void UpdateNotificationBadge(int notificationsOpened);

        Task TrySendToken();

    }
}
