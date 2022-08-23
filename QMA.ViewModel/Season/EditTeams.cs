using CommunityToolkit.Mvvm.Input;
using QMA.DataAccess;
using QMA.Model.Season;
using QMA.ViewModel.Observables.Season;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QMA.ViewModel.Season
{
    public class EditTeams : ItemsEditorObservable<Team, ObservableTeam, ITeamRepository>
    {
        private string _seasonId;

        public EditTeams(ITeamRepository repository, string seasonId)
        {
            _repository = repository;

            _seasonId = seasonId;

            Initialize = new RelayCommand(() =>
            {
                ShowBusy(() =>
                {
                    var items = _repository.GetBySeasonId(_seasonId);

                    foreach (var item in items)
                    {
                        var newItem = new ObservableTeam(
                            true,
                            item,
                            new AsyncRelayCommand(SoftDeleteAsyncCommand),
                            new AsyncRelayCommand(RestoreAsyncCommand),
                            new AsyncRelayCommand(SaveAsyncCommand)
                        );
                        Items.Add(newItem);
                    }
                });
                Add.NotifyCanExecuteChanged();
            });

            Add = new RelayCommand(() =>
            {
                var newItem = new ObservableTeam(
                    false,
                    new Team
                    {
                        PrimaryKey = Guid.NewGuid().ToString(),
                        SeasonId = _seasonId,
                        Name = $"Team {Items.Count + 1}",
                    },
                    new AsyncRelayCommand(SoftDeleteAsyncCommand),
                    new AsyncRelayCommand(RestoreAsyncCommand),
                    new AsyncRelayCommand(SaveAsyncCommand)
                );
                Items.Add(newItem);
                Selected = newItem;
                Add.NotifyCanExecuteChanged();
            },
            () => !Items.Any(x => x.HasErrors));
        }

        #region Commands

        #endregion
    }
}
