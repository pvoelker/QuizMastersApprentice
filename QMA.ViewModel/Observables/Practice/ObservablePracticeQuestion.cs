using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QMA.ViewModel.Observables.Practice
{
    public class ObservablePracticeQuestion : ObservableValidator
    {
        protected readonly Model.Question _model;

        public ObservablePracticeQuestion(Model.Question model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            this._model = model;
        }

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

        private int _usageCount = 0;
        public int UsageCount
        {
            get => _usageCount;
            set => SetProperty(ref _usageCount, value);
        }

        private bool _justLearned = false;
        public bool JustLearned
        {
            get => _justLearned;
            set => SetProperty(ref _justLearned, value);
        }
    }
}
