using Microsoft.Toolkit.Mvvm.ComponentModel;
using Newtonsoft.Json.Linq;
using QMA.ViewModel.Observables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace QMA.ViewModel
{
    public class ItemsEditorObservable<T> : ObservableObject
        where T : class
    {
        public ObservableCollection<T> Items { get; } = new ObservableCollection<T>();

        private T _selected = null;
        public T Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value, nameof(Selected));
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value, nameof(IsBusy));
        }
    }
}
