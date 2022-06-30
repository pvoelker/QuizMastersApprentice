using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using QMA.DataAccess;
using QMA.Importers;
using QMA.Model;
using QMA.Model.Season;
using QMA.ViewModel.Observables;
using QMA.ViewModel.Observables.Practice;
using QMA.ViewModel.Provider;
using QMA.ViewModel.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace QMA.ViewModel.Practice
{
    public class RunPractice : ObservableObject
    {
        private Random _random = new Random();

        private IMessageBoxService _messageBoxService;

        private IAssignedQuestionRepository _assignedRepository;

        public RunPractice(
            IMessageBoxService messageBoxService,
            IQuizzerRepository quizzerRepository,
            ITeamMemberRepository teamMemberRepository,
            IAssignedQuestionRepository assignedRepository,
            IEnumerable<string> teamMemberIds,
            string seasonName,
            IEnumerable<ObservablePracticeQuestion> questions)
        {
            if (messageBoxService == null)
            {
                throw new ArgumentNullException(nameof(messageBoxService));
            }

            _messageBoxService = messageBoxService;

            SeasonName = seasonName;

            _assignedRepository = assignedRepository;

            Initialize = new RelayCommand(() =>
            {
                foreach(var id in teamMemberIds)
                {
                    var teamMember = teamMemberRepository.GetByKey(id);

                    var quizzer = quizzerRepository.GetByKey(teamMember.QuizzerId);

                    var practiceQuizzer = new ObservablePracticeQuizzer(id, quizzer);

                    var assigned = _assignedRepository.GetByTeamMemberId(id);

                    foreach(var item in assigned)
                    {
                        var question = questions.SingleOrDefault(x => x.PrimaryKey == item.QuestionId);

                        if (question != null)
                        {
                            practiceQuizzer.AssignedQuestions.Add(question);
                        }
                        else
                        {
                            throw new Exception($"Question ID '{item.QuestionId}' is missing");
                        }
                    }

                    PracticeQuizzers.Add(practiceQuizzer);
                }

                PracticeQuestions.AddRange(questions);

                CurrentQuestion = GetNextQuestion();
            });

            NoAnswer = new RelayCommand(() =>
            {
                NoAnswerQuestions.Add(CurrentQuestion);

                CheckAndAssignQuestions(CurrentQuestion);

                CurrentQuestion = GetNextQuestion();
            });

            JustLearning = new RelayCommand(() =>
            {
                CurrentQuestion.JustLearned = true;

                JustLearningQuestions.Add(CurrentQuestion);

                CheckAndAssignQuestions(CurrentQuestion);

                CurrentQuestion = GetNextQuestion();
            });

            CorrectAnswer = new RelayCommand(() =>
            {
                SelectedQuizzer.CorrectQuestions.Add(CurrentQuestion);

                CheckAndAssignQuestions(CurrentQuestion);

                SelectedQuizzer = null;

                CurrentQuestion = GetNextQuestion();
            });

            WrongAnswer = new RelayCommand(() =>
            {
                SelectedQuizzer.WrongQuestions.Add(CurrentQuestion);

                CheckAndAssignQuestions(CurrentQuestion);

                SelectedQuizzer = null;

                CurrentQuestion = GetNextQuestion();
            });

            Closing = new RelayCommand<CancelEventArgs>((CancelEventArgs e) =>
            {
                if (CurrentQuestion != null)
                {
                    if (messageBoxService.PromptToContinue("Practice is in progress, are you sure you want to cancel the practice?") == false)
                    {
                        e.Cancel = true;
                    }
                }
            });
        }

        private void CheckAndAssignQuestions(ObservablePracticeQuestion currentQuestion)
        {
            foreach (var quizzer in PracticeQuizzers)
            {
                if (quizzer.AssignQuestion)
                {
                    _assignedRepository.Add(new AssignedQuestion
                    {
                        PrimaryKey = Guid.NewGuid().ToString(),
                        TeamMemberId = quizzer.TeamMemberId,
                        QuestionId = currentQuestion.PrimaryKey
                    });

                    quizzer.AssignedQuestions.Add(currentQuestion);

                    quizzer.AssignQuestion = false;
                }
            }
        }

        private string _seasonName = null;
        public string SeasonName
        {
            get => _seasonName;
            set => SetProperty(ref _seasonName, value);
        }

        public ObservableCollection<ObservablePracticeQuizzer> PracticeQuizzers { get; } = new ObservableCollection<ObservablePracticeQuizzer>();

        private ObservablePracticeQuizzer _selectedQuizzer = null;
        public ObservablePracticeQuizzer SelectedQuizzer
        {
            get => _selectedQuizzer;
            set => SetProperty(ref _selectedQuizzer, value);
        }

        public ObservableCollection<ObservablePracticeQuestion> PracticeQuestions { get; } = new ObservableCollection<ObservablePracticeQuestion>();

        private ObservablePracticeQuestion _currentQuestion = null;
        public ObservablePracticeQuestion CurrentQuestion
        {
            get => _currentQuestion;
            set => SetProperty(ref _currentQuestion, value);
        }

        public ObservableCollection<ObservablePracticeQuestion> NoAnswerQuestions { get; } = new ObservableCollection<ObservablePracticeQuestion>();

        public ObservableCollection<ObservablePracticeQuestion> JustLearningQuestions { get; } = new ObservableCollection<ObservablePracticeQuestion>();

        private int _totalQuestionsAsked = 0;
        public int TotalQuestionsAsked
        {
            get => _totalQuestionsAsked;
            set => SetProperty(ref _totalQuestionsAsked, value);
        }

        #region Commands

        public IRelayCommand Initialize { get; }

        public IRelayCommand NoAnswer { get; }

        public IRelayCommand JustLearning { get; }

        public IRelayCommand CorrectAnswer { get; }

        public IRelayCommand WrongAnswer { get; }

        public IRelayCommand<CancelEventArgs> Closing { get; }

        #endregion

        #region Bindable events

        public event EventHandler Closed;
        private void Close()
        {
            if (Closed != null) Closed(this, EventArgs.Empty);
        }

        #endregion

        private ObservablePracticeQuestion GetNextQuestion()
        {
            var lowestUsage = PracticeQuestions.Min(x => x.UsageCount);

            var lowestUsageQuestions = PracticeQuestions.Where(x => x.UsageCount == lowestUsage).ToList();

            var lowestUsageQuestionsCount = lowestUsageQuestions.Count();

            var index = _random.Next(lowestUsageQuestionsCount - 1);

            var nextQuestion = lowestUsageQuestions[index];

            nextQuestion.UsageCount = nextQuestion.UsageCount + 1;

            TotalQuestionsAsked = TotalQuestionsAsked + 1;

            foreach(var quizzer in PracticeQuizzers)
            {
                quizzer.QuestionAlreadyAssigned = quizzer.AssignedQuestions.Any(x => x.PrimaryKey == nextQuestion.PrimaryKey);
            }

            return nextQuestion;
        }
    }
}
