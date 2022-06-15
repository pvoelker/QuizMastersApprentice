using QMA.DataAccess;
using QMA.ViewModel;
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
    /// Interaction logic for EditQuestionsWindow.xaml
    /// </summary>
    public partial class ImportQuestionsWindow : Window
    {
        public ImportQuestionsWindow(string questionSetId, IRepositoryFactory repoFactory)
        {
            InitializeComponent();

            _csvImport.Initialize(repoFactory.GetQuestionRepository());
            _bfpImport.Initialize(repoFactory.GetQuestionRepository());

            DataContext = new ImportQuestions(repoFactory.GetQuestionRepository(),
                new MessageBoxService(this), questionSetId);
        }
    }
}
