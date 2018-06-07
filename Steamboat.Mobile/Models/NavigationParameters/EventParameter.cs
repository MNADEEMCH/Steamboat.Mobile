using System;
using Steamboat.Mobile.Models.Participant;

namespace Steamboat.Mobile.Models.NavigationParameters
{
    public class EventParameter
    {
        public EventDate EventDate { get; set; }
        public EventTime EventTime { get; set; }
        public RescheduleEvent RescheduleEvent { get; set; }
    }
}
