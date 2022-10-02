using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QMA.DataAccess;
using QMA.Helpers;
using QMA.Model;
using QMA.Model.Season;
using QMA.ViewModel.Observables;
using QMA.ViewModel.Observables.Practice;
using QMA.ViewModel.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace QMA.ViewModel.Practice
{
    public enum ConfigurePracticePage
    {
        None,
        Season,
        Quizzers,
        Questions,
        End
    }

    public class ConfigurePractice : ObservableObject
    {
        private IMessageBoxService _messageBoxService;
        private IRunPracticeService _runPracticeService;

        private ISeasonRepository _seasonRepository;
        private ITeamRepository _teamRepository;
        private IQuizzerRepository _quizzerRepository;
        private ITeamMemberRepository _teamMemberRepository;
        private IQuestionSetRepository _questionSetRepository;
        private IQuestionRepository _questionRepository;

        public ConfigurePractice(
            IMessageBoxService messageBoxService,
            IRunPracticeService runPracticeService,
            ISeasonRepository seasonRepository,
            ITeamRepository teamRepository,
            IQuizzerRepository quizzerRepository,
            ITeamMemberRepository teamMemberRepository,
            IQuestionSetRepository questionSetRepository,
            IQuestionRepository questionRepository)
        {
            if (messageBoxService == null)
            {
                throw new ArgumentNullException(nameof(messageBoxService));
            }

            _messageBoxService = messageBoxService;
            _runPracticeService = runPracticeService;

            _seasonRepository = seasonRepository;
            _teamRepository = teamRepository;
            _quizzerRepository = quizzerRepository;
            _teamMemberRepository = teamMemberRepository;
            _questionSetRepository = questionSetRepository;
            _questionRepository = questionRepository;

            this.TeamQuizzers.CollectionChanged += TeamQuizzers_CollectionChanged;

            Initialize = new RelayCommand(() =>
            {
                var seasons = _seasonRepository.GetAll(false);

                Seasons.AddRange(seasons);

                SelectedSeason = null;
            });

            SelectSeason = new RelayCommand(() =>
            {
                if (SelectedSeason == null)
                {
                    throw new InvalidOperationException("Season is not selected");
                }

                SelectedQuestionSet = _questionSetRepository.GetByKey(SelectedSeason.QuestionSetId);

                var quizzers = _quizzerRepository.GetAll(false);

                var teams = _teamRepository.GetBySeasonId(SelectedSeason.PrimaryKey);

                var teamMembers = new List<TeamMember>();
                foreach (var team in teams)
                {
                    var members = _teamMemberRepository.GetByTeamId(team.PrimaryKey);

                    teamMembers.AddRange(members);
                }

                TeamQuizzers.Clear();
                foreach (var teamMember in teamMembers)
                {
                    var team = teams.Single(x => x.PrimaryKey == teamMember.TeamId);
                    var quizzer = quizzers.SingleOrDefault(x => x.PrimaryKey == teamMember.QuizzerId);

                    if (quizzer != null)
                    {
                        TeamQuizzers.Add(new ObservableTeamQuizzer(
                            teamMember.PrimaryKey,
                            quizzer.PrimaryKey,
                            team.Name,
                            team.MaxPointValue,
                            quizzer.FirstName,
                            quizzer.LastName));
                    }
                    else
                    {
                        Debug.WriteLine("Quizzer id {0} was not found or was deleted", teamMember.QuizzerId);
                    }
                }

                WizardState = ConfigurePracticePage.Quizzers;
            },
            () => SelectedSeason != null);

            SelectQuizzers = new RelayCommand(() =>
            {
                ExistingQuestionCount = _questionRepository.CountByQuestionSetId(SelectedQuestionSet.PrimaryKey, MaxQuestionPointValue, false);

                WizardState = ConfigurePracticePage.Questions;
            }, () =>
            {
                CheckIfDuplicateQuizzersSelected();

                return TeamQuizzers.Any(x => x.IsSelected) && !TeamQuizzers.Any(x => x.IsDuplicate);
            });

            SelectQuestions = new AsyncRelayCommand(async () =>
            {
                foreach (var item in ParsedImportQuestions)
                {
                    if (item.AlreadyExists == false)
                    {
                        await _questionRepository.AddAsync(new Question
                        {
                            QuestionSetId = SelectedQuestionSet.PrimaryKey,
                            Number = item.Number,
                            Text = item.Text,
                            Answer = item.Answer,
                            Points = item.Points,
                            Notes = $"Imported on {DateTimeOffset.Now}"
                        });
                    }
                }

                _runPracticeService.Start(this);
            },
            () => {
                return (UseQuestionSetOnly)
                || (UseGeneratedQuestionSet && ImportParseSuccess && !ParsedImportQuestions.Any(x => x.HasParseError));
            });

            Closing = new RelayCommand<CancelEventArgs>((CancelEventArgs e) =>
            {
            });
        }

        private void TeamQuizzers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            MaxQuestionPointValue = TeamQuizzers.Where(x => x.IsSelected).Min(x => x.TeamMaxPointValue);

            SelectQuizzers.NotifyCanExecuteChanged();
        }

        private bool _useQuestionSetOnly = true;
        public bool UseQuestionSetOnly
        {
            get => _useQuestionSetOnly;
            set
            {
                SetProperty(ref _useQuestionSetOnly, value);
                OnPropertyChanged(nameof(UseGeneratedQuestionSet));
                SelectQuestions.NotifyCanExecuteChanged();
            }
        }
        public bool UseGeneratedQuestionSet
        {
            get => !_useQuestionSetOnly;
        }

        public ObservableCollection<SeasonInfo> Seasons { get; } = new ObservableCollection<SeasonInfo>();

        private SeasonInfo _selectedSeason = null;
        public SeasonInfo SelectedSeason
        {
            get => _selectedSeason;
            set
            {
                SetProperty(ref _selectedSeason, value);
                SelectSeason.NotifyCanExecuteChanged();
            }
        }

        private int _existingQuestionCount;
        public int ExistingQuestionCount
        {
            get => _existingQuestionCount;
            set => SetProperty(ref _existingQuestionCount, value);
        }

        private int? _maxQuestionPointValue;
        public int? MaxQuestionPointValue
        {
            get => _maxQuestionPointValue;
            set => SetProperty(ref _maxQuestionPointValue, value);
        }

        private QuestionSet _selectedQuestionSet = null;
        public QuestionSet SelectedQuestionSet
        {
            get => _selectedQuestionSet;
            set => SetProperty(ref _selectedQuestionSet, value);
        }

        private List<int> _questionNumberList = null;
        public List<int> QuestionNumberList
        {
            get => _questionNumberList;
            set => SetProperty(ref _questionNumberList, value);
        }

        public DeepObservableCollection<ObservableTeamQuizzer> TeamQuizzers { get; } = new DeepObservableCollection<ObservableTeamQuizzer>(new List<string> { nameof(ObservableTeamQuizzer.IsDuplicate), nameof(ObservableTeamQuizzer.IsNotDuplicate) });

        private bool _onlyUseImportQuestionsForPractice = false;
        public bool OnlyUseImportQuestionsForPractice
        {
            get => _onlyUseImportQuestionsForPractice;
            set
            {
                SetProperty(ref _onlyUseImportQuestionsForPractice, value);
                SelectQuestions.NotifyCanExecuteChanged();
            }
        }

        public ObservableCollection<ObservableImportQuestion> ParsedImportQuestions { get; set; } = new ObservableCollection<ObservableImportQuestion>();

        private bool _importParseSuccess = false;
        public bool ImportParseSuccess
        {
            get => _importParseSuccess;
            set
            {
                SetProperty(ref _importParseSuccess, value);
                OnPropertyChanged(nameof(ImportParseFailed));
                SelectQuestions.NotifyCanExecuteChanged();
            }
        }
        public bool ImportParseFailed
        {
            get => !_importParseSuccess;
        }

        public IEnumerable<ObservablePracticeQuestion> PracticeQuestions()
        {
            var retVal = new List<ObservablePracticeQuestion>();

            if (UseQuestionSetOnly || !OnlyUseImportQuestionsForPractice)
            {
                var questions = _questionRepository.GetByQuestionSetId(SelectedQuestionSet.PrimaryKey, false);

                if(QuestionNumberList != null && QuestionNumberList.Count > 0)
                {
                    questions = questions.Where(x => QuestionNumberList.Any(y => y == x.Number));
                }

                foreach (var item in questions.Where(x => !MaxQuestionPointValue.HasValue || x.Points <= MaxQuestionPointValue))
                {
                    retVal.Add(new ObservablePracticeQuestion(item));
                }
            }
            else if (UseGeneratedQuestionSet && OnlyUseImportQuestionsForPractice)
            {
                var questionIds = ParsedImportQuestions.Select(x => x.Number);

                foreach (var questionId in questionIds)
                {
                    var question = _questionRepository.GetByQuestionNumber(SelectedQuestionSet.PrimaryKey, questionId, false);

                    retVal.Add(new ObservablePracticeQuestion(question.First()));
                }
            }
            else
            {
                throw new InvalidOperationException("Unable to generate practice questions");
            }

            return retVal;
        }

        #region Wizard state

        private ConfigurePracticePage _wizardState = ConfigurePracticePage.Season;
        public ConfigurePracticePage WizardState
        {
            get => _wizardState;
            set
            {
                SetProperty(ref _wizardState, value);
                OnPropertyChanged(nameof(OnSeasonPage));
                OnPropertyChanged(nameof(OnQuizzersPage));
                OnPropertyChanged(nameof(OnQuestionsPage));
            }
        }
        public bool OnSeasonPage
        {
            get => _wizardState == ConfigurePracticePage.Season;
        }
        public bool OnQuizzersPage
        {
            get => _wizardState == ConfigurePracticePage.Quizzers;
        }
        public bool OnQuestionsPage
        {
            get => _wizardState == ConfigurePracticePage.Questions;
        }

        #endregion

        #region Commands

        public IRelayCommand Initialize { get; }

        public IRelayCommand SelectSeason { get; }
        public IRelayCommand SelectQuizzers { get; }
        public IRelayCommand SelectQuestions { get; }

        public IRelayCommand<CancelEventArgs> Closing { get; }

        #endregion

        private bool CheckIfDuplicateQuizzersSelected()
        {
            var retVal = false;

            var groupedQuizzers = TeamQuizzers.Where(x => x.IsSelected).GroupBy(x => x.QuizzerId);

            foreach (var item in groupedQuizzers)
            {
                if (item.Count() > 1)
                {
                    foreach (var dups in item as IEnumerable<ObservableTeamQuizzer>)
                    {
                        dups.IsDuplicate = true;
                    }
                }
                else if (item.Count() == 1)
                {
                    var itemToCheck = item.First().QuizzerId;
                    foreach (var dups in TeamQuizzers.Where(x => x.QuizzerId == itemToCheck))
                    {
                        dups.IsDuplicate = false;
                    }
                }
            }

            return retVal;
        }
    }
}
