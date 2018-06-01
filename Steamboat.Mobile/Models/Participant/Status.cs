using System;
using System.Collections.Generic;
using Steamboat.Mobile.Models.Participant.DispositionSteps;

namespace Steamboat.Mobile.Models.Participant
{
    public class Status
    {
        public Dashboard Dashboard { get; set; }
    }

    public class Dashboard
    {
        public Alert Alert { get; set; }
        public SurveyStep SurveyStep { get; set; }
        public SchedulingStep SchedulingStep { get; set; }
        public ScreeningStep ScreeningStep { get; set; }
        public ReportStep ReportStep { get; set; }
        public int UnreadMessageCount { get; set; }
    }

    public class Alert
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string ButtonText { get; set; }
    }







}
