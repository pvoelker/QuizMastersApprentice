using QMA.ViewModel.Practice;
using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.ViewModel.Services
{
    public interface IRunPracticeService
    {
        void Start(ConfigurePractice vm);
    }
}
