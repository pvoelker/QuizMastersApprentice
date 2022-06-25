using QMA.DataAccess;
using QMA.ViewModel;
using QMA.ViewModel.Season;
using QuizMastersApprenticeApp.Services;
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
    /// Interaction logic for EditTeamMembersWindow.xaml
    /// </summary>
    public partial class EditTeamMembersWindow : Window
    {
        public EditTeamMembersWindow(string teamId, string questionSetId, IRepositoryFactory repoFactory)
        {
            InitializeComponent();

            DataContext = new EditTeamMembers(
                new MessageBoxService(this),
                repoFactory.GetTeamMemberRepository(),
                repoFactory.GetAssignedQuestionRepository(),
                repoFactory.GetQuizzerRepository(),
                repoFactory.GetQuestionRepository(),
                teamId,
                questionSetId);
        }
    }
}
