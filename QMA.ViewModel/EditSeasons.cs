﻿using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using QMA.DataAccess;
using QMA.Helpers;
using QMA.Model;
using QMA.ViewModel.Observables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QMA.ViewModel
{
    public class EditSeasons : ObservableObject
    {
        private ISeasonRepository _repository;

        private IQuestionSetRepository _questionSetRepository;

        public EditSeasons(ISeasonRepository repository, IQuestionSetRepository questionSetRepository)
        {
            _repository = repository;

            _questionSetRepository = questionSetRepository;

            Initialize = new RelayCommand(() =>
            {
                var questionSets = _questionSetRepository.GetAll();

                foreach(var questionSet in questionSets)
                {
                    QuestionSets.Add(questionSet);
                }

                var items = _repository.GetAll(true);

                foreach(var item in items)
                {
                    var newItem = new ObservableSeason(
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
                var newItem = new ObservableSeason(
                    false,
                    new SeasonInfo
                    {
                        PrimaryKey = Guid.NewGuid().ToString(),
                        Name = $"Season {Items.Count + 1}",
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

        public ObservableCollection<ObservableSeason> Items { get; } = new ObservableCollection<ObservableSeason>();

        public ObservableSeason Selected { get; set; }

        public ObservableCollection<QuestionSet> QuestionSets { get; } = new ObservableCollection<QuestionSet>();

        #region Commands

        public IRelayCommand Initialize { get; }

        public IRelayCommand Add { get; }

        public IRelayCommand<CancelEventArgs> Closing { get; }

        // https://docs.microsoft.com/en-us/windows/communitytoolkit/controls/datagrid_guidance/editing_inputvalidation
        public IRelayCommand<CancelEventArgs> RowEditEnding { get; }

        #endregion

        private void DeleteCommand()
        {
            if (Selected.Deleted != null)
            {
                throw new InvalidOperationException($"Season ({Selected.PrimaryKey}) is already deleted");
            }

            if (Selected.Persisted == true)
            {
                Selected.Deleted = DateTimeOffset.UtcNow;
                AsyncHelper.RunSync(() => _repository.UpdateAsync(Selected.GetModel()));
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
                throw new InvalidOperationException($"Season ({Selected.PrimaryKey}) is not deleted");
            }

            Selected.Deleted = null;
            AsyncHelper.RunSync(() => _repository.UpdateAsync(Selected.GetModel()));
        }

        private void SaveCommand()
        {
            if(Selected != null)
            {
                if (Selected.Persisted)
                {
                    AsyncHelper.RunSync(() => _repository.UpdateAsync(Selected.GetModel()));
                }
                else
                {
                    AsyncHelper.RunSync(() => _repository.AddAsync(Selected.GetModel()));
                    Selected.Persisted = true;
                }
            }
            else
            {
                throw new InvalidOperationException("Save cannot occur with no selected season");
            }
        }
    }
}
