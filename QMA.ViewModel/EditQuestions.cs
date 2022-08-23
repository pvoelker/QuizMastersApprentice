using CommunityToolkit.Mvvm.Input;
using QMA.DataAccess;
using QMA.Model;
using QMA.ViewModel.Observables;
using System;
using System.Collections.Generic;
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
    public class EditQuestions : ItemsEditorObservable<Question, ObservableQuestion, IQuestionRepository>
    {
        private ISaveFileDialogService _saveFileDialogService;

        private string _questionSetId;

        public EditQuestions(ISaveFileDialogService saveFileDialogService, IQuestionRepository repository, string questionSetId)
        {
            _saveFileDialogService = saveFileDialogService;

            _repository = repository;

            _questionSetId = questionSetId;

            Initialize = new RelayCommand(() =>
            {
                ShowBusy(() =>
                {
                    var items = _repository.GetByQuestionSetId(_questionSetId, true);

                    Items.Clear();
                    foreach (var item in items)
                    {
                        var newItem = new ObservableQuestion(
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
                var newItem = new ObservableQuestion(
                    false,
                    new Question
                    {
                        PrimaryKey = Guid.NewGuid().ToString(),
                        QuestionSetId = _questionSetId,
                        Text = $"Question {Items.Count + 1}"
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
       
        public IRelayCommand Export { get; }

        #endregion
    }
}
