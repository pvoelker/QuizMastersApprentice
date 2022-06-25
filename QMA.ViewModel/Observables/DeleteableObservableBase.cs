using Microsoft.Toolkit.Mvvm.ComponentModel;
using QMA.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QMA.ViewModel.Observables
{
    public abstract class DeleteableObservableBase<T> : ObservableValidator where T : DeleteablePrimaryKeyBase
    {
        protected readonly T _model;

        public DeleteableObservableBase(bool persisted, T model, ICommand delete, ICommand restore, ICommand save)
        {
            if(model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if(delete == null)
            {
                throw new ArgumentNullException(nameof(delete));
            }

            this._model = model;

            Delete = delete;

            Restore = restore;

            Save = save;

            Persisted = persisted;

            ValidateAllProperties();
        }

        public T GetModel() { return _model; }

        public string PrimaryKey
        {
            get => _model.PrimaryKey;
        }

        public DateTimeOffset? Deleted
        {
            get => _model.Deleted;
            set
            {
                SetProperty(_model.Deleted, value, _model, (u, n) => u.Deleted = n);
                OnPropertyChanged(nameof(IsDeleted));
                OnPropertyChanged(nameof(IsNotDeleted));
            }
        }

        public bool IsDeleted
        {
            get => _model.Deleted != null;
        }

        public bool IsNotDeleted
        {
            get => _model.Deleted == null;
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

        public string Errors
        {
            get => string.Join(Environment.NewLine, GetErrors());
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName != nameof(Errors))
            {
                OnPropertyChanged(nameof(Errors));
            }
        }

        #region Commands

        public ICommand Delete { get; }

        public ICommand Restore { get; }

        public ICommand Save { get; }

        #endregion
    }
}
