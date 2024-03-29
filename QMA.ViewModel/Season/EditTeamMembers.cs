﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QMA.DataAccess;
using QMA.Helpers;
using QMA.Model.Season;
using QMA.ViewModel.Observables;
using QMA.ViewModel.Observables.Season;
using QMA.ViewModel.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QMA.ViewModel.Season
{
    public class EditTeamMembers : ObservableObject
    {
        private IMessageBoxService _messageBoxService;

        private ITeamMemberRepository _teamMemberRepository;
        private IAssignedQuestionRepository _assignedRepository;

        private string _teamId;
        public EditTeamMembers(
            IMessageBoxService messageBoxService,
            ITeamMemberRepository repository,
            IAssignedQuestionRepository assignedRepository,
            IQuizzerRepository quizzerRepository,
            IQuestionRepository questionRepository,
            string teamId,
            string questionsSetId)
        {
            _messageBoxService = messageBoxService;

            _teamMemberRepository = repository;

            _assignedRepository = assignedRepository;

            _teamId = teamId;

            Initialize = new RelayCommand(() =>
            {
                var quizzers = quizzerRepository.GetAll(true);
                var teamMembers = _teamMemberRepository.GetByTeamId(_teamId);

                var questions = questionRepository.GetByQuestionSetId(questionsSetId, false);

                // TODO: What to do with deleted quizzers?
                foreach (var quizzer in quizzers)
                {
                    var teamMember = teamMembers.SingleOrDefault(x => x.QuizzerId == quizzer.PrimaryKey);
                    var teamMemberId = teamMember == null ? null : teamMember.PrimaryKey;

                    var newItem = new ObservableTeamMember(
                        true,
                        teamMemberId,
                        quizzer.PrimaryKey,
                        $"{quizzer.LastName}, {quizzer.FirstName}"
                    );

                    if(teamMemberId != null)
                    {
                        newItem.IsMember = true;
                        //newItem.Persisted = true;
                    }

                    var assignedQuestions = _assignedRepository.GetByTeamMemberId(teamMemberId);
                    foreach (var assignedQuestion in assignedQuestions)
                    {
                        var foundQuestion = questions.SingleOrDefault(x => x.PrimaryKey == assignedQuestion.QuestionId);

                        if (foundQuestion != null)
                        {
                            newItem.AssignedQuestions.Add(new ObservableAssignedQuestion(true, foundQuestion));
                        }
                    }

                    TeamMembers.Add(newItem);
                }

                foreach(var item in teamMembers)
                {
                    var found = TeamMembers.SingleOrDefault(x => x.QuizzerId == item.QuizzerId);
                    found.IsMember = true;
                    found.Persisted = true;
                }

                foreach(var item in questions)
                {
                    Questions.Add(new ObservableAssignedQuestion(false, item));
                }
            });

            Save = new AsyncRelayCommand<CancelEventArgs>(async (CancelEventArgs e) =>
            {
                var existingQuizzers = _teamMemberRepository.GetByTeamId(_teamId);
                var existingQuizzerIds = existingQuizzers.Select(x => x.QuizzerId);
                var newQuizzerIds = TeamMembers.Where(x => x.IsMember == true).Select(x => x.QuizzerId);

                var quizzerIdsToDelete = existingQuizzerIds.Except(newQuizzerIds);
                var quizzerIdsToAdd = newQuizzerIds.Except(existingQuizzerIds);
                var quizzerIdsToUpdate = existingQuizzerIds.Union(newQuizzerIds);

                foreach (var quizzerId in quizzerIdsToDelete)
                {
                    var id = existingQuizzers.Single(x => x.QuizzerId == quizzerId).PrimaryKey;

                    await DeleteTeamAssignedQuestionsAsync(id);

                    await _teamMemberRepository.DeleteAsync(id);
                }

                foreach (var quizzerId in quizzerIdsToAdd)
                {
                    var newTeamMember = new TeamMember
                    {
                        PrimaryKey = _teamMemberRepository.GetNewPrimaryKey(),
                        TeamId = _teamId,
                        QuizzerId = quizzerId
                    };

                    var gridTeamMember = TeamMembers.Single(x => x.QuizzerId == quizzerId);

                    gridTeamMember.TeamMemberId = newTeamMember.PrimaryKey;

                    await _teamMemberRepository.AddAsync(newTeamMember);

                    await AddTeamAssignedQuestionsAsync(gridTeamMember);
                }

                foreach (var quizzerId in quizzerIdsToUpdate)
                {
                    var gridTeamMember = TeamMembers.Single(x => x.QuizzerId == quizzerId);
                    await UpdateTeamAssignedQuestionsAsync(gridTeamMember);
                }

                foreach (var item in TeamMembers)
                {
                    item.Persisted = true;
                }
            });

            Closing = new RelayCommand<CancelEventArgs>((CancelEventArgs e) =>
            {
                if(TeamMembers.Any(x => x.HasErrors))
                {
                    e.Cancel = true;
                }

                if(TeamMembers.Any(x => x.NotPersisted))
                {
                    if(_messageBoxService.PromptToContinue("Changes are not saved. If you continue any changes will be lost.") == false)
                    {
                        e.Cancel = true;
                    }
                }
            });
        }

        private async Task AddTeamAssignedQuestionsAsync(ObservableTeamMember newTeamMember)
        {
            foreach(var item in newTeamMember.AssignedQuestions)
            {
                await _assignedRepository.AddAsync(new AssignedQuestion
                {
                    PrimaryKey = _assignedRepository.GetNewPrimaryKey(),
                    TeamMemberId = newTeamMember.TeamMemberId,
                    QuestionId = item.PrimaryKey
                });

                item.Persisted = true;
            }
        }

        private async Task UpdateTeamAssignedQuestionsAsync(ObservableTeamMember newTeamMember)
        {
            var existingAssigned = _assignedRepository.GetByTeamMemberId(newTeamMember.TeamMemberId);
            var existingQuestionIds = existingAssigned.Select(x => x.QuestionId);
            var newQuestionIds = newTeamMember.AssignedQuestions.Select(x => x.PrimaryKey);

            var questionIdsToDelete = existingQuestionIds.Except(newQuestionIds).ToList();
            var questionIdsToAdd = newQuestionIds.Except(existingQuestionIds).ToList();

            foreach(var questionId in questionIdsToDelete)
            {
                var assignedId = existingAssigned.Single(x => x.TeamMemberId == newTeamMember.TeamMemberId && x.QuestionId == questionId).PrimaryKey;
                await _assignedRepository.DeleteAsync(assignedId);
            }

            foreach(var questionId in questionIdsToAdd)
            {
                await _assignedRepository.AddAsync(new AssignedQuestion
                {
                    PrimaryKey= _assignedRepository.GetNewPrimaryKey(),
                    TeamMemberId = newTeamMember.TeamMemberId,
                    QuestionId = questionId
                });
            }

            foreach(var item in newTeamMember.AssignedQuestions)
            {
                item.Persisted = true;
            }
        }

        private async Task DeleteTeamAssignedQuestionsAsync(string teamMemberId)
        {
            await _assignedRepository.DeleteAllByTeamMemberIdAsync(teamMemberId);
        }

        public ObservableCollection<ObservableTeamMember> TeamMembers { get; } = new ObservableCollection<ObservableTeamMember>();

        public ObservableCollection<ObservableAssignedQuestion> Questions { get; } = new ObservableCollection<ObservableAssignedQuestion>();


        #region Commands

        public IRelayCommand Initialize { get; }

        public IRelayCommand<CancelEventArgs> Closing { get; }

        public IRelayCommand Save { get; }

        #endregion
    }
}
