using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QMA.ViewModel.Observables
{
    public class ObservableQuestion : ObservableBase<Model.Question>
    {
        public ObservableQuestion(bool persisted, Model.Question model, ICommand delete, ICommand restore, ICommand save)
            : base(persisted, model, delete, restore, save)
        {
        }

        public string QuestionSetId
        {
            get => _model.QuestionSetId;
        }

        [Required(ErrorMessage = "Question Number is Required")]
        public int Number
        {
            get => _model.Number;
            set => SetProperty(_model.Number, value, _model, (u, n) => u.Number = n, true);
        }

        [Required(ErrorMessage = "Question Text is Required")]
        [MinLength(2, ErrorMessage = "Question text should be longer than one character")]
        public string Text
        {
            get => _model.Text;
            set => SetProperty(_model.Text, value, _model, (u, n) => u.Text = n, true);
        }

        [Required(ErrorMessage = "Question Answer is Required")]
        [MinLength(2, ErrorMessage = "Question answer should be longer than one character")]
        public string Answer
        {
            get => _model.Answer;
            set => SetProperty(_model.Answer, value, _model, (u, n) => u.Answer = n, true);
        }

        public string Notes
        {
            get => _model.Notes;
            set => SetProperty(_model.Notes, value, _model, (u, n) => u.Notes = n, true);
        }

        public int Points
        {
            get => _model.Points;
            set => SetProperty(_model.Points, value, _model, (u, n) => u.Points = n, true);
        }
    }
}
