using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.ViewModel.Services
{
    public class OpenFileFilter
    {
        public OpenFileFilter(string desription, string filter)
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

    public interface IOpenFileDialogService
    {
        string Show(string title, bool checkFileExists, IEnumerable<OpenFileFilter> filters, int selectedFilterIndex);
    }
}
