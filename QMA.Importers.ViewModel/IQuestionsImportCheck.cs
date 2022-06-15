using QMA.DataAccess;
using QMA.ViewModel.Observables;
using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.Importers.ViewModel
{
    public interface IQuestionsImportCheck
    {
        /// <summary>
        /// Check imported questions against existing questions in a question set
        /// </summary>
        /// <param name="importedQuestions">The imported questions to check</param>
        /// <param name="repository">Questions respository to check against</param>
        /// <param name="questionSetId">Question Set ID to check against</param>
        void CheckAgainstQuestionSet(
            IEnumerable<ObservableImportQuestion> importedQuestions,
                    IQuestionRepository repository,
                    string questionSetId);
    }
}
