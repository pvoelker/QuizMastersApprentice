using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using QMA.DataAccess;
using QMA.Model.Season;
using QMA.ViewModel.Observables.Season;
using QMA.ViewModel.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace QMA.ViewModel.Season
{
    public class EditTeamMembers : ObservableObject
    {
        private IMessageBoxService _messageBoxService;

        private ITeamMemberRepository _repository;

        private string _teamId;
        public EditTeamMembers(
            IMessageBoxService messageBoxService,
            ITeamMemberRepository repository,
            IQuizzerRepository quizzerRepository,
            string teamId)
        {
            _messageBoxService = messageBoxService;

            _repository = repository;

            _teamId = teamId;

            Initialize = new RelayCommand(() =>
            {
                var quizzers = quizzerRepository.GetAll(true);
                // TODO: What to do with deleted quizzers?
                foreach (var quizzer in quizzers)
                {
                    var newItem = new ObservableTeamMember(
                        true,
                        quizzer.PrimaryKey,
                        $"{quizzer.LastName}, {quizzer.FirstName}"
                    );
                    Items.Add(newItem);
                }

                var items = _repository.GetByTeamId(_teamId);

                foreach(var item in items)
                {
                    var found = Items.SingleOrDefault(x => x.QuizzerId == item.QuizzerId);
                    found.IsMember = true;
                    found.Persisted = true;
                }
            });

            Save = new RelayCommand<CancelEventArgs>((CancelEventArgs e) =>
            {
                var existingQuizzers = _repository.GetByTeamId(_teamId);
                var existingQuizzerIds = existingQuizzers.Select(x => x.QuizzerId);
                var newQuizzerIds = Items.Where(x => x.IsMember == true).Select(x => x.QuizzerId);

                var quizzerIdsToDelete = existingQuizzerIds.Except(newQuizzerIds);
                var quizzerIdsToAdd = newQuizzerIds.Except(existingQuizzerIds);

                foreach (var quizzerId in quizzerIdsToDelete)
                {
                    var id = existingQuizzers.Single(x => x.QuizzerId == quizzerId).PrimaryKey;
                    _repository.Delete(id);
                }

                foreach (var quizzerId in quizzerIdsToAdd)
                {
                    _repository.Add(new TeamMember {
                        PrimaryKey = Guid.NewGuid().ToString(),
                        TeamId = _teamId,
                        QuizzerId = quizzerId
                    });
                }

                foreach(var item in Items)
                {
                    item.Persisted = true;
                }
            });

            Closing = new RelayCommand<CancelEventArgs>((CancelEventArgs e) =>
            {
                if(Items.Any(x => x.HasErrors))
                {
                    e.Cancel = true;
                }

                if(Items.Any(x => x.NotPersisted))
                {
                    if(_messageBoxService.PromptToContinue("Changes are not saved. If you continue any changes will be lost.") == false)
                    {
                        e.Cancel = true;
                    }
                }
            });
        }

        public ObservableCollection<ObservableTeamMember> Items { get; } = new ObservableCollection<ObservableTeamMember>();

        #region Commands

        public IRelayCommand Initialize { get; }

        public IRelayCommand<CancelEventArgs> Closing { get; }

        public IRelayCommand Save { get; }

        #endregion
    }
}
