using System;
namespace Steamboat.Mobile.Models.Participant.DispositionSteps
{
    public enum StatusEnum { None, Complete, Pending, Disabled }

    public abstract class DispositionStep
    {
        public string Type { get; set; }
        public StatusEnum Status { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string InformationTitle { get; set; }
        public string InformationMessage { get; set; }
        public string ButtonText { get; set; }
    }
}
