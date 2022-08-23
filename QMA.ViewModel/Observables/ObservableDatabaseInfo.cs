using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QMA.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QMA.ViewModel.Observables
{
    public class ObservableDatabaseInfo : ObservableValidator
    {
        protected readonly DatabaseInfo _model;

        public ObservableDatabaseInfo(DatabaseInfo model, string typeName, Action<ObservableDatabaseInfo> parentDelete)
        {
            if(model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (typeName == null)
            {
                throw new ArgumentNullException(nameof(typeName));
            }
            if (parentDelete == null)
            {
                throw new ArgumentNullException(nameof(parentDelete));
            }

            _model = model;
            _typeName = typeName;

            Delete = new RelayCommand(() => { parentDelete(this); });

            ValidateAllProperties();
        }

        public DatabaseInfo GetModel() { return _model; }

        private string _typeName;
        [Required(ErrorMessage = "Connection is Required")]
        public string TypeName
        {
            get => _typeName;
            set => SetProperty(ref _typeName, value);
        }

        [Required(ErrorMessage = "Connection is Required")]
        public string Connection
        {
            get => _model.Connection;
            set => SetProperty(_model.Connection, value, _model, (u, n) => u.Connection = n, true);
        }

        #region Commands

        public IRelayCommand Delete { get; }

        #endregion
    }
}
