using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QMA.ViewModel.Observables
{
    public class ObservableQuestionSet : SoftDeletableObservableBase<Model.QuestionSet>
    {
        public ObservableQuestionSet(bool persisted, Model.QuestionSet model, ICommand delete, ICommand restore, ICommand save)
            : base(persisted, model, delete, restore, save)
        {
        }

        [Required(ErrorMessage = "Name is Required")]
        [MinLength(2, ErrorMessage = "Name should be longer than one character")]
        public string Name
        {
            get => _model.Name;
            set => SetProperty(_model.Name, value, _model, (u, n) => u.Name = n, true);
        }

        public string Notes
        {
            get => _model.Notes;
            set => SetProperty(_model.Notes, value, _model, (u, n) => u.Notes = n, true);
        }
    }
}
