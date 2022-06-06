using QMA.DataAccess;
using QMA.DataAccess.JsonFile;
using QMA.ViewModel.Observables.Practice;
using QMA.ViewModel.Practice;
using QuizMastersApprenticeApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for sending practice reports
    /// </summary>
    public partial class PracticeReportWindow : Window
    {
        private IRepositoryFactory _repoFactory;

        public PracticeReportWindow(IRepositoryFactory repoFactory,
            string seasonName,
            ObservableCollection<ObservablePracticeQuizzer> practiceQuizzers,
            int totalQuestionsAsked,
            ObservableCollection<ObservablePracticeQuestion> noAnswerQuestions,
            ObservableCollection<ObservablePracticeQuestion> justLearningQuestions)
        {
            _repoFactory = repoFactory;

            InitializeComponent();

            var app = (App)Application.Current;

            var appDataFilePath = System.IO.Path.Combine(app.AppDataFolder, app.AppDataFileName);
            DataContext = new PracticeReport(
                new MessageBoxService(this),
                seasonName,
                practiceQuizzers,
                totalQuestionsAsked,
                noAnswerQuestions,
                justLearningQuestions,
                new AppDataRepository(appDataFilePath));
        }
    }
}
