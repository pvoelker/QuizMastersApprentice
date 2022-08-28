using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QMA.Model.Season;

namespace QMA.DataAccess
{
    public interface IAssignedQuestionRepository : IBaseRepository<AssignedQuestion>
    {
        /// <summary>
        /// Get all values
        /// </summary>
        /// <returns>Assigned question values. Empty collection if no values</returns>
        IEnumerable<AssignedQuestion> GetAll();

        /// <summary>
        /// Get values by Question ID
        /// </summary>
        /// <param name="Id">Question ID</param>
        /// <returns>Assigned question values. Empty collection if no values</returns>
        IEnumerable<AssignedQuestion> GetByQuestionId(string Id);

        /// <summary>
        /// Get values by Team Member ID
        /// </summary>
        /// <param name="id">Team Member ID</param>
        /// <returns>Assigned question values. Empty collection if no values</returns>
        IEnumerable<AssignedQuestion> GetByTeamMemberId(string id);

        /// <summary>
        /// Delete value by primary key
        /// </summary>
        /// <param name="key">Primary key</param>
        /// <returns>Task for asynchronous operation</returns>
        Task DeleteAsync(string key);

        /// <summary>
        /// Delete multiple values by Team Member ID
        /// </summary>
        /// <param name="id">Team Member ID</param>
        /// <returns>Task for asynchronous operation</returns>
        Task DeleteAllByTeamMemberIdAsync(string id);
    }
}
