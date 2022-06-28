using System;
using System.Collections.Generic;
using QMA.Model;

namespace QMA.DataAccess
{
    public interface IQuestionRepository
    {
        /// <summary>
        /// Get value by primary key
        /// </summary>
        /// <param name="key">Primary key</param>
        /// <returns>A question value</returns>
        Question GetByKey(string key);

        /// <summary>
        /// Get all values
        /// </summary>
        /// <returns>Question values. Empty collection if no values</returns>
        IEnumerable<Question> GetAll();

        /// <summary>
        /// Get values by Questions Set ID and Question Number
        /// </summary>
        /// <param name="questionsSetId">Question Set ID</param>
        /// <param name="questionNumber">Question Number for the Question Set ID</param>
        /// <param name = "includeDeleted">True to include deleted values</param>
        /// <returns>Question values. Empty collection if no values</returns>
        IEnumerable<Question> GetByQuestionNumber(string questionsSetId, int questionNumber, bool includeDeleted);

        /// <summary>
        /// Get values by Questions Set ID
        /// </summary>
        /// <param name="questionsSetId">Question Set ID</param>
        /// <param name = "includeDeleted">True to include deleted values</param>
        /// <returns>Question values. Empty collection if no values</returns>
        IEnumerable<Question> GetByQuestionSetId(string id, bool includeDeleted);

        /// <summary>
        /// Returns the number of questions by Question Set ID and Max Point Value (optional) 
        /// </summary>
        /// <param name="questionSetId">Questions Set ID</param>
        /// <param name="maxQuestionPointValue">Max Point Value. Null if no limit applied</param>
        /// <param name="includeDeleted">True to include deleted values in the count</param>
        /// <returns>The number of questions found</returns>
        int CountByQuestionSetId(string questionSetId, int? maxQuestionPointValue, bool includeDeleted);

        /// <summary>
        /// Add a new value, cannot update an existing value
        /// </summary>
        /// <param name="value">The value to update</param>
        void Add(Question value);

        void Update(Question value);
    }
}
