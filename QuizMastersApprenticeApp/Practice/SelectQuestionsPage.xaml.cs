using QMA.ViewModel.Practice;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuizMastersApprenticeApp.Practice
{
    /// <summary>
    /// Interaction logic for SelectQuestionsPage.xaml
    /// </summary>
    public partial class SelectQuestionsPage : UserControl
    {
        public SelectQuestionsPage()
        {
            InitializeComponent();
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    var dataContext = (ConfigurePractice)DataContext;
        //    if (dataContext.QuestionSetImport != null)
        //    {
        //        dataContext.SelectQuestions.Execute(null);
        //    }

        //    var parentWindow = Window.GetWindow(this);

        //    var repositoryWindow = parentWindow as IRepositoryWindow;

        //    if (repositoryWindow != null)
        //    {
        //        var runPracticeWindow = new RunPracticeWindow(repositoryWindow.GetRepositoryFactory(),
        //            dataContext.TeamQuizzers.Where(x => x.IsSelected).Select(x => x.PrimaryKey),
        //            dataContext.SelectedSeason.Name,
        //            dataContext.PracticeQuestions());
        //        runPracticeWindow.Show();

        //        parentWindow.Close();
        //    }
        //    else
        //    {
        //        throw new InvalidOperationException($"The parent window does not implement {nameof(IRepositoryWindow)}");
        //    }
        //}
    }
}
