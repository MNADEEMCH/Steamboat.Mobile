using System;
namespace Steamboat.Mobile.Models.Participant.DispositionSteps
{
    public class ScreeningStep: DispositionStep
    {
        public ScreeningDetail Detail { get; set; }
    }

    public class ScreeningDetail
    {
        public Timeslot Timeslot { get; set; }
    }


    public class Timeslot
    {
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }
        public string ID { get; set; }
        public string EventID { get; set; }
    }
}
