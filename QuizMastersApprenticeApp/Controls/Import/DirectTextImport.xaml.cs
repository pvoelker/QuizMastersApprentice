using Microsoft.Toolkit.Mvvm.Input;
using QMA.DataAccess;
using QMA.Helpers;
using QMA.Importers;
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
    public partial class DirectTextImport : UserControl
    {
        private IMessageBoxService _messageBoxService;
        private IQuestionRepository _repository;

        public DirectTextImport()
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

        #region Data

        public static readonly DependencyProperty ImporterProperty = DependencyProperty.Register(nameof(Importer), typeof(IQuestionImporter), typeof(DirectTextImport),
            new FrameworkPropertyMetadata(null, null));

        public IQuestionImporter Importer
        {
            get { return (IQuestionImporter)GetValue(ImporterProperty); }
            set { SetValue(ImporterProperty, value); }
        }

        public static readonly DependencyProperty QuestionSetIdProperty = DependencyProperty.Register(nameof(QuestionSetId), typeof(string), typeof(DirectTextImport),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public string QuestionSetId
        {
            get { return (string)GetValue(QuestionSetIdProperty); }
            set { SetValue(QuestionSetIdProperty, value); }
        }

        public static readonly DependencyProperty ImportParseSuccessProperty = DependencyProperty.Register(nameof(ImportParseSuccess), typeof(bool), typeof(DirectTextImport),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool ImportParseSuccess
        {
            get { return (bool)GetValue(ImportParseSuccessProperty); }
            set { SetValue(ImportParseSuccessProperty, value); }
        }

        public static readonly DependencyProperty ParsedImportQuestionsProperty = DependencyProperty.Register(nameof(ParsedImportQuestions), typeof(ObservableCollection<ObservableImportQuestion>), typeof(DirectTextImport),
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

            try
            {
                var importedQuestions = Importer.Import(new StreamReader(_importText.Text.ToStream()));

                ParsedImportQuestions.Clear();
                foreach (var item in importedQuestions)
                {
                    ParsedImportQuestions.Add(new ObservableImportQuestion(item));
                }

                foreach (var item in ParsedImportQuestions)
                {
                    var existing = _repository.GetByQuestionNumber(QuestionSetId, item.Number, false);
                    if (existing.Count() == 1)
                    {
                        item.AlreadyExists = true;

                        var found = existing.First();
                        if (found.Text != item.Text)
                        {
                            item.ParseError = "Question Mismatch";
                        }
                        else if (found.Answer != item.Answer)
                        {
                            item.ParseError = "Answer Mismatch";
                        }
                        else if (found.Points != item.Points)
                        {
                            item.ParseError = "Points Mismatch";
                        }
                    }
                    else if (existing.Count() > 1)
                    {
                        item.AlreadyExists = true;

                        item.ParseError = "Multiples Found";
                    }
                }

                // This needs to happen after after error checking
                ImportParseSuccess = true;

                UpdateButtonsEnable();
            }
            catch (ImportFailedException ex)
            {
                ImportParseSuccess = false;

                UpdateButtonsEnable();

                _messageBoxService.ShowError($"Unable to parse text: {ex.Message}");
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            _parseText.IsEnabled = (Importer != null) && (_repository != null) &&
                (_importText.Text != null && _importText.Text.Count() > 0) && !ImportParseSuccess;
            _clearImport.IsEnabled = ImportParseSuccess;
            _parsedData.IsEnabled = ImportParseSuccess;
        }
    }
}
