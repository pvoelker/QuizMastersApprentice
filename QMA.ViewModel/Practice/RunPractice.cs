﻿using Microsoft.Toolkit.Mvvm.ComponentModel;
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

        private ISeasonRepository _seasonRepository;
        private ITeamRepository _teamRepository;
        private IQuizzerRepository _quizzerRepository;
        private ITeamMemberRepository _teamMemberRepository;
        private IQuestionSetRepository _questionSetRepository;
        private IQuestionRepository _questionRepository;

        public RunPractice(
            IMessageBoxService messageBoxService,
            ISeasonRepository seasonRepository,
            ITeamRepository teamRepository,
            IQuizzerRepository quizzerRepository,
            ITeamMemberRepository teamMemberRepository,
            IQuestionSetRepository questionSetRepository,
            IQuestionRepository questionRepository,
            IEnumerable<string> quizzerIds,
            string seasonName,
            IEnumerable<ObservablePracticeQuestion> questions)
        {
            if (messageBoxService == null)
            {
                throw new ArgumentNullException(nameof(messageBoxService));
            }

            _messageBoxService = messageBoxService;

            SeasonName = seasonName;

            _seasonRepository = seasonRepository;
            _teamRepository = teamRepository;
            _quizzerRepository = quizzerRepository;
            _teamMemberRepository = teamMemberRepository;
            _questionSetRepository = questionSetRepository;
            _questionRepository = questionRepository;

            Initialize = new RelayCommand(() =>
            {
                foreach(var id in quizzerIds)
                {
                    var quizzer = _quizzerRepository.GetByKey(id);

                    var practiceQuizzer = new ObservablePracticeQuizzer(quizzer);

                    PracticeQuizzers.Add(practiceQuizzer);
                }

                PracticeQuestions.AddRange(questions);

                CurrentQuestion = GetNextQuestion();
            });

            NoAnswer = new RelayCommand(() =>
            {
                NoAnswerQuestions.Add(CurrentQuestion);

                CurrentQuestion = GetNextQuestion();
            });

            JustLearning = new RelayCommand(() =>
            {
                CurrentQuestion.JustLearned = true;

                JustLearningQuestions.Add(CurrentQuestion);

                CurrentQuestion = GetNextQuestion();
            });

            CorrectAnswer = new RelayCommand(() =>
            {
                SelectedQuizzer.CorrectQuestions.Add(CurrentQuestion);

                SelectedQuizzer = null;

                CurrentQuestion = GetNextQuestion();
            });

            WrongAnswer = new RelayCommand(() =>
            {
                SelectedQuizzer.WrongQuestions.Add(CurrentQuestion);

                SelectedQuizzer = null;

                CurrentQuestion = GetNextQuestion();
            });

            Closing = new RelayCommand<CancelEventArgs>((CancelEventArgs e) =>
            {
            });
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

        private ObservablePracticeQuestion GetNextQuestion()
        {
            var lowestUsage = PracticeQuestions.Min(x => x.UsageCount);

            var lowestUsageQuestions = PracticeQuestions.Where(x => x.UsageCount == lowestUsage).ToList();

            var lowestUsageQuestionsCount = lowestUsageQuestions.Count();

            var index = _random.Next(lowestUsageQuestionsCount - 1);

            var nextQuestion = lowestUsageQuestions[index];

            nextQuestion.UsageCount = nextQuestion.UsageCount + 1;

            TotalQuestionsAsked = TotalQuestionsAsked + 1;

            return nextQuestion;
        }
    }
}