using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using QMA.DataAccess;
using QMA.Helpers;
using QMA.Model;
using QMA.ViewModel.Observables;
using QMA.ViewModel.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QMA.ViewModel
{
    public class ImportQuestions : ObservableObject
    {
        private IQuestionRepository _repository;

        private IMessageBoxService _messageBoxService;

        public ImportQuestions(IQuestionRepository repository, IMessageBoxService messageBoxService, string questionSetId)
        {
            if(messageBoxService == null)
            {
                throw new ArgumentNullException(nameof(messageBoxService));
            }

            _repository = repository;

            _messageBoxService = messageBoxService;

            _questionSetId = questionSetId;

            Initialize = new RelayCommand(() =>
            {
            });

            Import = new RelayCommand(ImportCommand,
                () =>
                (CsvImport && CsvImportParseSuccess && CsvParsedImportQuestions.Where(x => x.AlreadyExists == false).Count() > 0 && !CsvParsedImportQuestions.Any(x => x.HasParseError)) ||
                (BibleFactPacImport && BfpImportParseSuccess && BfpParsedImportQuestions.Where(x => x.AlreadyExists == false).Count() > 0 && !BfpParsedImportQuestions.Any(x => x.HasParseError))
                );

            Closing = new RelayCommand<CancelEventArgs>((CancelEventArgs e) =>
            {
            });
        }

        private bool _csvImport = true;
        public bool CsvImport
        {
            get => _csvImport;
            set
            {
                SetProperty(ref _csvImport, value, nameof(CsvImport));
                OnPropertyChanged(nameof(BibleFactPacImport));
                Import.NotifyCanExecuteChanged();
            }
        }
        public bool BibleFactPacImport
        {
            get => !_csvImport;
        }

        private string _questionSetId;
        public string QuestionSetId
        {
            get => _questionSetId;
            private set => SetProperty(ref _questionSetId, value, nameof(QuestionSetId));
        }

        public ObservableCollection<ObservableImportQuestion> CsvParsedImportQuestions { get; set; } = new ObservableCollection<ObservableImportQuestion>();

        public ObservableCollection<ObservableImportQuestion> BfpParsedImportQuestions { get; set; } = new ObservableCollection<ObservableImportQuestion>();

        private bool _csvImportParseSuccess = false;
        public bool CsvImportParseSuccess
        {
            get => _csvImportParseSuccess;
            set
            {
                SetProperty(ref _csvImportParseSuccess, value);
                OnPropertyChanged(nameof(CsvImportParseFailed));
                Import.NotifyCanExecuteChanged();
            }
        }
        public bool CsvImportParseFailed
        {
            get => !_csvImportParseSuccess;
        }

        private bool _bfpImportParseSuccess = false;
        public bool BfpImportParseSuccess
        {
            get => _bfpImportParseSuccess;
            set
            {
                SetProperty(ref _bfpImportParseSuccess, value);
                OnPropertyChanged(nameof(BfpImportParseFailed));
                Import.NotifyCanExecuteChanged();
            }
        }
        public bool BfpImportParseFailed
        {
            get => !_bfpImportParseSuccess;
        }

        #region Commands

        public ICommand Initialize { get; }

        public IRelayCommand Import { get; }

        public IRelayCommand<CancelEventArgs> Closing { get; }   

        #endregion

        private void ImportCommand()
        {
            if(CsvImport == true)
            {
                if(_messageBoxService.PromptToContinue($"Are you sure you want to import {CsvParsedImportQuestions.Where(x => x.AlreadyExists == false).Count()} item(s)?"))
                {
                    try
                    {
                        AsyncHelper.RunSync(() => AddQuestions(_questionSetId, CsvParsedImportQuestions));
                    }
                    catch (Exception ex)
                    {
                        _messageBoxService.ShowError(ex.Message);
                    }
                }
            }
            else if(BibleFactPacImport == true)
            {
                if (_messageBoxService.PromptToContinue($"Are you sure you want to import {BfpParsedImportQuestions.Where(x => x.AlreadyExists == false).Count()} item(s)?"))
                {
                    try
                    {
                        AsyncHelper.RunSync(() => AddQuestions(_questionSetId, BfpParsedImportQuestions));
                    }
                    catch (Exception ex)
                    {
                        _messageBoxService.ShowError(ex.Message);
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("A valid import method is not selected");
            }
        }

        private async Task AddQuestions(string questionSetId, ObservableCollection<ObservableImportQuestion> imported)
        {
            foreach(var item in imported.Where(x => x.AlreadyExists == false))
            {
                await _repository.AddAsync(new Question
                {
                    PrimaryKey = Guid.NewGuid().ToString(),
                    QuestionSetId = questionSetId,
                    Number = item.Number,
                    Text = item.Text,
                    Answer = item.Answer,
                    Points = item.Points,
                    Notes = $"Imported on {DateTimeOffset.Now}"
                });
            }
        }
    }
}
