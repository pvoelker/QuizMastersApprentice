using System;
using QMA.ViewModel.Services;
using System.Windows;
using Microsoft.Win32;
using System.Collections.Generic;

namespace QuizMastersApprenticeApp.Services
{
    public class SaveFileDialogService : ISaveFileDialogService
    {
        private Window _owner;

        public SaveFileDialogService(Window owner)
        {
            _owner = owner;
        }

        public string Show(string title, IEnumerable<SaveFileFilter> filters, int selectedFilterIndex)
        {
            var dlg = new SaveFileDialog();
            dlg.CheckPathExists = true;
            dlg.Filter = String.Join("|", filters);
            dlg.FilterIndex = selectedFilterIndex;
            dlg.Title = title;
            if(dlg.ShowDialog(_owner) == true)
            {
                return dlg.FileName;
            }
            else
            {
                return null;
            }
        }
    }
}
