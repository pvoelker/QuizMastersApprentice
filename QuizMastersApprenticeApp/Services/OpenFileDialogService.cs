using System;
using QMA.ViewModel.Services;
using System.Windows;
using Microsoft.Win32;
using System.Collections.Generic;

namespace QuizMastersApprenticeApp.Services
{
    public class OpenFileDialogService : IOpenFileDialogService
    {
        private Window _owner;

        public OpenFileDialogService(Window owner)
        {
            _owner = owner;
        }

        public string Show(string title, bool checkFileExists, IEnumerable<OpenFileFilter> filters, int selectedFilterIndex)
        {
            var dlg = new OpenFileDialog();
            dlg.CheckPathExists = true;
            dlg.Filter = String.Join("|", filters);
            dlg.FilterIndex = selectedFilterIndex;
            dlg.Title = title;
            dlg.CheckPathExists = true;
            dlg.CheckFileExists = checkFileExists;
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
