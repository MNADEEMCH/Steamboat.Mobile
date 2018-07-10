using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.DeviceInfo;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Helpers.Settings;
using Steamboat.Mobile.Managers.Account;
using Steamboat.Mobile.Managers.Participant;
using Steamboat.Mobile.Models.Application;
using Steamboat.Mobile.Models.Notification;
using Steamboat.Mobile.Services.App;
using Steamboat.Mobile.Services.Dialog;
using Steamboat.Mobile.Services.Navigation;
using Steamboat.Mobile.Services.Notification;
using Xamarin.Forms;

namespace Steamboat.Mobile.Managers.Application
{
	public class ApplicationManager : ManagerBase, IApplicationManager
	{
		private IApplicationService _applicationService;
		private INotificationService _notificationService;
		private IAccountManager _accountManager;
		private IParticipantManager _participantManager;
		private ISettings _settings;

		private TimeSpan? _inactivityTimeStamp;
		private int _timeoutLimit;
		private int _stopwatchTimeoutLimit;
		private int _notificationsCount;
		private PushNotificationType _notificationTypePending;
		private static Stopwatch _stopWatch;

		public event EventHandler<PushNotificationEventParam> OnNotification;

		public int NotificationsBadge { get { return _notificationsCount; } set { _notificationsCount = value; } }

		public ApplicationManager(IApplicationService applicationService = null,
								  INotificationService notificationService = null,
								  IAccountManager accountManager = null,
								  IParticipantManager participantManager = null,
								  ISettings settings = null)

		{
			_applicationService = applicationService ?? DependencyContainer.Resolve<IApplicationService>();
			_notificationService = notificationService ?? DependencyContainer.Resolve<INotificationService>();
			_accountManager = accountManager ?? DependencyContainer.Resolve<IAccountManager>();
			_participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();
			_settings = settings ?? DependencyContainer.Resolve<ISettings>();

			_stopWatch = new Stopwatch();
			_timeoutLimit = _settings.TimeoutLimit;
			_stopwatchTimeoutLimit = _settings.TimeoutLimit;
		}

		public async Task InitializeApplication(PushNotification pushNotification = null)
		{
			if (pushNotification != null)
			{
				NotificationsBadge = pushNotification.Badge;
				UpdateNotificationBadge(0);
				_notificationTypePending = pushNotification.Type;
			}
			else
				NotificationsBadge = 0;

			await _navigationService.InitializeAsync();
		}

		public async Task HandlePushNotification(bool notificationOpenedByTouch, bool isAppBackgrounded, PushNotification pushNotification)
		{
			NotificationsBadge = pushNotification.Badge;

			var inactivityDetected = CheckForInactivity();
			var isUserLoggedIn = App.SessionID != null;

			//NAVIGATE TO
			if (notificationOpenedByTouch)
			{
				if (!inactivityDetected
					&& isUserLoggedIn)
					await HandlePushNotificationNavigatTo(pushNotification.Type);
				else
					_notificationTypePending = pushNotification.Type;
			}
			//BADGE
			if (!isUserLoggedIn || isAppBackgrounded)
				UpdateNotificationBadge(0);
			else
				UpdateNotificationBadge(NotificationsBadge);

			//ON NOTIFICATION
			if (!inactivityDetected && OnNotification != null)
			{

				var notificationEventParam = new PushNotificationEventParam()
				{
					PushNotification = pushNotification,
					IsAppBackgrounded = isAppBackgrounded,
					NotificationOpenedByTouch = notificationOpenedByTouch
				};

				OnNotification(this, notificationEventParam);
			}
		}

		public void UpdateNotificationBadge(int notificationsOpened)
		{
			int updatedBadge = GetBadgeToSet(notificationsOpened);

			_notificationService.SetNotificationBadge(updatedBadge);

			NotificationsBadge -= notificationsOpened;
		}

		private int GetBadgeToSet(int notificationsOpened)
		{
			var currentBadge = NotificationsBadge;

			var updatedBadge = 0;

			if (currentBadge >= notificationsOpened)
				updatedBadge = currentBadge - notificationsOpened;

			return updatedBadge;
		}

