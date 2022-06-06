using QMA.DataAccess;
using QMA.ViewModel;
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
    /// Interaction logic for EditQuestionSetsWindow.xaml
    /// </summary>
    public partial class EditQuestionSetsWindow : Window
    {
        private IRepositoryFactory _repoFactory;

        public EditQuestionSetsWindow(IRepositoryFactory repoFactory)
        {
            _repoFactory = repoFactory;

            InitializeComponent();

            DataContext = new EditQuestionSets(repoFactory.GetQuestionSetRepository());
        }

        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var rowEditEndingCommand = ((EditQuestionSets)DataContext).RowEditEnding;
                if (rowEditEndingCommand != null)
                {
                    var eventArgs = new CancelEventArgs();
                    rowEditEndingCommand.Execute(eventArgs);
                    e.Cancel = eventArgs.Cancel;
                }
            }
        }

        private void EditQuestionsButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = ((EditQuestionSets)DataContext).Selected;

            var win = new EditQuestionsWindow(selected.PrimaryKey, _repoFactory);
            win.ShowDialog();
        }
    }
}
