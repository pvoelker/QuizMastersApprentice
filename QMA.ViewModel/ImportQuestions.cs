using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using QMA.Model;
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
        private IMessageBoxService _messageBoxService;

        public ImportQuestions(IMessageBoxService messageBoxService)
        {
            if(messageBoxService == null)
            {
                throw new ArgumentNullException(nameof(messageBoxService));
            }

            _messageBoxService = messageBoxService;

            Initialize = new RelayCommand(() =>
            {
            });

            Import = new RelayCommand(ImportCommand,
                () => CsvImport);

            Closing = new RelayCommand<CancelEventArgs>((CancelEventArgs e) =>
            {
            });
        }

        private bool _csvImport;
        public bool CsvImport
        {
            get => _csvImport;
            set
            {
                SetProperty(ref _csvImport, value, nameof(CsvImport));
                Import.NotifyCanExecuteChanged();
            }
        }

        private string _importFileName;
        public string ImportFileName
        {
            get => _importFileName;
            set => SetProperty(ref _importFileName, value, nameof(ImportFileName));
        }

        public IEnumerable<Question> ImportedQuestions { get; private set; }

        #region Commands

        public ICommand Initialize { get; }

        public IRelayCommand Import { get; }

        public IRelayCommand<CancelEventArgs> Closing { get; }   

        #endregion

        private void ImportCommand()
        {
            if(CsvImport == true)
            {
                try
                {
                    using (var reader = new StreamReader(ImportFileName))
                    {
                        var importer = new Importers.Csv.QuestionImporter(reader);
                        var items = importer.Import();

                        if (_messageBoxService.PromptToContinue($"Do you want to import {items.Count()} item(s)?"))
                        {
                            var importedItems = new List<Question>();

                            foreach (var item in items)
                            {
                                importedItems.Add(new Question
                                {
                                    Number = item.Number,
                                    Text = item.Text,
                                    Answer = item.Answer,
                                    Points = item.Points,
                                });
                            }

                            ImportedQuestions = importedItems;
                        }
                    }
                }
                catch(Exception ex)
                {
                    _messageBoxService.ShowError(ex.Message);
                }
            }
            else
            {
                throw new InvalidOperationException("A valid import method is not selected");
            }
        }
    }
}
