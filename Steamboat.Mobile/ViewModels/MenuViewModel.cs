using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Managers.Application;
using Steamboat.Mobile.Managers.Participant;
using Steamboat.Mobile.Models.Application;
using Steamboat.Mobile.Models.Menu;
using Steamboat.Mobile.Models.NavigationParameters;
using Steamboat.Mobile.Models.Notification;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
	public class MenuViewModel : ViewModelBase
	{
		#region Properties

		private ObservableCollection<Models.Menu.MenuItem> _menuItems;
		private IParticipantManager _participantManager;
		private IApplicationManager _applicationManager;
		private bool _userTapped;

		public ObservableCollection<Models.Menu.MenuItem> MenuItems { get { return _menuItems; } set { SetPropertyValue(ref _menuItems, value); } }
		public ICommand ItemSelectedCommand { get; set; }
		public event EventHandler<bool> OnShowNotifications;

		#endregion

		public MenuViewModel(IParticipantManager participantManager = null, IApplicationManager applicationManager = null)
		{
			_participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();
			_applicationManager = applicationManager ?? DependencyContainer.Resolve<IApplicationManager>();

			ItemSelectedCommand = new Command<Models.Menu.MenuItem>((item) => this.ItemSelected(item));
		}

		public override Task InitializeAsync(object parameter)
		{
			MenuItems = new ObservableCollection<Models.Menu.MenuItem>();
			var initParameter = parameter as MainViewModelInitParameter;
			InitMenuItems(initParameter);
			_applicationManager.OnNotification += OnNotificationHandler;
			return Task.FromResult(true);
		}

		private void OnNotificationHandler(object sender, PushNotificationEventParam pushNotificationEventParam)
		{
			var pushNotification = pushNotificationEventParam.PushNotification;
			var pushNotificationOpenedByTouch = pushNotificationEventParam.NotificationOpenedByTouch;

			if (pushNotification.Type.Equals(PushNotificationType.CoachMessage))
			{
				if (pushNotificationOpenedByTouch)
				{
					GetMessagesMenuItem().NotificationsCount = 0;
					SelectItem(GetMessagesMenuItem(), false);
				}
				else
					GetMessagesMenuItem().NotificationsCount = pushNotification.Badge;

				HandleNotificationsCount();
			}
			else if (pushNotificationOpenedByTouch && pushNotification.Type.Equals(PushNotificationType.ReportReady))
			{
				SelectItem(GetDispositionStepMenuItem(), false);
			}
		}

		private void HandleNotificationsCount()
		{
			var anyNotification = MenuItems.Any(mi => mi.ShowNotificationBadge);

			if (OnShowNotifications != null)
				OnShowNotifications(this, anyNotification);
		}

		public void UpdateMenuItem(Type vmType)
		{
			var stepName = DashboardHelper.GetStepName(vmType);
			var item = GetDispositionStepMenuItem();
			item.Title = stepName;
		}

		private Models.Menu.MenuItem GetDispositionStepMenuItem()
		{
			return MenuItems.First();
		}

		private Models.Menu.MenuItem GetMessagesMenuItem()
		{
			return MenuItems.Where(mi => mi.Title.Equals("Messages")).First();
		}

		private void InitMenuItems(MainViewModelInitParameter initParameter)
		{
			var stepName = DashboardHelper.GetStepName(initParameter.DispositionStepType);
			var unreadMessageCount = initParameter.UnreadMessageCount;
			var isDispositionStepSelected = initParameter.NavigatingToDispositionStep;

			MenuItems.Add(new Models.Menu.MenuItem
			{
				Title = stepName,
				IsSelected = isDispositionStepSelected,
				NavigationAction = NavigateToDispositionStep
			});

			MenuItems.Add(new Models.Menu.MenuItem
			{
				Title = "Messages",
				IsSelected = !isDispositionStepSelected,
				NavigationAction = NavigateToMessaging,
				NotificationsCount = !isDispositionStepSelected ? 0 : unreadMessageCount
			});

			MenuItems.Add(new Models.Menu.MenuItem
			{
				Title = "Logout",
				ShowSeparator = false,
				NavigationAction = Logout
			});

			var selectedItem = isDispositionStepSelected ? GetDispositionStepMenuItem() : GetMessagesMenuItem();
			SelectItem(selectedItem, false);
			HandleNotificationsCount();
		}

		private void ItemSelected(Models.Menu.MenuItem item)
		{
			//Handle selected state
			if (_userTapped)
				return;
			_userTapped = true;

			SelectItem(item, true);
		}

		private void SelectItem(Models.Menu.MenuItem item, bool invokeAction)
		{
			var previousItem = MenuItems.First(x => x.IsSelected);
			previousItem.IsSelected = false;
			if (invokeAction)
				item.NavigationAction?.Invoke();
			item.IsSelected = true;
		}

		private async Task NavigateToDispositionStep()
		{
			await TryExecute(async () =>
			{
				var status = await _participantManager.GetStatus();
				Device.BeginInvokeOnMainThread(async () =>
				{
					var viewModelType = DashboardHelper.GetViewModelForStatus(status);
					await NavigationService.NavigateToAsync(viewModelType, status, mainPage: true);
				});
			}, null, () => _userTapped = false);
		}

		private async Task NavigateToMessaging()
		{
			_userTapped = false;
			GetMessagesMenuItem().NotificationsCount = 0;
			HandleNotificationsCount();
			await NavigationService.NavigateToAsync<MessagingViewModel>(mainPage: true);
		}

		private async Task Logout()
		{
			IsLoading = true;
			await TryExecute(async () =>
			{
				await DependencyContainer.RefreshDependencies();
				Device.BeginInvokeOnMainThread(async () => await NavigationService.NavigateToAsync<LoginViewModel>(new Logout()));
				_userTapped = false;
			}, null, () =>
			{
				IsLoading = false;
				_userTapped = false;
			});
		}

		public void Reset()
		{
			_applicationManager.OnNotification -= OnNotificationHandler;
		}
	}
}
