using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Managers.Participant;
using Steamboat.Mobile.Models.Menu;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        #region Properties

        private ObservableCollection<Models.Menu.MenuItem> _menuItems;
        private IParticipantManager _participantManager;
        private bool _userTapped;

        public ObservableCollection<Models.Menu.MenuItem> MenuItems { get { return _menuItems; } set { SetPropertyValue(ref _menuItems, value); } }
        public ICommand ItemSelectedCommand { get; set; }

        #endregion

        public MenuViewModel(IParticipantManager participantManager = null)
        {
            _participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();
            MenuItems = new ObservableCollection<Models.Menu.MenuItem>();

            ItemSelectedCommand = new Command<Models.Menu.MenuItem>((item) => this.ItemSelected(item));
        }

        public override Task InitializeAsync(object parameter)
        {
            var vmType = parameter as Type;
            var stepName = DashboardHelper.GetStepName(vmType);
            InitMenuItems(stepName);
            return Task.FromResult(true);
        }

        public void UpdateMenuItem(Type vmType)
        {
            var stepName = DashboardHelper.GetStepName(vmType);
            var item = MenuItems.First();
            item.Title = stepName;
        }

        private void InitMenuItems(string stepName)
        {
            MenuItems.Add(new Models.Menu.MenuItem
            {
                Title = stepName,
                IsSelected = true,
                NavigationAction = NavigateToDispositionStep
            });
            MenuItems.Add(new Models.Menu.MenuItem
            {
                Title = "Support",
                IsSelected = false,
                NavigationAction = NavigateToMessaging
            });
            MenuItems.Add(new Models.Menu.MenuItem
            {
                Title = "Logout",
                ShowSeparator = false,
                NavigationAction = Logout
            });
        }

        private void ItemSelected(Models.Menu.MenuItem item)
        {
            //Handle selected state
            if (_userTapped)
                return;
            _userTapped = true;

            var previousItem = MenuItems.First(x => x.IsSelected);
            previousItem.IsSelected = false;
            item.NavigationAction?.Invoke();
            item.IsSelected = true;
        }

        private async Task NavigateToDispositionStep()
        {
            var status = await _participantManager.GetStatus();

            var viewModelType = DashboardHelper.GetViewModelForStatus(status);
            await NavigationService.NavigateToAsync(viewModelType, status, mainPage: true);
            _userTapped = false;
        }

        private async Task NavigateToMessaging()
        {
            _userTapped = false;
            await NavigationService.NavigateToAsync<MessagingViewModel>(mainPage:true);
        }

        private async Task Logout()
        {
            IsLoading = true;

            try
            {
                await NavigationService.NavigateToAsync<LoginViewModel>("Logout");
                _userTapped = false;
            }
            catch (Exception e)
            {
                await DialogService.ShowAlertAsync(e.Message, "Error", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
