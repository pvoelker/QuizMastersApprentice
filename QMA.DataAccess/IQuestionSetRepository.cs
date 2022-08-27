using QMA.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QMA.DataAccess
{
    public interface IQuestionSetRepository : IUpdatableBaseRepository<QuestionSet>
    {

        /// <summary>
        /// Get all values
        /// </summary>
        /// <returns>Question set values. Empty collection if no values</returns>
        IEnumerable<QuestionSet> GetAll();
    }
}
