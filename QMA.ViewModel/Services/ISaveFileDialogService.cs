using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.ViewModel.Services
{
    public class SaveFileFilter
    {
        public SaveFileFilter(string desription, string filter)
        {
            Descrition = desription;
            Filter = filter;
        }

        public string Descrition { get; }
        public string Filter { get; }

        public override string ToString()
        {
            return $"{Descrition}|{Filter}";
        }
    }

    public interface ISaveFileDialogService
    {
        string Show(string title, IEnumerable<SaveFileFilter> filters, int selectedFilterIndex);
    }
}
