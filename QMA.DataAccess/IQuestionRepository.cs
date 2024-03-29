﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QMA.Model;

namespace QMA.DataAccess
{
    public interface IQuestionRepository : IUpdatableBaseRepository<Question>
    {
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
    }
}
