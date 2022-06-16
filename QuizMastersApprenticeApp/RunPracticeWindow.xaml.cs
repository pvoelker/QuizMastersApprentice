using QMA.DataAccess;
using QMA.ViewModel.Observables.Practice;
using QMA.ViewModel.Practice;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuizMastersApprenticeApp
{
    /// <summary>
    /// Interaction logic for running a practice
    /// </summary>
    public partial class RunPracticeWindow : Window
    {
        private IRepositoryFactory _repoFactory;

        public RunPracticeWindow(IRepositoryFactory repoFactory,
            IEnumerable<string> quizzerIds,
            string seasonName,
            IEnumerable<ObservablePracticeQuestion> questions)
        {
            _repoFactory = repoFactory;

            InitializeComponent();

            DataContext = new RunPractice(
                new MessageBoxService(this),
                repoFactory.GetQuizzerRepository(),
                quizzerIds,
                seasonName,
                questions);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (RunPractice)DataContext;

            var runPracticeWindow = new PracticeReportWindow(_repoFactory,
                dataContext.SeasonName,
                dataContext.PracticeQuizzers,
                dataContext.TotalQuestionsAsked,
                dataContext.NoAnswerQuestions,
                dataContext.JustLearningQuestions);
            runPracticeWindow.Show();
        }
    }
}
