using Microsoft.Toolkit.Mvvm.Input;
using QMA.DataAccess;
using QMA.Model;
using QMA.ViewModel.Observables;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QMA.ViewModel
{
    public class EditSeasons : ItemsEditorObservable<SeasonInfo, ObservableSeason, ISeasonRepository>
    {
        private IQuestionSetRepository _questionSetRepository;

        public EditSeasons(ISeasonRepository repository, IQuestionSetRepository questionSetRepository)
        {
            _repository = repository;

            _questionSetRepository = questionSetRepository;

            Initialize = new RelayCommand(() =>
            {
                ShowBusy(() =>
                {
                    var questionSets = _questionSetRepository.GetAll();

                    foreach (var questionSet in questionSets)
                    {
                        QuestionSets.Add(questionSet);
                    }

                    var items = _repository.GetAll(true);

                    foreach (var item in items)
                    {
                        var newItem = new ObservableSeason(
                            true,
                            item,
                            new AsyncRelayCommand(SoftDeleteAsyncCommand),
                            new AsyncRelayCommand(RestoreAsyncCommand),
                            new AsyncRelayCommand(SaveAsyncCommand)
                        );
                        Items.Add(newItem);
                    }
                });
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
                    new AsyncRelayCommand(SoftDeleteAsyncCommand),
                    new AsyncRelayCommand(RestoreAsyncCommand),
                    new AsyncRelayCommand(SaveAsyncCommand)
                );
                Items.Add(newItem);
                Selected = newItem;
                Add.NotifyCanExecuteChanged();
            },
            () => !Items.Any(x => x.HasErrors));
        }

        public ObservableCollection<QuestionSet> QuestionSets { get; } = new ObservableCollection<QuestionSet>();

        #region Commands

        #endregion
    }
}
