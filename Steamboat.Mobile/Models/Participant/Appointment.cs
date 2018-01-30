using System;
namespace Steamboat.Mobile.Models.Participant
{
    public class Appointment
    {
        public int EventID { get; set; }
        public int EventTimeslotID { get; set; }
        public string Location { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }

        public Appointment()
        {
            
        }
    }
}
