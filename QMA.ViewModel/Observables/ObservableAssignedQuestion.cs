using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QMA.ViewModel.Observables
{
    public class ObservableAssignedQuestion : ObservableValidator
    {
        protected readonly Model.Question _model;

        public ObservableAssignedQuestion(bool persisted, Model.Question model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _model = model;

            Persisted = persisted;

            ValidateAllProperties();
        }

        public Model.Question GetModel() { return _model; }

        public string PrimaryKey
        {
            get => _model.PrimaryKey;
        }

        public string QuestionSetId
        {
            get => _model.QuestionSetId;
        }

        public int Number
        {
            get => _model.Number;
        }

        public string Text
        {
            get => _model.Text;
        }

        public string Answer
        {
            get => _model.Answer;
        }

        public string Notes
        {
            get => _model.Notes;
        }

        public int Points
        {
            get => _model.Points;
        }

        private bool _persisted = false;
        public bool Persisted
        {
            get => _persisted;
            set
            {
                SetProperty(ref _persisted, value, nameof(Persisted));
                OnPropertyChanged(nameof(NotPersisted));
            }
        }
        public bool NotPersisted
        {
            get => !_persisted;
        }
    }
}
