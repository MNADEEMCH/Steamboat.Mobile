using System;
using System.Threading.Tasks;

namespace Steamboat.Mobile.Models.Menu
{
    public class MenuItem : BindableBase
    {
        private string _title;
        private bool _isSelected;
        private bool _showSeparator;
        private bool _showNotificationBadge;
        private int _notificationsCount;

        public string Title { get { return _title; } set { _title = value; RaisePropertyChanged(); } }
        public bool IsSelected { get { return _isSelected; } set { _isSelected = value; RaisePropertyChanged(); } }
        public bool ShowSeparator { get { return _showSeparator; } set { _showSeparator = value; RaisePropertyChanged(); } }
        public Func<Task> NavigationAction { get; set; }
        public bool ShowNotificationBadge { get { return _showNotificationBadge; } private set { _showNotificationBadge = value; RaisePropertyChanged(); } }
        public int NotificationsCount { get { return _notificationsCount; } set { _notificationsCount = value; ShowNotificationBadge = _notificationsCount>0; RaisePropertyChanged(); } }

        public MenuItem()
        {
            ShowSeparator = true;
        }
    }
}


