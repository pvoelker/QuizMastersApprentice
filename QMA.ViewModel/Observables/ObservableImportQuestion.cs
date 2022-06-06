using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.ViewModel.Observables
{
    public class ObservableImportQuestion : ObservableValidator
    {
        protected readonly Model.ImportQuestion _model;

        public ObservableImportQuestion(Model.ImportQuestion model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            this._model = model;
        }

        public int Number
        {
            get => _model.Number;
            set => SetProperty(_model.Number, value, _model, (u, n) => u.Number = n, true);
        }

        public string Text
        {
            get => _model.Text;
            set => SetProperty(_model.Text, value, _model, (u, n) => u.Text = n, true);
        }

        public string Answer
        {
            get => _model.Answer;
            set => SetProperty(_model.Answer, value, _model, (u, n) => u.Answer = n, true);
        }

        public int Points
        {
            get => _model.Points;
            set => SetProperty(_model.Points, value, _model, (u, n) => u.Points = n, true);
        }

        private string _parseError = null;
        public string ParseError
        {
            get => _parseError;
            set
            {
                SetProperty(ref _parseError, value);
                OnPropertyChanged(nameof(HasParseError));
            }
        }
        public bool HasParseError
        {
            get => _parseError != null;
        }
    }
}
