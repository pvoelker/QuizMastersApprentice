using CommunityToolkit.Mvvm.Input;
using QMA.DataAccess;
using QMA.Model;
using QMA.ViewModel.Observables;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QMA.ViewModel
{
    public class EditQuizzers : ItemsEditorObservable<Quizzer, ObservableQuizzer, IQuizzerRepository>
    {
        public EditQuizzers(IQuizzerRepository repository)
        {
            _repository = repository;

            Initialize = new RelayCommand(() =>
            {
                ShowBusy(() =>
                {
                    var items = _repository.GetAll(true);

                    foreach (var item in items)
                    {
                        var newItem = new ObservableQuizzer(
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
                var newItem = new ObservableQuizzer(
                    false,
                    new Quizzer
                    {
                        PrimaryKey = _repository.GetNewPrimaryKey(),
                        FirstName = null,
                        LastName = null,
                        ParentFullName = null
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
