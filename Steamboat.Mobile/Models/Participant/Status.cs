using System;
namespace Steamboat.Mobile.Models.Participant
{
    public class Status
    {
        public Dashboard Dashboard { get; set; }
        public string OnClick { get; set; }
    }

    public class EnrollmentStep
    {
        public string Type { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string IconClass { get; set; }
        public string Class { get; set; }
        public object Url { get; set; }
        public bool IsPopup { get; set; }
        public int Count { get; set; }
        public object OnClick { get; set; }
    }

    public class SurveyStep
    {
        public string Type { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string IconClass { get; set; }
        public string Class { get; set; }
        public string Url { get; set; }
        public bool IsPopup { get; set; }
        public int Count { get; set; }
        public string OnClick { get; set; }
    }

    public class EvaluationStep
    {
        public string Type { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string IconClass { get; set; }
        public string Class { get; set; }
        public object Url { get; set; }
        public bool IsPopup { get; set; }
        public int Count { get; set; }
        public object OnClick { get; set; }
    }

    public class ReportStep
    {
        public string Type { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string IconClass { get; set; }
        public string Class { get; set; }
        public object Url { get; set; }
        public bool IsPopup { get; set; }
        public int Count { get; set; }
        public object OnClick { get; set; }
    }

    public class NextStepContent
    {
        public string ButtonText { get; set; }
        public string Url { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
    }

    public class Dashboard
    {
        public EnrollmentStep EnrollmentStep { get; set; }
        public SurveyStep SurveyStep { get; set; }
        public EvaluationStep EvaluationStep { get; set; }
        public ReportStep ReportStep { get; set; }
        public NextStepContent NextStepContent { get; set; }
    }
}
