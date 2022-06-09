using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using QMA.DataAccess;
using QMA.Model;
using QMA.Model.Season;
using QMA.ViewModel.Observables;
using QMA.ViewModel.Observables.Season;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace QMA.ViewModel.Season
{
    public class EditTeams : ObservableObject
    {
        private ITeamRepository _repository;

        private string _seasonId;

        public EditTeams(ITeamRepository repository, string seasonId)
        {
            _repository = repository;

            _seasonId = seasonId;

            Initialize = new RelayCommand(() =>
            {
                var items = _repository.GetBySeasonId(_seasonId);

                foreach(var item in items)
                {
                    var newItem = new ObservableTeam(
                        true,
                        item,
                        new RelayCommand(DeleteCommand),
                        new RelayCommand(RestoreCommand),
                        new RelayCommand(SaveCommand)
                    );
                    Items.Add(newItem);
                }
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
                    new RelayCommand(DeleteCommand),
                    new RelayCommand(RestoreCommand),
                    new RelayCommand(SaveCommand)
                );
                Items.Add(newItem);
                Selected = newItem;
                Add.NotifyCanExecuteChanged();
            },
            () => !Items.Any(x => x.HasErrors));

            Closing = new RelayCommand<CancelEventArgs>((CancelEventArgs e) =>
            {
                if(Items.Any(x => x.HasErrors))
                {
                    e.Cancel = true;
                }
            });

            RowEditEnding = new AsyncRelayCommand<CancelEventArgs>(async (CancelEventArgs e) =>
            {
                if (Selected.HasErrors)
                {
                    e.Cancel = true;
                }
                else
                {
                    SaveCommand();
                    Add.NotifyCanExecuteChanged();
                }
            });
        }

        public ObservableCollection<ObservableTeam> Items { get; } = new ObservableCollection<ObservableTeam>();

        public ObservableTeam Selected { get; set; }

        #region Commands

        public IRelayCommand Initialize { get; }

        public IRelayCommand Add { get; }

        public IRelayCommand<CancelEventArgs> Closing { get; }

        // https://docs.microsoft.com/en-us/windows/communitytoolkit/controls/datagrid_guidance/editing_inputvalidation
        public IRelayCommand<CancelEventArgs> RowEditEnding { get; }

        #endregion

        private void DeleteCommand()
        {
            if (Selected.Deleted != null)
            {
                throw new InvalidOperationException($"Team ({Selected.PrimaryKey}) is already deleted");
            }

            if (Selected.Persisted == true)
            {
                Selected.Deleted = DateTimeOffset.UtcNow;
                _repository.Update(Selected.GetModel());
            }
            else
            {
                Items.Remove(Selected);
            }
        }

        private void RestoreCommand()
        {
            if (Selected.Deleted == null)
            {
                throw new InvalidOperationException($"Team ({Selected.PrimaryKey}) is not deleted");
            }

            Selected.Deleted = null;
            _repository.Update(Selected.GetModel());
        }

        private void SaveCommand()
        {
            if(Selected != null)
            {
                if (Selected.Persisted)
                {
                    _repository.Update(Selected.GetModel());
                }
                else
                {
                    _repository.Add(Selected.GetModel());
                    Selected.Persisted = true;
                }
            }
            else
            {
                throw new InvalidOperationException("Save cannot occur with no selected team");
            }
        }
    }
}
