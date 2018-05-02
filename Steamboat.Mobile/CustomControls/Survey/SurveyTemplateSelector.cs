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
        private readonly DataTemplate _selectmanyDataTemplate;
        private readonly DataTemplate _freeTextDataTemplate;

        public SurveyTemplateSelector()
        {
            _labelDataTemplate = new DataTemplate(typeof(SurveyLabelView));
            _selectoneDataTemplate = new DataTemplate(typeof(SurveySelectOneView));
            _selectmanyDataTemplate = new DataTemplate(typeof(SurveySelectManyView));
            _freeTextDataTemplate = new DataTemplate(typeof(SurveyFreeTextView));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var question = item as Question;
            if (question == null)
                return null;

            if (IsLabelOrPartOfQuestion(question)){
                _labelDataTemplate.SetValue(SurveyLabelView.ParentBindingContextProperty, container.BindingContext);
                return _labelDataTemplate;
            }
            else if (question.Type.Equals(SurveyHelper.SelectManyType))
            {
                _selectmanyDataTemplate.SetValue(SurveySelectManyView.ParentBindingContextProperty, container.BindingContext);
                return _selectmanyDataTemplate;
            }
            else if(question.Type.Equals(SurveyHelper.StringType))
            {
                _freeTextDataTemplate.SetValue(SurveyFreeTextView.ParentBindingContextProperty, container.BindingContext);
                return _freeTextDataTemplate;
            }
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
