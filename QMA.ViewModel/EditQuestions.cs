using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using QMA.DataAccess;
using QMA.Model;
using QMA.ViewModel.Observables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using QMA.ViewModel.Services;
using QMA.Helpers;

namespace QMA.ViewModel
{
    public class EditQuestions : ItemsEditorObservable<ObservableQuestion>
    {
        private ISaveFileDialogService _saveFileDialogService;

        private IQuestionRepository _repository;

        private string _questionSetId;

        public EditQuestions(ISaveFileDialogService saveFileDialogService, IQuestionRepository repository, string questionSetId)
        {
            _saveFileDialogService = saveFileDialogService;

            _repository = repository;

            _questionSetId = questionSetId;

            Initialize = new RelayCommand(() =>
            {
                var items = _repository.GetByQuestionSetId(_questionSetId, true);

                Items.Clear();
                foreach(var item in items)
                {
                    var newItem = new ObservableQuestion(
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
                var newItem = new ObservableQuestion(
                    false,
                    new Question
                    {
                        PrimaryKey = Guid.NewGuid().ToString(),
                        QuestionSetId = _questionSetId,
                        Text = $"Question {Items.Count + 1}"
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

            Export = new RelayCommand(() =>
            {
                var saveFilePath = _saveFileDialogService.Show("Export Questions as JSON File",
                    new List<SaveFileFilter> {
                        new SaveFileFilter("JSON File", "*.json")
                    }, 1);

                if (saveFilePath != null)
                {
                    var exportItems = Items.Select(x => new ImportQuestion
                    {
                        Number = x.Number,
                        Text = x.Text,
                        Answer = x.Answer,
                        Points = x.Points
                    });

                    var json = JsonSerializer.Serialize(exportItems,
                        new JsonSerializerOptions
                        {
                            WriteIndented = true
                        });
                    File.WriteAllText(saveFilePath, json);
                }
            });
        }

        #region Commands

        public IRelayCommand Initialize { get; }

        public IRelayCommand Add { get; }

        public IRelayCommand<CancelEventArgs> Closing { get; }

        // https://docs.microsoft.com/en-us/windows/communitytoolkit/controls/datagrid_guidance/editing_inputvalidation
        public IRelayCommand<CancelEventArgs> RowEditEnding { get; }
        
        public IRelayCommand Export { get; }

        #endregion

        private async Task DeleteAsyncCommand()
        {
            if (Selected.Deleted != null)
            {
                throw new InvalidOperationException($"Question ({Selected.PrimaryKey}) is already deleted");
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
                throw new InvalidOperationException($"Question ({Selected.PrimaryKey}) is not deleted");
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
                throw new InvalidOperationException("Save cannot occur with no selected question");
            }
        }
    }
}
