using System;
using System.Collections.Generic;

namespace Steamboat.Mobile.Models.Participant.Survey
{
    public class SurveyResponse
    {
        public List<ParticipantConsent> Responses { get; set; }
    }

    public class ParticipantConsent
    {
        public int QuestionKey { get; set; }
        public int AnswerKey { get; set; }
        public string Text { get; set; }
    }
}
