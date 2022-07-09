using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using QMA.DataAccess;
using QMA.Model;
using QMA.ViewModel.Observables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMA.ViewModel
{
    /// <summary>
    /// Abstract class for implementing an editor for a list of data
    /// </summary>
    /// <typeparam name="T1">The data model to use</typeparam>
    /// <typeparam name="T2">The viewmodel to use</typeparam>
    /// <typeparam name="T3">The repository to use</typeparam>
    public abstract class ItemsEditorObservable<T1, T2, T3> : ObservableObject
        where T1 : SoftDeletablePrimaryKeyBase
        where T2 : SoftDeletableObservableBase<T1>
        where T3 : IBaseRepository <T1>
    {
        protected T3 _repository;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ItemsEditorObservable()
        {
            Closing = new RelayCommand<CancelEventArgs>((CancelEventArgs e) =>
            {
                if (Items.Any(x => x.HasErrors))
                {
                    e.Cancel = true;
                }
            });

            RowEditEnding = new AsyncRelayCommand<CancelEventArgs>(async (CancelEventArgs e) =>
            {
                if (Selected.HasErrors)
                {
                    e.Cancel = true;
                }
                else
                {
                    await SaveAsyncCommand();
                    Add.NotifyCanExecuteChanged();
                }
            });
        }

        /// <summary>
        /// The list of items being editted
        /// </summary>
        public ObservableCollection<T2> Items { get; } = new ObservableCollection<T2>();

        private T2 _selected = null;
        /// <summary>
        /// The currently selected item
        /// </summary>
        public T2 Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value, nameof(Selected));
        }

        private bool _isBusy = false;
        /// <summary>
        /// True when an operation is in progress, otherwise false
        /// </summary>
        /// <remarks>Other operations should NOT be started when this is true</remarks>
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value, nameof(IsBusy));
        }

        /// <summary>
        /// Wrapper for a method to set <see cref="IsBusy"/>
        /// </summary>
        /// <param name="action">The method to wrap</param>
        public void ShowBusy(Action action)
        {
            IsBusy = true;
            try
            {
                action();
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Wrapper for an async method to set <see cref="IsBusy"/>
        /// </summary>
        /// <param name="func">The async method to wrap</param>
        /// <returns>A task for asyncronous operation</returns>
        public async Task ShowBusyAsync(Func<Task> func)
        {
            IsBusy = true;
            try
            {
                await func();
            }
            finally
            {
                IsBusy = false;
            }
        }

        #region Commands

        /// <summary>
        /// Command for intialization
        /// </summary>
        public IRelayCommand Initialize { get; protected set; }

        /// <summary>
        /// Command for adding a new value
        /// </summary>
        public IRelayCommand Add { get; protected set; }

        /// <summary>
        /// Command for when editting on a data row has finished
        /// </summary>
        public IRelayCommand<CancelEventArgs> RowEditEnding { get; }

        /// <summary>
        ///  Command for closing
        /// </summary>
        public IRelayCommand<CancelEventArgs> Closing { get; }

        #endregion

        /// <summary>
        /// Command for soft deleting a value
        /// </summary>
        /// <returns>A task for asyncronous operation</returns>
        /// <exception cref="InvalidOperationException">The value is already soft deleted</exception>
        protected async Task SoftDeleteAsyncCommand()
        {
            if (Selected.Deleted != null)
            {
                throw new InvalidOperationException($"Value ({Selected.PrimaryKey}) is already soft deleted");
            }

            if (Selected.Persisted == true)
            {
                await ShowBusyAsync(async () =>
                {
                    Selected.Deleted = DateTimeOffset.UtcNow;
                    await _repository.UpdateAsync(Selected.GetModel());
                });
            }
            else
            {
                Items.Remove(Selected);
            }
        }

        /// <summary>
        /// Command for restoring (un soft deleting) a value
        /// </summary>
        /// <returns>A task for asyncronous operation</returns>
        /// <exception cref="InvalidOperationException">The value is not soft deleted</exception>
        protected async Task RestoreAsyncCommand()
        {
            if (Selected.Deleted == null)
            {
                throw new InvalidOperationException($"Value ({Selected.PrimaryKey}) is not already soft deleted");
            }

            await ShowBusyAsync(async () =>
            {
                Selected.Deleted = null;
                await _repository.UpdateAsync(Selected.GetModel());
            });
        }

        /// <summary>
        /// Command for saving a value (add or update)
        /// </summary>
        /// <returns>A task for asyncronous operation</returns>
        /// <exception cref="InvalidOperationException">No value is selected for a save to occur</exception>
        protected async Task SaveAsyncCommand()
        {
            if (Selected != null)
            {
                await ShowBusyAsync(async () =>
                {
                    if (Selected.Persisted)
                    {
                        await _repository.UpdateAsync(Selected.GetModel());
                    }
                    else
                    {
                        await _repository.AddAsync(Selected.GetModel());
                        Selected.Persisted = true;
                    }
                });
            }
            else
            {
                throw new InvalidOperationException("Save cannot occur with no selected value");
            }
        }
    }
}
