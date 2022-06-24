using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QMA.ViewModel.Observables.Practice
{
    public class ObservablePracticeQuizzer : ObservableValidator
    {
        protected readonly Model.Quizzer _model;

        public ObservablePracticeQuizzer(string teamMemberId, Model.Quizzer model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _teamMemberId = teamMemberId;

            _model = model;
        }

        private string _teamMemberId;
        public string TeamMemberId
        {
            get => _teamMemberId;
        }

        public string Name
        {
            get => $"{_model.FirstName} {_model.LastName}";
        }

        public string FirstName
        {
            get => _model.FirstName;
        }

        public string LastName
        {
            get => _model.LastName;
        }

        public string ParentFullName
        {
            get => _model.ParentFullName;
        }

        public string ParentEmail
        {
            get => _model.ParentEmail;
        }

        public string Notes
        {
            get => _model.Notes;
        }

        public ObservableCollection<ObservablePracticeQuestion> CorrectQuestions { get; } = new ObservableCollection<ObservablePracticeQuestion>();

        public ObservableCollection<ObservablePracticeQuestion> WrongQuestions { get; } = new ObservableCollection<ObservablePracticeQuestion>();

        private bool _assignQuestion = false;
        public bool AssignQuestion
        {
            get => _assignQuestion;
            set => SetProperty(ref _assignQuestion, value);
        }

        private bool _reportSent = false;
        public bool ReportSent
        {
            get => _reportSent;
            set => SetProperty(ref _reportSent, value);
        }

        private string _reportError = null;
        public string ReportError
        {
            get => _reportError;
            set => SetProperty(ref _reportError, value);
        }
    }
}
