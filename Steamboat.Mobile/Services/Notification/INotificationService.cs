using System;
namespace Steamboat.Mobile.Services.Notification
{
    public interface INotificationService
    {
        string GetToken();

        bool IsValidToken();

        void SetNotificationBadge(int badge=0);
    }
}
