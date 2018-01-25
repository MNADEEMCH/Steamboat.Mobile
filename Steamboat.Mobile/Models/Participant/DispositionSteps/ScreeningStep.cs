using System;
namespace Steamboat.Mobile.Models.Participant.DispositionSteps
{
    public class ScreeningStep: DispositionStep
    {
        public Detail Detail { get; set; }
    }

    public class Detail
    {
        public Timeslot Timeslot { get; set; }
    }


    public class Timeslot
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }

    }
}
