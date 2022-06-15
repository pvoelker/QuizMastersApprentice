using Microsoft.Toolkit.Mvvm.Input;
using QMA.DataAccess;
using QMA.Helpers;
using QMA.Importers;
using QMA.Importers.ViewModel;
using QMA.ViewModel.Observables;
using QMA.ViewModel.Services;
using QuizMastersApprenticeApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuizMastersApprenticeApp.Controls.Import
{
    /// <summary>
    /// Interaction logic for DirectTextImport.xaml
    /// </summary>
    public partial class FileTextImport : UserControl
    {
        private IMessageBoxService _messageBoxService;
        private IQuestionRepository _repository;

        public FileTextImport()
        {
            InitializeComponent();
        }

        public void Initialize(IQuestionRepository repository)
        {
            _repository = repository;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);

            _messageBoxService = new MessageBoxService(parentWindow);

            UpdateButtonsEnable();
        }

        private void _importFile_FileNameChanged(object sender, EventArgs e)
        {
            UpdateButtonsEnable();
        }

        #region Data

        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof(Filter), typeof(string), typeof(FileTextImport),
            new FrameworkPropertyMetadata((string)string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

        public string Filter
        {
            get { return (string)GetValue(FilterProperty) ?? string.Empty; }
            set { SetValue(FilterProperty, value); }
        }

        public static readonly DependencyProperty ImporterProperty = DependencyProperty.Register(nameof(Importer), typeof(IQuestionImporter), typeof(FileTextImport),
            new FrameworkPropertyMetadata(null, null));

        public IQuestionImporter Importer
        {
            get { return (IQuestionImporter)GetValue(ImporterProperty); }
            set { SetValue(ImporterProperty, value); }
        }

        public static readonly DependencyProperty QuestionSetIdProperty = DependencyProperty.Register(nameof(QuestionSetId), typeof(string), typeof(FileTextImport),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public string QuestionSetId
        {
            get { return (string)GetValue(QuestionSetIdProperty); }
            set { SetValue(QuestionSetIdProperty, value); }
        }

        public static readonly DependencyProperty ImportParseSuccessProperty = DependencyProperty.Register(nameof(ImportParseSuccess), typeof(bool), typeof(FileTextImport),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool ImportParseSuccess
        {
            get { return (bool)GetValue(ImportParseSuccessProperty); }
            set { SetValue(ImportParseSuccessProperty, value); }
        }

        public static readonly DependencyProperty ParsedImportQuestionsProperty = DependencyProperty.Register(nameof(ParsedImportQuestions), typeof(ObservableCollection<ObservableImportQuestion>), typeof(FileTextImport),
            new FrameworkPropertyMetadata(new ObservableCollection<ObservableImportQuestion>(), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ObservableCollection<ObservableImportQuestion> ParsedImportQuestions
        {
            get { return (ObservableCollection<ObservableImportQuestion>)GetValue(ParsedImportQuestionsProperty); }
            set { SetValue(ParsedImportQuestionsProperty, value); }
        }

        #endregion

        private void ParseText_Click(object sender, RoutedEventArgs e)
        {
            if(Importer == null)
            {
                throw new ArgumentNullException(nameof(Importer), "An importer must be specified on the control");
            }

            if(File.Exists(_importFile.FilePath))
            {
                try
                {
                    var importedQuestions = Importer.Import(new StreamReader(_importFile.FilePath));

                    ParsedImportQuestions.Clear();
                    foreach (var item in importedQuestions)
                    {
                        ParsedImportQuestions.Add(new ObservableImportQuestion(item));
                    }

                    var checker = new QuestionImportCheck();

                    checker.CheckAgainstQuestionSet(ParsedImportQuestions, _repository, QuestionSetId);

                    // This needs to happen after after error checking
                    ImportParseSuccess = true;

                    UpdateButtonsEnable();
                }
                catch (ImportMissingHeadersException ex)
                {
                    ImportParseSuccess = false;

                    UpdateButtonsEnable();

                    _messageBoxService.ShowError($"Unable to parse text: {ex.Message}");
                }
                catch (ImportFailedException ex)
                {
                    ImportParseSuccess = false;

                    UpdateButtonsEnable();

                    _messageBoxService.ShowError($"Unable to parse text: {ex.Message}");
                }
            }
            else
            {
                _messageBoxService.ShowError($"File '{_importFile.FilePath}' not found");
            }
        }

        private void ClearImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ParsedImportQuestions.Clear();
                ImportParseSuccess = false;
            }
            finally
            {
                UpdateButtonsEnable();
            }
        }

        private void UpdateButtonsEnable()
        {            
            _parseText.IsEnabled = (Importer != null) && (_repository != null) &&
                (_importFile.FilePath != null) && (_importFile.FilePath.Count() > 0) &&
                !ImportParseSuccess;
            _importFile.IsEnabled = !ImportParseSuccess;
            _clearImport.IsEnabled = ImportParseSuccess;
            _parsedData.IsEnabled = ImportParseSuccess;
        }
    }
}
