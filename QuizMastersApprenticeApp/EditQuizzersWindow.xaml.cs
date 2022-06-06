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

namespace QuizMastersApprenticeApp //https://github.com/Microsoft/XamlBehaviorsWpf/wiki
{
    /// <summary>
    /// Interaction logic for EditQuizzersWindow.xaml
    /// </summary>
    public partial class EditQuizzersWindow : Window
    {
        public EditQuizzersWindow(IRepositoryFactory repoFactory)
        {
            InitializeComponent();

            DataContext = new EditQuizzers(repoFactory.GetQuizzerRepository());
        }

        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var rowEditEndingCommand = ((EditQuizzers)DataContext).RowEditEnding;
                if (rowEditEndingCommand != null)
                {
                    var eventArgs = new CancelEventArgs();
                    rowEditEndingCommand.Execute(eventArgs);
                    e.Cancel = eventArgs.Cancel;
                }
            }
        }
    }
}
