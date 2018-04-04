﻿using System;
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
    public class ScreeningInterviewViewModel : ViewModelBase
    {
        #region Properties

        private bool _isBusy;
        private IParticipantManager _participantManager;
        private ObservableCollection<Question> _surveyQuestions;
        private List<Question> _localQuestions;
        private int _questionIndex;
        private List<ParticipantConsent> _answersList;
        private int _questionGroupID;
        private string _imgSource;
        private bool _freeTextSendEnabled;

        public bool IsBusy { set { SetPropertyValue(ref _isBusy, value); } get { return _isBusy; } }
        public string ImgSource { set { SetPropertyValue(ref _imgSource, value); } get { return _imgSource; } }
        public ICommand HandleSelectOneAnswerCommand { get; set; }
        public ICommand HandleFreeTextAnswerCommand { get; set; }
        public ICommand ValidateFreeTextToSendCommand { get; set; }
        public ObservableCollection<Question> SurveyQuestions { get { return _surveyQuestions; } set { SetPropertyValue(ref _surveyQuestions, value); } }
        public bool FreeTextSendEnabled { get { return _freeTextSendEnabled; } set { SetPropertyValue(ref _freeTextSendEnabled, value); } }

        #endregion

        public ScreeningInterviewViewModel(IParticipantManager participantManager = null)
        {
            IsLoading = true;
            _participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();
            HandleSelectOneAnswerCommand = new Command(async (selectedAnswer) => await HandleSelectOneAnswer(selectedAnswer));
            HandleFreeTextAnswerCommand = new Command(async (freeTexAnswer) => await HandleFreeTextAnswer(freeTexAnswer));
            ValidateFreeTextToSendCommand = new Command<string>((freeTexAnswer) =>ValidateFreeTextToSend(freeTexAnswer));
            _questionIndex = 0;
            ImgSource = App.CurrentUser.AvatarUrl;
            SurveyQuestions = new ObservableCollection<Question>();
        }

        public async override Task InitializeAsync(object parameter)
        {
            try
            {
                var questionGroups = await _participantManager.GetSurvey();
                _localQuestions = questionGroups.Questions;
                _questionGroupID = questionGroups.ID;
                _answersList = new List<ParticipantConsent>();
                IsLoading = false;

                await ContinueSurvey(true);
            }
            catch (Exception e)
            {
                await DialogService.ShowAlertAsync(e.Message, "Error", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ContinueSurvey(bool isFirstQuestion)
        {
            Question currentQuestion;
            var shouldBreak = false;
            for (int i = _questionIndex; i < _localQuestions.Count; i++)
            {
                currentQuestion =_localQuestions.ElementAt(_questionIndex);
                if (currentQuestion.IsEnabled)
                {
                    currentQuestion.IsFirstQuestion = isFirstQuestion;
                    SurveyQuestions.Add(currentQuestion);

                    await WaitAnimation();
                    if (!currentQuestion.Type.Equals(SurveyHelper.LabelType))
                    {
                        //Set answer part of the question
                        var extraQuestion = currentQuestion;
                        extraQuestion.IsAnswer = true;
                        SurveyQuestions.Add(extraQuestion);
                        shouldBreak = true;
                    }
                }

                _questionIndex++;
                isFirstQuestion = false;
                if (shouldBreak)
                    break;
            }
        }

        private async Task HandleSelectOneAnswer(object answerQuestion)
        {
            var lastQuestion = SurveyQuestions.Last();
            var answer = answerQuestion as Answers;
            lastQuestion.AnswerText = answer.Text;

            await HandleAnswer(lastQuestion, answer);
        }

        private async Task HandleFreeTextAnswer(object freeText)
        {
            if (FreeTextSendEnabled)
            {
                FreeTextSendEnabled = false;
                var lastQuestion = SurveyQuestions.Last();
                var answer = lastQuestion.Answers.First();
                lastQuestion.AnswerText = (freeText as string).Trim();

                await HandleAnswer(lastQuestion, answer);
            }
        }

        private void ValidateFreeTextToSend(string freeTextAnswer){
            FreeTextSendEnabled = freeTextAnswer.Trim().Length >= 1;
        }


        private async Task HandleAnswer(Question question, Answers answer){
            
            answer.IsSelected = true;
            question.IsComplete = true;
            _localQuestions.First(q => q.Key.Equals(question.Key)).IsComplete = true;

            SaveAnswer(question, answer);
            await AddRejoinder(answer);

            if (question.IsDependencyTarget)
            {
                var response = await _participantManager.SendSurvey(_questionGroupID, _answersList);
                _localQuestions = response.Questions;
            }
            if (IsLastQuestion())
            {
                await _participantManager.CompleteSurvey(_questionGroupID, _answersList);
                await NavigateToDashboard();
            }
            else
                await ContinueSurvey(!HasRejoinder(answer));  

        }

        private bool HasRejoinder(Answers answer)
        {
            return !string.IsNullOrEmpty(answer.AlternateText);
        }

        private bool IsLastQuestion()
        {
            return !_localQuestions.Any(q => q.IsEnabled.Equals(true) && !q.Type.Equals(SurveyHelper.LabelType) && q.IsComplete.Equals(false));
        }

        private async Task AddRejoinder(Answers answer)
        {
            if (HasRejoinder(answer))
            {
                var rejoinder = new Question();
                rejoinder.Type = SurveyHelper.LabelType;
                rejoinder.IsAnswer = false;
                rejoinder.Text = answer.AlternateText;
                rejoinder.IsFirstQuestion = true;
                SurveyQuestions.Add(rejoinder);
                await WaitAnimation();
            }
        }

        private async Task WaitAnimation()
        {
            await Task.Delay(1000);
        }

        private void SaveAnswer(Question question, Answers answer)
        {
            var response = new ParticipantConsent();
            response.QuestionKey = question.Key;
            response.AnswerKey = answer.Key;
            if(question.Type.Equals(SurveyHelper.StringType))//in order to not add overhead on the request
                response.Text = question.AnswerText;
            _answersList.Add(response);
        }

        private async Task NavigateToDashboard()
        {
            var status = await _participantManager.GetStatus();

            var viewModelType = DashboardHelper.GetViewModelForStatus(status);
            await NavigationService.NavigateToAsync(viewModelType, status, mainPage: true);
        }
    }
}