		private async Task HandlePushNotificationNavigatTo(PushNotificationType notificationType)
		{
			if (NotificationNavigateToHelper.NotificationNavigateToDictionary.ContainsKey(notificationType))
			{

				var viewModelType = NotificationNavigateToHelper.NotificationNavigateToDictionary[notificationType];
				if (viewModelType != null)
					await NavigateToView(viewModelType);
				else
					await NavigateToStatusView();
			}
		}

		private async Task NavigateToView(Type viewModelType)
		{
			await _navigationService.NavigateToAsync(viewModelType, null, mainPage: true);
		}

		private async Task NavigateToStatusView()
		{
			var status = await _participantManager.GetStatus();
			var viewModelType = DashboardHelper.GetViewModelForStatus(status);

			Device.BeginInvokeOnMainThread(async () =>
			{
				await _navigationService.NavigateToAsync(viewModelType, status, mainPage: true);
			});

		}

		public async Task TrySendToken()
		{
			await TryExecute(async () =>
			{
				var isUserLoggedIn = App.SessionID != null;
				var readyToSendToken = _notificationService.IsValidToken() && isUserLoggedIn;
				if (readyToSendToken)
				{
					var devicePlatform = CrossDeviceInfo.Current.Platform.ToString();
					var deviceModel = CrossDeviceInfo.Current.Model;
					var deviceLocalID = _notificationService.GetToken();

					var applicationDeviceInfo = new ApplicationDeviceInfo()
					{
						Platform = devicePlatform,
						Model = deviceModel,
						LocalID = deviceLocalID
					};

					await _applicationService.SendToken(applicationDeviceInfo, App.SessionID);
				}
			});
		}


		public async Task OnApplicationStart()
		{
			if (App.SessionID != null)
			{
				StartTimer();
			}

			Device.StartTimer(new TimeSpan(0, 0, 1), () =>
			{
				if (_stopWatch.IsRunning && _stopWatch.Elapsed.Minutes >= _stopwatchTimeoutLimit)
				{
					Task.Run(async () => await SessionExpired());
					ResetTimer();
				}

				return true;
			});
		}

		public async Task OnApplicationSleep()
		{
			StartTimingInactivity();
		}

		public async Task OnApplicationResume()
		{
			if (CheckForInactivity())
				await SessionExpired();
			else if (App.SessionID != null)
			{
				StartTimer();
				Device.BeginInvokeOnMainThread(() => UpdateNotificationBadge(NotificationsBadge));
			}

			_inactivityTimeStamp = null;
		}

		private void StartTimingInactivity()
		{
			if (App.SessionID != null)
			{
				_inactivityTimeStamp = new TimeSpan(DateTime.Now.Ticks);
				ResetTimer();
			}
		}

		private bool CheckForInactivity()
		{
			if (_inactivityTimeStamp == null || App.SessionID == null)
				return false;

			TimeSpan? currentTimeSpan = new TimeSpan(DateTime.Now.Ticks);
			var difference = currentTimeSpan - _inactivityTimeStamp;

			return difference.Value.Minutes >= _timeoutLimit;

		}

		Task IApplicationManager.SessionExpired()
		{
			return base.SessionExpired();
		}

		public Type GetPendingViewModelType()
		{
			Type viewModelType = null;
			if (NotificationNavigateToHelper.NotificationNavigateToDictionary.ContainsKey(_notificationTypePending))
				viewModelType = NotificationNavigateToHelper.NotificationNavigateToDictionary[_notificationTypePending];

			_notificationTypePending = PushNotificationType.Unknown;
			return viewModelType;
		}

		public void StartTimer()
		{
			if (!_stopWatch.IsRunning)
				_stopWatch.Start();
		}

		public void ResetTimer()
		{
			_stopWatch.Reset();
		}

		public void RestartTimer()
		{
			_stopWatch.Restart();
		}

		public void IncreaseTimer()
		{
			_stopwatchTimeoutLimit = _settings.ReportTimeoutLimit;
		}

		public void DecreaseTimer()
		{
			_stopwatchTimeoutLimit = _settings.TimeoutLimit;
		}
	}
}
