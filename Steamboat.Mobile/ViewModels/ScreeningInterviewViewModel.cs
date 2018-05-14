﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Managers.Participant;
using Steamboat.Mobile.Models.Participant.Survey;
using Steamboat.Mobile.ViewModels.Modals;
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
        private bool _enableContinue;
        private bool _userTapped;
        private readonly int _questionsToPopupEdit = 2;
        private int _questionAnsweredCount;
        private int _countNotLabelQuestions;
        private Answers _answerWithPendingRejoinder = null;

        public bool IsBusy { set { SetPropertyValue(ref _isBusy, value); } get { return _isBusy; } }
        public string ImgSource { set { SetPropertyValue(ref _imgSource, value); } get { return _imgSource; } }
        public ICommand HandleSelectOneCommand { get; set; }
        public ICommand HandleFreeTextCommand { get; set; }
        public ICommand HandleSelectManyCommand { get; set; }
        public ICommand ValidateFreeTextContinueCommand { get; set; }
        public ICommand SelectCheckboxCommand { get; set; }
        public ICommand EditQuestionCommand { get; set; }
        public ICommand ChangeProgressCommand { get; set; }
        public ObservableCollection<Question> SurveyQuestions { get { return _surveyQuestions; } set { SetPropertyValue(ref _surveyQuestions, value); } }
        public bool EnableContinue { set { SetPropertyValue(ref _enableContinue, value); } get { return _enableContinue; } }
        public ICommand ScrollToBottomCommand { get; set; }
        #endregion

        public ScreeningInterviewViewModel(IParticipantManager participantManager = null)
        {
            IsLoading = true;
            _participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();
            HandleSelectOneCommand = new Command(async (selectedAnswer) => await HandleSelectOneAnswer(selectedAnswer));
            HandleFreeTextCommand = new Command(async (freeTexAnswer) => await HandleFreeTextAnswer(freeTexAnswer));
            HandleSelectManyCommand = new Command(async (selectedOption) => await HandleSelectMany(selectedOption));
            ValidateFreeTextContinueCommand = new Command<string>(freeTextAnswer => ValidateFreeTextToSend(freeTextAnswer));
            SelectCheckboxCommand = new Command((selectedOption) => HandleSelectCheckbox(selectedOption));
            EditQuestionCommand = new Command(async (selectedQuestion) => await EditQuestion(selectedQuestion));

            _questionAnsweredCount = 0;
            _countNotLabelQuestions = 0;
            _questionIndex = 0;
            EnableContinue = false;
            ImgSource = App.CurrentUser.AvatarUrl;
            SurveyQuestions = new ObservableCollection<Question>();
        }

        public async override Task InitializeAsync(object parameter)
        {
            await TryExecute(async () =>
            {
                var questionGroups = await _participantManager.GetSurvey();
                _localQuestions = questionGroups.Questions;
                _questionGroupID = questionGroups.ID;
                _answersList = _participantManager.GetSurveyResponses();
                IsLoading = false;

                Device.BeginInvokeOnMainThread(async () => await SetInitialQuestionIndex());
            }, null, () => IsLoading = false);
        }

        private async Task SetInitialQuestionIndex()
        {
            AddFakeQuestionToSimulateDotsAnimation();

            var isAnyQuestionAnswered = _localQuestions.Any(q => !q.Type.Equals(SurveyHelper.LabelType) && q.IsComplete && q.IsEnabled);
            if (isAnyQuestionAnswered)
            {
                HandleUnansweredQuestionIndex();
                SetAnsweredQuestionsCount();
                HandleProgress(true);
                await AddContinueLabel("Welcome back!", true);
                await AddContinueLabel("We have a few more questions for you in order to get a better picture of your overall health and wellness.", false);
                await ContinueSurvey(false, true);
            }
            else
                await ContinueSurvey(true);
        }

        private void SetAnsweredQuestionsCount()
        {
            _questionAnsweredCount = _answersList.GroupBy(q => q.QuestionKey).Count();
        }

        private void HandleUnansweredQuestionIndex()
        {
            var firstUnansweredQuestion = _localQuestions.FirstOrDefault(q => !q.Type.Equals(SurveyHelper.LabelType) && !q.IsComplete && q.IsEnabled);
            var newIndex = _localQuestions.IndexOf(firstUnansweredQuestion);
            _questionIndex = newIndex;
        }

        private void HandleProgress(bool recalculateQuestionsCount)
        {
            if (recalculateQuestionsCount || _countNotLabelQuestions == 0)
            {
                _countNotLabelQuestions = _localQuestions.Where(q => !q.Type.Equals(SurveyHelper.LabelType)).Count();
            }
            var progress = (double)(_questionAnsweredCount) / ((double)_countNotLabelQuestions - 1); //-1 because the I'm finished is not a question
            ChangeProgressCommand.Execute(progress);
        }

        private async Task ContinueSurvey(bool isFirstQuestion, bool resumingSurvey = false)
        {
            Question currentQuestion;
            var shouldBreak = false;
            var replaceQuestion = true;

            replaceQuestion = !resumingSurvey && !(await HandlePendingRejoinder());

            for (int i = _questionIndex; i < _localQuestions.Count; i++)
            {
                currentQuestion = _localQuestions.ElementAt(_questionIndex);
                if (currentQuestion.IsEnabled)
                {
                    currentQuestion.IsFirstQuestion = isFirstQuestion;
                    if (replaceQuestion)
                    {
                        await ReplaceQuestion(currentQuestion);
                        replaceQuestion = false;
                    }
                    else
                        await AddQuestion(currentQuestion);

                    if (!currentQuestion.Type.Equals(SurveyHelper.LabelType))
                    {
                        //Set answer part of the question
                        var extraQuestion = SetUpExtraQuestion(currentQuestion);
                        extraQuestion.IsFirstQuestion = false;
                        EnableContinue = false;
                        await AddQuestion(extraQuestion);
                        shouldBreak = true;
                    }
                }

                _questionIndex++;
                isFirstQuestion = false;
                if (shouldBreak)
                {
                    _userTapped = false;
                    break;
                }
            }
        }

        private async Task HandleSelectOneAnswer(object answerQuestion)
        {
            await TryExecute(async () =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    _userTapped = true;
                    var lastQuestion = SurveyQuestions.Last();

                    var answer = answerQuestion as Answers;
                    lastQuestion.AnswerText = answer.Text;
                    MarkQuestionAsCompleted(lastQuestion);
                    answer.IsSelected = true;

                    SaveAnswer(lastQuestion, answer);
                    _questionAnsweredCount++;

                    var hasRejoinder = HasRejoinder(answer);

                    AddFakeQuestionToSimulateDotsAnimation();

                    if (!lastQuestion.IsDependencyTarget)
                        HandleProgress(false);

                    if (hasRejoinder)
                        _answerWithPendingRejoinder = answer;


                    await WaitAnimation();
                    await HandleAnswer(lastQuestion, hasRejoinder);
                });
            });
        }

        private async Task HandleFreeTextAnswer(object freeText)
        {
            await TryExecute(async () =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (EnableContinue)
                    {
                        _userTapped = true;
                        EnableContinue = false;

                        var lastQuestion = SurveyQuestions.Last();

                        var answer = lastQuestion.Answers.First();
                        lastQuestion.AnswerText = (freeText as string).Trim();
                        MarkQuestionAsCompleted(lastQuestion);
                        answer.IsSelected = true;

                        SaveAnswer(lastQuestion, answer);
                        _questionAnsweredCount++;

                        AddFakeQuestionToSimulateDotsAnimation();

                        if (!lastQuestion.IsDependencyTarget)
                            HandleProgress(false);

                        await WaitAnimation();
                        await HandleAnswer(lastQuestion, false);
                    }
                });
            });
        }

        private async Task HandleSelectMany(object selectedQuestion)
        {
            await TryExecute(async () =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    _userTapped = true;
                    var question = selectedQuestion as Question;

                    var answers = question.Answers;

                    var selectedAnswers = answers.Where(x => x.IsSelected);
                    var addSeparator = selectedAnswers.Count() > 1;
                    var lastItemKey = selectedAnswers.Last().Key;
                    MarkQuestionAsCompleted(question);

                    foreach (var item in selectedAnswers)
                    {
                        SaveAnswer(question, item);
                        question.AnswerText += item.Text;
                        if (addSeparator && item.Key != lastItemKey)
                        {
                            question.AnswerText += ", ";
                        }
                    }
                    _questionAnsweredCount++;

                    AddFakeQuestionToSimulateDotsAnimation();

                    if (!question.IsDependencyTarget)
                        HandleProgress(false);

                    await HandleAnswer(question, false);
                });
            });
        }

        private async Task HandleAnswer(Question question, bool hasRejoinder)
        {
            await TryExecute(async () =>
            {
                if (question.IsDependencyTarget)
                {
                    var response = await _participantManager.SendSurvey(_questionGroupID, _answersList);
                    _localQuestions = response.Questions;
                    HandleProgress(true);
                }

                if (IsLastQuestion())
                {
                    await _participantManager.CompleteSurvey(_questionGroupID, _answersList);
                    await NavigateToDashboard();
                }
                else
                    Device.BeginInvokeOnMainThread(async () => await ContinueSurvey(!hasRejoinder));
            });
        }

        private void HandleSelectCheckbox(object selectedOption)
        {
            var answer = selectedOption as Answers;
            var allAnswers = SurveyQuestions.Last().Answers;

            EnableContinue = allAnswers.Any(a => a.IsSelected);

            if (answer.IsExclusive)
            {
                var selectedAnsers = allAnswers.Where(x => x.IsSelected && x.Key != answer.Key);
                foreach (var item in selectedAnsers)
                {
                    item.IsSelected = false;
                }
            }
            else
            {
                var exclusiveAnswer = allAnswers.FirstOrDefault(x => x.IsSelected && x.IsExclusive);
                if (exclusiveAnswer != null)
                    exclusiveAnswer.IsSelected = false;
            }
        }

        private async Task EditQuestion(object selectedQuestion)
        {
            if (_userTapped)
                return;

            _userTapped = true;

            var question = selectedQuestion as Question;

            if (question.IsComplete)
            {
                if (ShouldShowPopup(question))
                {
                    Func<object, Task> confirmEditQuestionFunction = ConfirmEditQuestion;
                    await ModalService.PushAsync<InterviewEditQuestionModalViewModel>(async (obj) => await confirmEditQuestionFunction(selectedQuestion));
                }
                else
                    await ConfirmEditQuestion(selectedQuestion);
            }

            _userTapped = false;
        }

        private async Task ConfirmEditQuestion(object selectedQuestion)
        {
            var question = selectedQuestion as Question;

            if (question.IsComplete)
            {
                foreach (var answer in question.Answers)
                {
                    answer.IsSelected = false;
                }
                question.AnswerText = string.Empty;

                var questionIndex = SurveyQuestions.IndexOf(question);
                for (int i = SurveyQuestions.Count - 1; i > questionIndex + 1; i--)
                {
                    SurveyQuestions.RemoveAt(i);
                }

                var answerIndex = _answersList.IndexOf(_answersList.FirstOrDefault(a => a.QuestionKey == question.Key));
                var removeLimit = _answersList.Count() - answerIndex;
                _answersList.RemoveRange(answerIndex, removeLimit);

                var localQuestion = _localQuestions.FirstOrDefault(q => q.Key == question.Key);
                var newIndex = _localQuestions.IndexOf(localQuestion);
                _questionIndex = newIndex + 1;

                var completedQuestions = _localQuestions.Where(q => (q.IsComplete || q.IsAnswer) && _localQuestions.IndexOf(q) >= newIndex);
                foreach (var item in completedQuestions)
                {
                    item.IsComplete = false;
                    item.AnswerText = string.Empty;
                    item.IsAnswer = false;
                }
                question.IsComplete = false;

                SetAnsweredQuestionsCount();
                HandleProgress(true);
                ScrollToBottomCommand.Execute(null);
            }
            await Task.FromResult(true);
        }

        private Question SetUpExtraQuestion(Question currentQuestion)
        {
            var extraQuestion = currentQuestion;
            extraQuestion.IsAnswer = true;
            foreach (var item in extraQuestion.Answers)
            {
                item.IsSelected = false;
                item.Text = Regex.Replace(item.Text, @"<[^>]*>", String.Empty);
            }
            extraQuestion.AnswerText = string.Empty;

            return extraQuestion;
        }

        private void ValidateFreeTextToSend(string freeTextAnswer)
        {
            EnableContinue = freeTextAnswer.Trim().Length >= 1;
        }

        private bool HasRejoinder(Answers answer)
        {
            return !string.IsNullOrEmpty(answer.AlternateText);
        }

        private bool IsLastQuestion()
        {
            return !_localQuestions.Any(q => q.IsEnabled && !q.Type.Equals(SurveyHelper.LabelType) && !q.IsComplete);
        }


        private async Task<bool> HandlePendingRejoinder()
        {
            var pendingRejoinderHandled = false;
            if (_answerWithPendingRejoinder != null)
            {
                var fakeQuestion = SurveyQuestions.Last();
                fakeQuestion.Text = _answerWithPendingRejoinder.AlternateText;
                fakeQuestion.IsFirstQuestion = true;
                fakeQuestion.ShowQuestionDots = false;
                _answerWithPendingRejoinder = null;
                Device.BeginInvokeOnMainThread(() => ScrollToBottomCommand.Execute(null));
                await WaitAnimation();
                pendingRejoinderHandled = true;
            }

            return pendingRejoinderHandled;
        }


        private async Task AddContinueLabel(string text, bool firstQuestion)
        {
            await WaitAnimation();
            Question labelQuestion = firstQuestion ? SurveyQuestions.Last() : new Question();

            labelQuestion.Type = SurveyHelper.LabelType;
            labelQuestion.IsAnswer = false;
            labelQuestion.Text = text;
            labelQuestion.IsFirstQuestion = firstQuestion;
            labelQuestion.ShowQuestionDots = false;

            if (!firstQuestion)
                await AddQuestion(labelQuestion);
        }

        private void AddFakeQuestionToSimulateDotsAnimation()
        {
            var fakeQuestion = new Question();
            fakeQuestion.Type = SurveyHelper.LabelType;
            fakeQuestion.IsAnswer = false;
            fakeQuestion.IsFirstQuestion = true;
            fakeQuestion.ShowQuestionDots = true;
            SurveyQuestions.Add(fakeQuestion);
        }

        private async Task AddQuestion(Question question)
        {
            question.ShowQuestionDots = false;
            SurveyQuestions.Add(question);

            Device.BeginInvokeOnMainThread(() => ScrollToBottomCommand.Execute(null));
            await WaitAnimation();
        }
        private async Task ReplaceQuestion(Question question)
        {
            var lastFakeQuestion = SurveyQuestions.Last();
            lastFakeQuestion.Text = question.Text;
            lastFakeQuestion.IsFirstQuestion = question.IsFirstQuestion;
            lastFakeQuestion.ShowQuestionDots = true;

            await WaitAnimation(2000);
            lastFakeQuestion.ShowQuestionDots = false;

            Device.BeginInvokeOnMainThread(() => ScrollToBottomCommand.Execute(null));
            await WaitAnimation();
        }

        private async Task WaitAnimation(int duration = 1000)
        {
            await Task.Delay(duration);
        }

        private void SaveAnswer(Question question, Answers answer)
        {
            var response = new ParticipantConsent();
            response.QuestionKey = question.Key;
            response.AnswerKey = answer.Key;
            if (question.Type.Equals(SurveyHelper.StringType))
                response.Text = question.AnswerText;

            _answersList.Add(response);
        }

        private async Task NavigateToDashboard()
        {
            var status = await _participantManager.GetStatus();

            var viewModelType = DashboardHelper.GetViewModelForStatus(status);
            await NavigationService.NavigateToAsync(viewModelType, status, mainPage: true);
            DependencyContainer.Resolve<MenuViewModel>().UpdateMenuItem(viewModelType);
        }

        private void MarkQuestionAsCompleted(Question question)
        {
            question.IsComplete = true;
            _localQuestions.First(q => q.Key.Equals(question.Key)).IsComplete = true;
        }

        private bool ShouldShowPopup(Question question)
        {
            var questionsList = _localQuestions.Where(q => !q.Type.Equals(SurveyHelper.LabelType)).ToList();
            var editedQuestion = questionsList.First(q => q.Key.Equals(question.Key));
            var editedQuestionIndex = questionsList.IndexOf(editedQuestion);
            var lastAnsweredQuestion = questionsList.LastOrDefault(q => !q.Type.Equals(SurveyHelper.LabelType) && q.IsComplete && q.IsEnabled);
            var currenQuestionIndex = questionsList.IndexOf(lastAnsweredQuestion) + 1;
            var diff = currenQuestionIndex - editedQuestionIndex;
            return diff > _questionsToPopupEdit;
        }
    }
}

