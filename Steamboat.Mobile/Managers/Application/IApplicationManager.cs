using System;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.Notification;

namespace Steamboat.Mobile.Managers.Application
{
	public interface IApplicationManager
	{
		Task InitializeApplication(PushNotification pushNotification = null);

		Task HandlePushNotification(bool notificationOpenedByTouch, bool isAppBackgrounded, PushNotification pushNotification);

		void UpdateNotificationBadge(int notificationsOpened);

		Task TrySendToken();

		Task SessionExpired();

		Task OnApplicationStart();

		Task OnApplicationResume();

		Task OnApplicationSleep();

		Type GetPendingViewModelType();

		void StartTimer();

		void ResetTimer();

		void RestartTimer();

		void IncreaseTimer();

		void DecreaseTimer();

		event EventHandler<PushNotificationEventParam> OnNotification;
	}
}
