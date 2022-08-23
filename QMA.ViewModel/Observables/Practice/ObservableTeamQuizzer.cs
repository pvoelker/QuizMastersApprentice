using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QMA.ViewModel.Observables.Practice
{
    public class ObservableTeamQuizzer : ObservableValidator
    {
        public ObservableTeamQuizzer(
            string primaryKey,
            string quizzerId,
            string teamName,
            int? teamMaxPointValue,
            string firstName,
            string lastName)
        {
            _primaryKey = primaryKey;
            _quizzerId = quizzerId;
            _teamName = teamName;
            _teamMaxPointValue = teamMaxPointValue;
            _firstName = firstName;
            _lastName = lastName;
        }

        private string _primaryKey;
        /// <summary>
        /// Team Member ID
        /// </summary>
        public string PrimaryKey
        {
            get => _primaryKey;
        }

        private string _quizzerId;
        public string QuizzerId
        {
            get => _quizzerId;
        }

        private string _teamName;
        public string TeamName
        {
            get => _teamName;
        }

        private int? _teamMaxPointValue;
        public int? TeamMaxPointValue
        {
            get => _teamMaxPointValue;
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                SetProperty(ref _isSelected, value);
                OnPropertyChanged(nameof(IsNotSelected));
            }
        }
        public bool IsNotSelected
        {
            get => !_isSelected;
        }

        private bool _isDuplicate;
        public bool IsDuplicate
        {
            get => _isDuplicate;
            set
            {
                SetProperty(ref _isDuplicate, value);
                OnPropertyChanged(nameof(IsNotDuplicate));
            }
        }
        public bool IsNotDuplicate
        {
            get => !_isDuplicate;
        }
    }
}
