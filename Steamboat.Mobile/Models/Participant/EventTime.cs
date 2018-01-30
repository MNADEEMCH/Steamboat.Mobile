using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Steamboat.Mobile.Models.Participant
{
    //public class EventReponse
    //{
    //    public List<EventTime> EventTimeslots { get; set; }
    //}

    public class EventTime : INotifyPropertyChanged
    {
        private bool _isActive;

        public int ID { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsActive { set { if (_isActive != value) { _isActive = value; RaisePropertyChanged(); } } get { return _isActive; } }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
