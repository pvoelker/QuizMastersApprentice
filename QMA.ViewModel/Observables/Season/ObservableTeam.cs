using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QMA.ViewModel.Observables.Season
{
    public class ObservableTeam : SoftDeletableObservableBase<Model.Season.Team>
    {
        public ObservableTeam(bool persisted, Model.Season.Team model, ICommand delete, ICommand restore, ICommand save)
            : base(persisted, model, delete, restore, save)
        {
        }

        public string SeasonId
        {
            get => _model.SeasonId;
        }

        [Required(ErrorMessage = "Name is Required")]
        [MinLength(2, ErrorMessage = "Name should be longer than one character")]
        public string Name
        {
            get => _model.Name;
            set => SetProperty(_model.Name, value, _model, (u, n) => u.Name = n, true);
        }

        public int? MaxPointValue
        {
            get => _model.MaxPointValue;
            set => SetProperty(_model.MaxPointValue, value, _model, (u, n) => u.MaxPointValue = n, true);
        }

        public string Notes
        {
            get => _model.Notes;
            set => SetProperty(_model.Notes, value, _model, (u, n) => u.Notes = n, true);
        }
    }
}
