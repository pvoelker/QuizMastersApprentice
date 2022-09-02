using System;
using QMA.ViewModel.Services;
using System.Windows;

namespace QuizMastersApprenticeApp.Services
{
    public class CloseWindowService : ICloseWindowService
    {
        private Window _owner;

        public CloseWindowService(Window owner)
        {
            _owner = owner;
        }

        public void CloseWindow()
        {
            _owner.Close();
        }
    }
}
