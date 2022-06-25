using QMA.ViewModel.Observables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuizMastersApprenticeApp.Controls
{
    public partial class AssignQuestions : UserControl
    {
        public AssignQuestions()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty QuestionsProperty = DependencyProperty.Register(nameof(Questions), typeof(ObservableCollection<ObservableAssignedQuestion>), typeof(AssignQuestions),
            new FrameworkPropertyMetadata(new ObservableCollection<ObservableAssignedQuestion>(), FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnQuestionsChanged)));

        private static void OnQuestionsChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            var assignQuestionsObj = depObj as AssignQuestions;

            if (assignQuestionsObj != null)
            {
                assignQuestionsObj.QuestionsView = CollectionViewSource.GetDefaultView(assignQuestionsObj.Questions);
                assignQuestionsObj.QuestionsView.SortDescriptions.Add(new SortDescription(nameof(ObservableAssignedQuestion.Number), ListSortDirection.Ascending));
                assignQuestionsObj.QuestionsView.SortDescriptions.Add(new SortDescription(nameof(ObservableAssignedQuestion.Text), ListSortDirection.Ascending));
                assignQuestionsObj.QuestionsView.Refresh();
                assignQuestionsObj.UpdateFilter();
            }
        }

        public ObservableCollection<ObservableAssignedQuestion> Questions
        {
            get => (ObservableCollection<ObservableAssignedQuestion>)GetValue(QuestionsProperty);
            set => SetValue(QuestionsProperty, value);
        }

        private static readonly DependencyProperty QuestionsViewProperty = DependencyProperty.Register(nameof(QuestionsView), typeof(ICollectionView), typeof(AssignQuestions),
            new PropertyMetadata(null));

        public ICollectionView QuestionsView
        {
            get => (ICollectionView) GetValue(QuestionsViewProperty);
            set => SetValue(QuestionsViewProperty, value);
        }

        public static readonly DependencyProperty AssignedQuestionsProperty = DependencyProperty.Register(nameof(AssignedQuestions), typeof(ObservableCollection<ObservableAssignedQuestion>), typeof(AssignQuestions),
            new FrameworkPropertyMetadata(new ObservableCollection<ObservableAssignedQuestion>(), FrameworkPropertyMetadataOptions.AffectsRender));

        public ObservableCollection<ObservableAssignedQuestion> AssignedQuestions
        {
            get => (ObservableCollection<ObservableAssignedQuestion>)GetValue(AssignedQuestionsProperty);
            set => SetValue(AssignedQuestionsProperty, value);
        }

        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFilter();
        }

        private void UpdateFilter()
        {
            if (string.IsNullOrWhiteSpace(_filter.Text))
            {
                QuestionsView.Filter = o => AssignedQuestions == null ? true : !AssignedQuestions.Any(x => x.PrimaryKey == ((ObservableAssignedQuestion)o).PrimaryKey);
            }
            else
            {
                QuestionsView.Filter = o => AssignedQuestions == null ? true : !AssignedQuestions.Any(x => x.PrimaryKey == ((ObservableAssignedQuestion)o).PrimaryKey) &&
                    ((ObservableAssignedQuestion)o).Text.Contains(_filter.Text, StringComparison.CurrentCultureIgnoreCase);
            }

            var count = QuestionsView.Cast<ObservableAssignedQuestion>().Count();
            if(count == 0)
            {
                _questions.Visibility = Visibility.Collapsed;
                if(string.IsNullOrWhiteSpace(_filter.Text))
                {
                    _noQuestionsText.Text = "No questions to add...";
                }
                else
                {
                    _noQuestionsText.Text = "No questions to add, clear filter...";
                }
                _noQuestionsText.Visibility = Visibility.Visible;
            }
            else
            {
                _questions.Visibility = Visibility.Visible;
                _noQuestionsText.Visibility = Visibility.Collapsed;
            }
        }

        private void Questions_DropDownClosed(object sender, EventArgs e)
        {
            var question = _questions.SelectedValue as ObservableAssignedQuestion;

            if (question != null)
            {
                AssignedQuestions.Add(question);
                question.Persisted = false;

                UpdateFilter();
            }
        }
    }
}
