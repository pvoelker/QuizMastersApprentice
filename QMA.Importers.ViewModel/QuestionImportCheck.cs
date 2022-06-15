using QMA.DataAccess;
using QMA.ViewModel.Observables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QMA.Importers.ViewModel
{
    public class QuestionImportCheck
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public QuestionImportCheck()
        {
        }

        /// <inheritdoc />
        public void CheckAgainstQuestionSet(
            IEnumerable<ObservableImportQuestion> importedQuestions,
            IQuestionRepository repository,
            string questionSetId)
        {
            foreach (var item in importedQuestions)
            {
                var existing = repository.GetByQuestionNumber(questionSetId, item.Number, false);
                if (existing.Count() == 1)
                {
                    item.AlreadyExists = true;

                    var found = existing.First();
                    if (found.Text != item.Text)
                    {
                        item.ParseError = "Question Mismatch";
                    }
                    else if (found.Answer != item.Answer)
                    {
                        item.ParseError = "Answer Mismatch";
                    }
                    else if (found.Points != item.Points)
                    {
                        item.ParseError = "Points Mismatch";
                    }
                }
                else if (existing.Count() > 1)
                {
                    item.AlreadyExists = true;

                    item.ParseError = "Multiples Found";
                }
            }
        }
    }
}
