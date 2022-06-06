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

namespace QMA.ViewModel
{
    public class EditQuestions : ObservableObject
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

                foreach(var item in items)
                {
                    var newItem = new ObservableQuestion(
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
                var newItem = new ObservableQuestion(
                    false,
                    new Question
                    {
                        PrimaryKey = Guid.NewGuid().ToString(),
                        QuestionSetId = _questionSetId,
                        Text = $"Question {Items.Count + 1}"
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

        public ObservableCollection<ObservableQuestion> Items { get; } = new ObservableCollection<ObservableQuestion>();

        public ObservableQuestion Selected { get; set; }

        #region Commands

        public IRelayCommand Initialize { get; }

        public IRelayCommand Add { get; }

        public IRelayCommand<CancelEventArgs> Closing { get; }

        // https://docs.microsoft.com/en-us/windows/communitytoolkit/controls/datagrid_guidance/editing_inputvalidation
        public IRelayCommand<CancelEventArgs> RowEditEnding { get; }
        
        public IRelayCommand Export { get; }

        #endregion

        private void DeleteCommand()
        {
            if (Selected.Deleted != null)
            {
                throw new InvalidOperationException($"Question ({Selected.PrimaryKey}) is already deleted");
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
                throw new InvalidOperationException($"Question ({Selected.PrimaryKey}) is not deleted");
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
