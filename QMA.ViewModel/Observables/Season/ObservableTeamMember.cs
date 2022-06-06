using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QMA.ViewModel.Observables.Season
{
    public class ObservableTeamMember : ObservableValidator
    {
        public ObservableTeamMember(bool persisted, string quizzerId, string quizzerName)
        {
            if (quizzerId == null)
            {
                throw new ArgumentNullException(nameof(quizzerId));
            }
            if (quizzerName == null)
            {
                throw new ArgumentNullException(nameof(quizzerName));
            }

            _quizzerId = quizzerId;

            _quizzerName = quizzerName;

            Persisted = persisted;

            ValidateAllProperties();
        }

        private string _quizzerId;
        public string QuizzerId
        {
            get => _quizzerId;
        }

        private string _quizzerName;
        public string QuizzerName
        {
            get => _quizzerName;
        }

        private bool _isMember;
        public bool IsMember
        {
            get => _isMember;
            set
            {
                SetProperty(ref _isMember, value);
                OnPropertyChanged(nameof(IsNotMember));
                SetProperty(ref _persisted, false, nameof(Persisted));
                OnPropertyChanged(nameof(NotPersisted));
            }
        }
        public bool IsNotMember
        {
            get => !_isMember;
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
    }
}
