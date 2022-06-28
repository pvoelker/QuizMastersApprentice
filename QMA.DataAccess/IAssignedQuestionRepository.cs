using System;
using System.Collections.Generic;
using QMA.Model.Season;

namespace QMA.DataAccess
{
    public interface IAssignedQuestionRepository
    {
        /// <summary>
        /// Get value by primary key
        /// </summary>
        /// <param name="key">Primary key</param>
        /// <returns>An assigned question value</returns>
        AssignedQuestion GetByKey(string key);

        /// <summary>
        /// Get all values
        /// </summary>
        /// <returns>Assigned question values. Empty collection if no values</returns>
        IEnumerable<AssignedQuestion> GetAll();

        IEnumerable<AssignedQuestion> GetByQuestionId(string Id);

        IEnumerable<AssignedQuestion> GetByTeamMemberId(string id);

        void Add(AssignedQuestion value);

        void Delete(string id);

        void DeleteAllByTeamMemberId(string id);
    }
}
