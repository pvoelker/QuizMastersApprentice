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
    /// Interaction logic for EditSeasonsWindow.xaml
    /// </summary>
    public partial class EditSeasonsWindow : Window
    {
        private IRepositoryFactory _repoFactory;

        public EditSeasonsWindow(IRepositoryFactory repoFactory)
        {
            _repoFactory = repoFactory;

            InitializeComponent();

            DataContext = new EditSeasons(
                repoFactory.GetSeasonRepository(),
                repoFactory.GetQuestionSetRepository());
        }

        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var rowEditEndingCommand = ((EditSeasons)DataContext).RowEditEnding;
                if (rowEditEndingCommand != null)
                {
                    var eventArgs = new CancelEventArgs();
                    rowEditEndingCommand.Execute(eventArgs);
                    e.Cancel = eventArgs.Cancel;
                }
            }
        }

        private void EditTeamsButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = ((EditSeasons)DataContext).Selected;

            var win = new EditTeamsWindow(selected.PrimaryKey, selected.QuestionSetId, _repoFactory);
            win.ShowDialog();
        }
    }
}
