using System;
namespace Steamboat.Mobile.Models.Participant
{
    public enum StatusEnum { None, Complete, Pending, Disabled }
    public class Status
    {
        public Dashboard Dashboard { get; set; }
    }
    public class SurveyStep
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public StatusEnum Status { get; set; }
        public string Message { get; set; }
    }

    public class SchedulingStep
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public StatusEnum Status { get; set; }
        public string Message { get; set; }
    }

    public class ScreeningStep
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public StatusEnum Status { get; set; }
        public string Message { get; set; }
    }

    public class ReportStep
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public StatusEnum Status { get; set; }
        public string Message { get; set; }
    }

    public class Dashboard
    {
        public SurveyStep SurveyStep { get; set; }
        public SchedulingStep SchedulingStep { get; set; }
        public ScreeningStep ScreeningStep { get; set; }
        public ReportStep ReportStep { get; set; }
    }

}
