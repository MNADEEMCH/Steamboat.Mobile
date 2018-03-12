using System;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Models.Participant.Survey;
using Steamboat.Mobile.Views.Survey;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls.Survey
{
    public class SurveyTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate _labelDataTemplate;
        private readonly DataTemplate _selectoneDataTemplate;

        public SurveyTemplateSelector()
        {
            _labelDataTemplate = new DataTemplate(typeof(SurveyLabelView));
            _selectoneDataTemplate = new DataTemplate(typeof(SurveySelectOneView));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var question = item as Question;
            if (question == null)
                return null;

            if (IsLabelOrPartOfQuestion(question))
                return _labelDataTemplate;
            else
            {
                _selectoneDataTemplate.SetValue(SurveySelectOneView.ParentBindingContextProperty, container.BindingContext);
                return _selectoneDataTemplate;
            }
        }

        private static bool IsLabelOrPartOfQuestion(Question question)
        {
            return question.Type.Equals(SurveyHelper.LabelType) || !question.Type.Equals(SurveyHelper.LabelType) && !question.IsAnswer;
        }
    }
}
