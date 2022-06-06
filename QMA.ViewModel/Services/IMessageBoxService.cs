using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.ViewModel.Services
{
    public interface IMessageBoxService
    {
        void ShowError(string message);

        bool PromptToContinue(string messsage);
    }
}
