using System;
using QMA.ViewModel.Services;
using System.Windows;

namespace QuizMastersApprenticeApp.Services
{
    public class MessageBoxService : IMessageBoxService
    {
        private Window _owner;

        public MessageBoxService(Window owner)
        {
            _owner = owner;
        }

        public void ShowError(string message)
        {
            MessageBox.Show(_owner, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public bool PromptToContinue(string message)
        {
            return MessageBox.Show(_owner, message, "Continue?", MessageBoxButton.YesNo, MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes;
        }
    }
}
