using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using QMA.DataAccess;
using QMA.Helpers;
using QMA.Model;
using QMA.ViewModel.Observables;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QMA.ViewModel
{
    public class EditQuizzers : ItemsEditorObservable<ObservableQuizzer>
    {
        private IQuizzerRepository _repository;

        public EditQuizzers(IQuizzerRepository repository)
        {
            _repository = repository;

            Initialize = new RelayCommand(() =>
            {
                var items = _repository.GetAll(true);

                foreach (var item in items)
                {
                    var newItem = new ObservableQuizzer(
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
                var newItem = new ObservableQuizzer(
                    false,
                    new Quizzer
                    {
                        PrimaryKey = Guid.NewGuid().ToString(),
                        FirstName = null,
                        LastName = null,
                        ParentFullName = null
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

            //https://social.msdn.microsoft.com/Forums/en-US/72169f1b-1d2a-42f6-9918-63a45975e982/mvvm-close-window-event?forum=wpf
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
            if(Selected.Deleted != null)
            {
                throw new InvalidOperationException($"Quizzer ({Selected.PrimaryKey}) is already deleted");
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
                throw new InvalidOperationException($"Quizzer ({Selected.PrimaryKey}) is not deleted");
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
                throw new InvalidOperationException("Save cannot occur with no selected quizzer");
            }
        }
    }
}
