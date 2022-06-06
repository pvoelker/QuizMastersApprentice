using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QMA.ViewModel.Observables
{
    public class ObservableQuizzer : ObservableBase<Model.Quizzer>
    {
        public ObservableQuizzer(bool persisted, Model.Quizzer model, ICommand delete, ICommand restore, ICommand save)
            : base(persisted, model, delete, restore, save)
        {
        }

        [Required(ErrorMessage = "First Name is Required")]
        [MinLength(2, ErrorMessage = "First Name should be longer than one character")]
        public string FirstName
        {
            get => _model.FirstName;
            set => SetProperty(_model.FirstName, value, _model, (u, n) => u.FirstName = n, true);
        }

        [Required(ErrorMessage = "Last Name is Required")]
        [MinLength(2, ErrorMessage = "Last Name should be longer than one character")]
        public string LastName
        {
            get => _model.LastName;
            set => SetProperty(_model.LastName, value, _model, (u, n) => u.LastName = n, true);
        }

        [Required(ErrorMessage = "Parent's Name is Required")]
        [MinLength(2, ErrorMessage = "Parent's Name should be longer than one character")]
        public string ParentFullName
        {
            get => _model.ParentFullName;
            set => SetProperty(_model.ParentFullName, value, _model, (u, n) => u.ParentFullName = n, true);
        }

        [RegularExpression(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$")]
        public string ParentEmail
        {
            get => _model.ParentEmail;
            set => SetProperty(_model.ParentEmail, value, _model, (u, n) => u.ParentEmail = n, true);
        }

        public string Notes
        {
            get => _model.Notes;
            set => SetProperty(_model.Notes, value, _model, (u, n) => u.Notes = n, true);
        }
    }
}
