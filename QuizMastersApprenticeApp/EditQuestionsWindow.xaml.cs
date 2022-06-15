using QMA.DataAccess;
using QMA.ViewModel;
using QuizMastersApprenticeApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace QuizMastersApprenticeApp
{
    /// <summary>
    /// Interaction logic for EditQuestionsWindow.xaml
    /// </summary>
    public partial class EditQuestionsWindow : Window
    {
        private readonly string _questionsSetId;

        private readonly IRepositoryFactory _repoFactory;

        public EditQuestionsWindow(string questionSetId, IRepositoryFactory repoFactory)
        {
            InitializeComponent();

            _questionsSetId = questionSetId;

            _repoFactory = repoFactory;

            DataContext = new EditQuestions(new SaveFileDialogService(this),
                repoFactory.GetQuestionRepository(),
                questionSetId);
        }

        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var rowEditEndingCommand = ((EditQuestions)DataContext).RowEditEnding;
                if (rowEditEndingCommand != null)
                {
                    var eventArgs = new CancelEventArgs();
                    rowEditEndingCommand.Execute(eventArgs);
                    e.Cancel = eventArgs.Cancel;
                }
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            var importDlg = new ImportQuestionsWindow(_questionsSetId, _repoFactory);
            importDlg.ShowDialog();

            var thisDataContext = DataContext as EditQuestions;
            var dataContext = importDlg.DataContext as ImportQuestions;

            if(dataContext != null)
            {
            }
            else
            {
                MessageBox.Show("Nothing to import");
            }
        }
    }
}
