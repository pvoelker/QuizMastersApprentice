using QMA.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace QuizMastersApprenticeApp
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        private IRepositoryFactory _repoFactory;

        public MainMenu(IRepositoryFactory repoFactory)
        {
            _repoFactory = repoFactory;

            InitializeComponent();
        }

        private void QuestionSets_Click(object sender, RoutedEventArgs e)
        {
            var win = new EditQuestionSetsWindow(_repoFactory);
            win.ShowDialog();
        }

        private void Quizzers_Click(object sender, RoutedEventArgs e)
        {
            var win = new EditQuizzersWindow(_repoFactory);
            win.ShowDialog();
        }

        private void Seasons_Click(object sender, RoutedEventArgs e)
        {
            var win = new EditSeasonsWindow(_repoFactory);
            win.ShowDialog();
        }

        private void Practice_Click(object sender, RoutedEventArgs e)
        {
            var win = new ConfigurePracticeWizard(_repoFactory);
            win.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var version = assembly.GetName().Version;

            if (version != null)
            {
                tbVersion.Text = $" version {version}";
            }
            else
            {
                tbVersion.Text = "Error: Unable to retrieve version info";
            }

            var copyright = "Error: Unable to retrieve copyright";
            object[] attribs = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), true);
            if (attribs.Length > 0)
            {
                copyright = ((AssemblyCopyrightAttribute)attribs[0]).Copyright;
            }

            tbCopyright.Text = copyright;
        }
    }
}
