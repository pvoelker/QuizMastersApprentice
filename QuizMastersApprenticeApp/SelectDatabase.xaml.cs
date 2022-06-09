using QMA.DataAccess.JsonFile;
using QuizMastersApprenticeApp.Services;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for SelectDatabase.xaml
    /// </summary>
    public partial class SelectDatabase : Window
    {
        public SelectDatabase()
        {
            var app = (App)Application.Current;

            var appDataFilePath = System.IO.Path.Combine(app.AppDataFolder, app.AppDataFileName);
            DataContext = new QMA.ViewModel.SelectDatabase(
                new OpenFileDialogService(this),
                new AppDataRepository(appDataFilePath));

            InitializeComponent();
        }

        private void NewDatabase_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (QMA.ViewModel.SelectDatabase)DataContext;

            dataContext.NewConnection.Execute(null);

            if (dataContext.RepositoryFactory != null)
            {
                var mainMenu = new MainMenu(dataContext.RepositoryFactory);
                mainMenu.Show();

                Close();
            }
        }

        private void Databases_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dataContext = (QMA.ViewModel.SelectDatabase)DataContext;

            dataContext.OldConnection.Execute(null);

            if (dataContext.RepositoryFactory != null)
            {
                var mainMenu = new MainMenu(dataContext.RepositoryFactory);
                mainMenu.Show();

                Close();
            }
        }
    }
}
