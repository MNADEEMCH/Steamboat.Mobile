using System;
namespace Steamboat.Mobile.Models.Participant.DispositionSteps
{
    public class SchedulingStep: DispositionStep
    {
        public SchedulingDetail Detail { get; set; }

    }
    public class SchedulingDetail
    {
        public int EventCount { get; set; }
    }
}
