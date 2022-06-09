﻿using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using QMA.DataAccess;
using QMA.Model;
using QMA.ViewModel.Observables;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace QMA.ViewModel
{
    public class EditQuizzers : ObservableObject
    {
        private IQuizzerRepository _repository;

        public EditQuizzers(IQuizzerRepository repository)
        {
            _repository = repository;

            Initialize = new RelayCommand(() =>
            {
                var items = _repository.GetAll(true);

                foreach (var item in items)
                {
                    var newItem = new ObservableQuizzer(
                        true,
                        item,
                        new RelayCommand(DeleteCommand),
                        new RelayCommand(RestoreCommand),
                        new RelayCommand(SaveCommand)
                    );
                    Items.Add(newItem);
                }

                Add.NotifyCanExecuteChanged();
            });

            Add = new RelayCommand(() =>
            {
                var newItem = new ObservableQuizzer(
                    false,
                    new Quizzer
                    {
                        PrimaryKey = Guid.NewGuid().ToString(),
                        FirstName = null,
                        LastName = null,
                        ParentFullName = null
                    },
                    new RelayCommand(DeleteCommand),
                    new RelayCommand(RestoreCommand),
                    new RelayCommand(SaveCommand)
                );
                Items.Add(newItem);
                Selected = newItem;
                Add.NotifyCanExecuteChanged();
            },
            () => !Items.Any(x => x.HasErrors));

            //https://social.msdn.microsoft.com/Forums/en-US/72169f1b-1d2a-42f6-9918-63a45975e982/mvvm-close-window-event?forum=wpf
            Closing = new RelayCommand<CancelEventArgs>((CancelEventArgs e) =>
            {
                if(Items.Any(x => x.HasErrors))
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
                    SaveCommand();
                    Add.NotifyCanExecuteChanged();
                }
            });
        }

        public ObservableCollection<ObservableQuizzer> Items { get; } = new ObservableCollection<ObservableQuizzer>();

        public ObservableQuizzer Selected { get; set; }

        #region Commands

        public IRelayCommand Initialize { get; }

        public IRelayCommand Add { get; }

        public IRelayCommand<CancelEventArgs> Closing { get; }

        // https://docs.microsoft.com/en-us/windows/communitytoolkit/controls/datagrid_guidance/editing_inputvalidation
        public IRelayCommand<CancelEventArgs> RowEditEnding { get; }       

        #endregion

        private void DeleteCommand()
        {
            if(Selected.Deleted != null)
            {
                throw new InvalidOperationException($"Quizzer ({Selected.PrimaryKey}) is already deleted");
            }

            if (Selected.Persisted == true)
            {
                Selected.Deleted = DateTimeOffset.UtcNow;
                _repository.Update(Selected.GetModel());
            }
            else
            {
                Items.Remove(Selected);
            }
            Add.NotifyCanExecuteChanged();
        }

        private void RestoreCommand()
        {
            if (Selected.Deleted == null)
            {
                throw new InvalidOperationException($"Quizzer ({Selected.PrimaryKey}) is not deleted");
            }

            Selected.Deleted = null;
            _repository.Update(Selected.GetModel());
        }

        private void SaveCommand()
        {
            if(Selected != null)
            {
                if (Selected.Persisted)
                {
                    _repository.Update(Selected.GetModel());
                }
                else
                {
                    _repository.Add(Selected.GetModel());
                    Selected.Persisted = true;
                }
            }
            else
            {
                throw new InvalidOperationException("Save cannot occur with no selected quizzer");
            }
        }
    }
}
