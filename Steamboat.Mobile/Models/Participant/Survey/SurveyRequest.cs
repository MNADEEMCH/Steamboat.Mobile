using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Steamboat.Mobile.Models.Participant.Survey
{
    public class SurveyRequest
    {
        public QuestionGroup QuestionGroup { get; set; }
    }

    public class Answers : BindableBase
    {
        public int Key { get; set; }
        public string Text { get; set; }
        public string AlternateText { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsExclusive { get; set; }
        public bool IsSelected { get; set; }
    }

    public class Question : BindableBase
    {
        public int Key { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsComplete { set; get; }
        public bool IsAnswer { get; set; }
        public bool IsFirstQuestion { get; set; }
        public bool IsDependencyTarget { get; set; }
        public List<Answers> Answers { get; set; }
    }

    public class QuestionGroup
    {
        public string Text { get; set; }
        public int ID { get; set; }
        public List<Question> Questions { get; set; }
    }
}
