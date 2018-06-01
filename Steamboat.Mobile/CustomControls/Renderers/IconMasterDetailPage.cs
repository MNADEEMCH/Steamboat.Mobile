using System;
using System.Windows.Input;
using Steamboat.Mobile.Services.Notification;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class IconMasterDetailPage:MasterDetailPage
    {
        private INotificationService _notificationService;
        private bool _notificationPresent;

        public static BindableProperty ClosedMenuIconProperty
        = BindableProperty.Create(nameof(ClosedMenuIcon), typeof(string), typeof(IconMasterDetailPage), string.Empty);


        public static BindableProperty ClosedNotificationMenuIconProperty
        = BindableProperty.Create(nameof(ClosedNotificationMenuIcon), typeof(string), typeof(IconMasterDetailPage), string.Empty);

        public static BindableProperty OpenedMenuIconProperty
        = BindableProperty.Create(nameof(OpenedMenuIcon), typeof(string), typeof(IconMasterDetailPage), string.Empty);


        public static BindableProperty OpenedNotificationMenuIconProperty
        = BindableProperty.Create(nameof(OpenedNotificationMenuIcon), typeof(string), typeof(IconMasterDetailPage), string.Empty);


        public static BindableProperty ChangeMenuIconCommandProperty
        = BindableProperty.Create(nameof(ChangeMenuIconCommand), typeof(ICommand), typeof(IconMasterDetailPage), null, BindingMode.OneWayToSource);

        public string ClosedMenuIcon
        {
            get { return (string)GetValue(ClosedMenuIconProperty); }
            set { SetValue(ClosedMenuIconProperty, value); }
        }

        public string ClosedNotificationMenuIcon
        {
            get { return (string)GetValue(ClosedNotificationMenuIconProperty); }
            set { SetValue(ClosedNotificationMenuIconProperty, value); }
        }

        public string OpenedMenuIcon
        {
            get { return (string)GetValue(OpenedMenuIconProperty); }
            set { SetValue(OpenedMenuIconProperty, value); }
        }

        public string OpenedNotificationMenuIcon
        {
            get { return (string)GetValue(OpenedNotificationMenuIconProperty); }
            set { SetValue(OpenedNotificationMenuIconProperty, value); }
        }

        public ICommand ChangeMenuIconCommand
        {
            get { return (ICommand)GetValue(ChangeMenuIconCommandProperty); }
            set { SetValue(ChangeMenuIconCommandProperty, value); }
        }

        ~ IconMasterDetailPage(){
            
            this.IsPresentedChanged -= IsPresentedChangedHandler;
        }
       
        public IconMasterDetailPage()
        {
            _notificationService = _notificationService ?? DependencyContainer.Resolve<INotificationService>();

            ChangeMenuIconCommand = new Command<bool>((notificationPresent) =>
            {
                _notificationPresent = notificationPresent;
                ChangeIcon();

            });

            this.IsPresentedChanged += IsPresentedChangedHandler;
        }

        private void IsPresentedChangedHandler(object sender, EventArgs e)
        {
            ChangeIcon();
        }

        private void ChangeIcon(){

            var icon = "";

            if(this.IsPresented)
                icon = this._notificationPresent ? OpenedNotificationMenuIcon : OpenedMenuIcon;
            else
                icon = this._notificationPresent ? ClosedNotificationMenuIcon : ClosedMenuIcon;

            this.Master.Icon = icon;
            _notificationService.SetMasterDetailMenuIcon(icon);
        }

	}
}
