using QMA.DataAccess;
using QMA.ViewModel;
using QMA.ViewModel.Season;
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
    /// Interaction logic for EditTeamsWindow.xaml
    /// </summary>
    public partial class EditTeamsWindow : Window
    {
        private IRepositoryFactory _repoFactory;

        public EditTeamsWindow(string seasonId, IRepositoryFactory repoFactory)
        {
            _repoFactory = repoFactory;

            InitializeComponent();

            DataContext = new EditTeams(repoFactory.GetTeamRepository(), seasonId);
        }

        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var rowEditEndingCommand = ((EditTeams)DataContext).RowEditEnding;
                if (rowEditEndingCommand != null)
                {
                    var eventArgs = new CancelEventArgs();
                    rowEditEndingCommand.Execute(eventArgs);
                    e.Cancel = eventArgs.Cancel;
                }
            }
        }
        private void EditTeamMembersButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = ((EditTeams)DataContext).Selected;

            var win = new EditTeamMembersWindow(selected.PrimaryKey, _repoFactory);
            win.ShowDialog();
        }
    }
}
