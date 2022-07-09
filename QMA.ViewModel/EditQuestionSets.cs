using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using QMA.DataAccess;
using QMA.Helpers;
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
    public class EditQuestionSets : ItemsEditorObservable<ObservableQuestionSet>
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
                        new AsyncRelayCommand(DeleteAsyncCommand),
                        new AsyncRelayCommand(RestoreAsyncCommand),
                        new AsyncRelayCommand(SaveAsyncCommand)
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
                    new AsyncRelayCommand(DeleteAsyncCommand),
                    new AsyncRelayCommand(RestoreAsyncCommand),
                    new AsyncRelayCommand(SaveAsyncCommand)
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
                    await SaveAsyncCommand();
                    Add.NotifyCanExecuteChanged();
                }
            });
        }

        #region Commands

        public IRelayCommand Initialize { get; }

        public IRelayCommand Add { get; }

        public IRelayCommand<CancelEventArgs> Closing { get; }

        // https://docs.microsoft.com/en-us/windows/communitytoolkit/controls/datagrid_guidance/editing_inputvalidation
        public IRelayCommand<CancelEventArgs> RowEditEnding { get; }

        #endregion

        private async Task DeleteAsyncCommand()
        {
            if (Selected.Deleted != null)
            {
                throw new InvalidOperationException($"Question set ({Selected.PrimaryKey}) is already deleted");
            }

            if (Selected.Persisted == true)
            {
                IsBusy = true;
                try
                {
                    Selected.Deleted = DateTimeOffset.UtcNow;
                    await _repository.UpdateAsync(Selected.GetModel());
                }
                finally
                {
                    IsBusy = false;
                }
            }
            else
            {
                Items.Remove(Selected);
            }
            Add.NotifyCanExecuteChanged();
        }

        private async Task RestoreAsyncCommand()
        {
            if (Selected.Deleted == null)
            {
                throw new InvalidOperationException($"Question set ({Selected.PrimaryKey}) is not deleted");
            }

            IsBusy = true;
            try
            {
                Selected.Deleted = null;
                await _repository.UpdateAsync(Selected.GetModel());
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SaveAsyncCommand()
        {
            if(Selected != null)
            {
                IsBusy = true;
                try
                {
                    if (Selected.Persisted)
                    {
                        await _repository.UpdateAsync(Selected.GetModel());
                    }
                    else
                    {
                        await _repository.AddAsync(Selected.GetModel());
                        Selected.Persisted = true;
                    }
                }
                finally
                {
                    IsBusy = false;
                }
            }
            else
            {
                throw new InvalidOperationException("Save cannot occur with no selected question set");
            }
        }
    }
}
