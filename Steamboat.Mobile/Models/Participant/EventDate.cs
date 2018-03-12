using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Steamboat.Mobile.Models.Participant
{
    public class EventDate : INotifyPropertyChanged
    {
        private bool _isActive;

        public int Id { get; set; }
        public string FullAddress { get; set; }
        public string Distance { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public bool IsActive { set { if (_isActive != value) { _isActive = value; RaisePropertyChanged(); } } get { return _isActive; } }

        //TODO: Use BindableBase
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
