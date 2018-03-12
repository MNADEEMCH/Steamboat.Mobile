using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Managers.Participant;
using Steamboat.Mobile.Models.Participant.Survey;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class PreScreeningInterviewViewModel : ViewModelBase
    {
        #region Properties

        private bool _isBusy;
        private IParticipantManager _participantManager;
        private ObservableCollection<Question> _surveyQuestions;
        private List<Question> _localQuestions;
        private int _questionIndex;

        public bool IsBusy { set { SetPropertyValue(ref _isBusy, value); } get { return _isBusy; } }
        public ICommand SelectAnswerCommand { get; set; }
        public ObservableCollection<Question> SurveyQuestions { get { return _surveyQuestions; } set { SetPropertyValue(ref _surveyQuestions, value); } }

        #endregion

        public PreScreeningInterviewViewModel(IParticipantManager participantManager = null)
        {
            IsLoading = true;
            _participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();
            SelectAnswerCommand = new Command(async (selectedAnswer)=> await AnswerSelected(selectedAnswer));

            _questionIndex = 0;
            SurveyQuestions = new ObservableCollection<Question>();
        }

        public async override Task InitializeAsync(object parameter)
        {
            _localQuestions = await _participantManager.GetSurvey();
            IsLoading = false;

            await ContinueSurvey();
        }

        private async Task ContinueSurvey()
        {
            Question currentQuestion;
            var shouldBreak = false;
            for (int i = _questionIndex; i < _localQuestions.Count; i++)
            {
                currentQuestion = _localQuestions.ElementAt(_questionIndex);
                if (currentQuestion.IsEnabled)
                {
                    SurveyQuestions.Add(currentQuestion);
                    await Task.Delay(1000);
                    if (!currentQuestion.Type.Equals(SurveyHelper.LabelType))
                    {
                        var extraQuestion = currentQuestion;
                        extraQuestion.IsAnswer = true;
                        SurveyQuestions.Add(extraQuestion);
                        shouldBreak = true;
                    }
                }

                _questionIndex++;
                if (shouldBreak)
                    break;
            }
        }

        private async Task AnswerSelected(object selectedAnswer)
        {            
            var answer = selectedAnswer as Answers;
            var lastQuestion = SurveyQuestions.Last();
            lastQuestion.IsComplete = true;
            await AddRejoinder(answer);
            await ContinueSurvey();
        }

        private async Task AddRejoinder(Answers answer)
        {
            var rejoinder = new Question();
            rejoinder.Type = SurveyHelper.LabelType;
            rejoinder.IsAnswer = false;
            rejoinder.Text = answer.AlternateText;
            SurveyQuestions.Add(rejoinder);
            await Task.Delay(1000);
        }
    }
}
