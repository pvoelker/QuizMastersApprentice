using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using QMA.DataAccess;
using QMA.Model;
using QMA.ViewModel.Observables;
using QMA.ViewModel.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QMA.ViewModel
{
    public class SelectDatabase : ObservableObject
    {
        private IOpenFileDialogService _openFileDialogService;

        private IAppDataRepository _repository;

        public SelectDatabase(IOpenFileDialogService openFileDialogService, IAppDataRepository repository)
        {
            _openFileDialogService = openFileDialogService;

            _repository = repository;

            Initialize = new RelayCommand(() =>
            {
                var types = DatabaseTypeHelpers.GetDatabaseTypes();

                var items = _repository.GetDatabases();
                foreach (var item in items)
                {
                    var newItem = new ObservableDatabaseInfo(item,
                        types.Single(x => x.Id == item.TypeId).Name,
                        (ObservableDatabaseInfo x) => { Items.Remove(x); });
                    Items.Add(newItem);
                }
            });

            Closing = new RelayCommand<CancelEventArgs>((CancelEventArgs e) =>
            {
                var items = Items.Select(x => x.GetModel()).ToList();
                _repository.SetDatabases(items);

                _repository.Dispose();
                _repository = null;
            });

            OldConnection = new RelayCommand(() =>
            {
                if (Selected != null)
                {
                    RepositoryFactory = new DataAccess.JsonFile.RepositoryFactory(Selected.Connection);
                }
                else
                {
                    RepositoryFactory = null;
                }
            });

            NewConnection = new RelayCommand(() =>
            {
                var fileName = _openFileDialogService.Show(
                    "Open file...",
                    false,
                    new List<OpenFileFilter> {
                        new OpenFileFilter("QMA Database File", "*.qma")
                    },
                    1);

                if (fileName != null)
                {
                    RepositoryFactory = new DataAccess.JsonFile.RepositoryFactory(fileName);

                    Items.Add(new ObservableDatabaseInfo(new DatabaseInfo {
                        TypeId = RepositoryFactory.Id,
                        Connection = fileName,
                    }, RepositoryFactory.Name,
                    (ObservableDatabaseInfo x) => { Items.Remove(x); }));

                    _repository.Save();
                }
                else
                {
                    RepositoryFactory = null;
                }
            });
        }

        public IRepositoryFactory RepositoryFactory { get; private set; }

        #region Data

        private bool _autoLoadDatabase = false;
        public bool AutoLoadDatabase
        {
            get => _autoLoadDatabase;
            set
            {
                SetProperty(ref _autoLoadDatabase, value);
            }
        }

        public ObservableCollection<ObservableDatabaseInfo> Items { get; } = new ObservableCollection<ObservableDatabaseInfo>();

        public ObservableDatabaseInfo Selected { get; set; }

        #endregion

        #region Commands

        public IRelayCommand Initialize { get; }

        public IRelayCommand<CancelEventArgs> Closing { get; }

        public IRelayCommand OldConnection { get; }

        public IRelayCommand NewConnection { get; }

        #endregion

        private void DeleteCommand()
        {
            Items.Remove(Selected);            
        }
    }
}
