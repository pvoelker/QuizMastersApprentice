using System;
using QMA.ViewModel.Services;
using System.Windows;
using QMA.ViewModel.Practice;
using System.Linq;

namespace QuizMastersApprenticeApp.Services
{
    public class RunPracticeService : IRunPracticeService
    {
        private Window _owner;

        public RunPracticeService(Window owner)
        {
            _owner = owner;
        }

        public void Start(ConfigurePractice vm)
        {
            var repositoryWindow = _owner as IRepositoryWindow;

            if (repositoryWindow != null)
            {
                var runPracticeWindow = new RunPracticeWindow(repositoryWindow.GetRepositoryFactory(),
                    vm.TeamQuizzers.Where(x => x.IsSelected).Select(x => x.PrimaryKey),
                    vm.SelectedSeason.Name,
                    vm.PracticeQuestions());
                runPracticeWindow.Show();

                _owner.Close();
            }
            else
            {
                throw new InvalidOperationException($"The parent window does not implement {nameof(IRepositoryWindow)}");
            }
        }
    }
}
