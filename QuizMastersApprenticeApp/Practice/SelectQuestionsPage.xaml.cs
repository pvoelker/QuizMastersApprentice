using QMA.DataAccess;
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

        public void Initialize(IQuestionRepository repository)
        {
            _directTextImport.Initialize(repository);
        }
    }
}
