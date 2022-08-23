using CommunityToolkit.Mvvm.Input;
using QMA.DataAccess;
using QMA.Model;
using QMA.ViewModel.Observables;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QMA.ViewModel
{
    public class EditQuestionSets : ItemsEditorObservable<QuestionSet, ObservableQuestionSet, IQuestionSetRepository>
    {
        public EditQuestionSets(IQuestionSetRepository repository)
        {
            _repository = repository;

            Initialize = new RelayCommand(() =>
            {
                ShowBusy(() =>
                {
                    var items = _repository.GetAll();

                    foreach (var item in items)
                    {
                        var newItem = new ObservableQuestionSet(
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
                var newItem = new ObservableQuestionSet(
                    false,
                    new QuestionSet
                    {
                        PrimaryKey = Guid.NewGuid().ToString(),
                        Name = $"Question Set {Items.Count + 1}",
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

        #region Commands

        #endregion
    }
}
