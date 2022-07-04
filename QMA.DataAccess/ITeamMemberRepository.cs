using QMA.Model.Season;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QMA.DataAccess
{
    public interface ITeamMemberRepository
    {
        /// <summary>
        /// Get value by primary key
        /// </summary>
        /// <param name="key">Primary key</param>
        /// <returns>A team member value</returns>
        TeamMember GetByKey(string key);

        /// <summary>
        /// Get all values
        /// </summary>
        /// <returns>Team member values. Empty collection if no values</returns>
        IEnumerable<TeamMember> GetAll();

        /// <summary>
        /// Get values by Team ID
        /// </summary>
        /// <param name="id">Team ID</param>
        /// <returns>Team member values. Empty collection if no values</returns>
        IEnumerable<TeamMember> GetByTeamId(string id);

        /// <summary>
        /// Add a new value, cannot update an existing value
        /// </summary>
        /// <param name="value">The value to add</param>
        /// <returns>Task for asynchronous operation</returns>
        Task AddAsync(TeamMember value);

        /// <summary>
        /// Delete value by primary key
        /// </summary>
        /// <param name="key">Primary key</param>
        /// <returns>Task for asynchronous operation</returns>
        Task DeleteAsync(string key);
    }
}
