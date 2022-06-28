using QMA.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QMA.DataAccess
{
    public interface IQuestionSetRepository
    {
        /// <summary>
        /// Get value by primary key
        /// </summary>
        /// <param name="key">Primary key</param>
        /// <returns>A question set value</returns>
        QuestionSet GetByKey(string key);

        /// <summary>
        /// Get all values
        /// </summary>
        /// <returns>Question set values. Empty collection if no values</returns>
        IEnumerable<QuestionSet> GetAll();

        void Add(QuestionSet value);

        void Update(QuestionSet value);
    }
}
