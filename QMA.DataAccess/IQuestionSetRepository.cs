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

        /// <summary>
        /// Add a new value, cannot update an existing value
        /// </summary>
        /// <param name="value">The value to add</param>
        void Add(QuestionSet value);

        /// <summary>
        /// Update an existing value, cannot add a new value
        /// </summary>
        /// <param name="value">The value to update</param>
        void Update(QuestionSet value);
    }
}
