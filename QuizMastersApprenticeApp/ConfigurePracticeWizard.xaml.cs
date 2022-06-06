using QMA.DataAccess;
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
    /// Interaction logic for Practice.xaml
    /// </summary>
    public partial class ConfigurePracticeWizard : Window, IRepositoryWindow
    {
        private IRepositoryFactory _repoFactory;

        public ConfigurePracticeWizard(IRepositoryFactory repoFactory)
        {
            _repoFactory = repoFactory;

            InitializeComponent();

            DataContext = new ConfigurePractice(
                new MessageBoxService(this),
                new RunPracticeService(this),
                repoFactory.GetSeasonRepository(),
                repoFactory.GetTeamRepository(),
                repoFactory.GetQuizzerRepository(),
                repoFactory.GetTeamMemberRepository(),
                repoFactory.GetQuestionSetRepository(),
                repoFactory.GetQuestionRepository()); ;
        }

        public IRepositoryFactory GetRepositoryFactory()
        {
            return _repoFactory;
        }
    }
}
