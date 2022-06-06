using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using QMA.DataAccess;
using QMA.Model;
using QMA.ViewModel.Observables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QMA.ViewModel
{
    public class EditQuestionSets : ObservableObject
    {
        private IQuestionSetRepository _repository;

        public EditQuestionSets(IQuestionSetRepository repository)
        {
            _repository = repository;

            Initialize = new RelayCommand(() =>
            {
                var items = _repository.GetAll();

                foreach(var item in items)
                {
                    var newItem = new ObservableQuestionSet(
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
                var newItem = new ObservableQuestionSet(
                    false,
                    new QuestionSet
                    {
                        PrimaryKey = Guid.NewGuid().ToString(),
                        Name = $"Question Set {Items.Count + 1}",
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
                    if (Selected.Persisted)
                    {
                        _repository.Update(Selected.GetModel());
                    }
                    else
                    {
                        _repository.Add(Selected.GetModel());
                        Selected.Persisted = true;
                    }
                    Add.NotifyCanExecuteChanged();
                }
            });
        }

        public ObservableCollection<ObservableQuestionSet> Items { get; } = new ObservableCollection<ObservableQuestionSet>();

        public ObservableQuestionSet Selected { get; set; }

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
                throw new InvalidOperationException($"Question set ({Selected.PrimaryKey}) is already deleted");
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
            Add.NotifyCanExecuteChanged();
        }

        private void RestoreCommand()
        {
            if (Selected.Deleted == null)
            {
                throw new InvalidOperationException($"Question set ({Selected.PrimaryKey}) is not deleted");
            }

            Selected.Deleted = null;
            _repository.Update(Selected.GetModel());
        }

        private void SaveCommand()
        {
            if(Selected != null)
            {
                if(Selected.PrimaryKey == default)
                {
                    Selected.GetModel().PrimaryKey = Guid.NewGuid().ToString();
                    _repository.Add(Selected.GetModel());
                }
                else
                {
                    _repository.Update(Selected.GetModel());
                }
            }
            else
            {
                throw new InvalidOperationException("Save cannot occur with no selected quizzer");
            }
        }
    }
}
