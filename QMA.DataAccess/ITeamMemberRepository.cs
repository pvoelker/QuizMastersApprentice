using QMA.Model.Season;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QMA.DataAccess
{
    public interface ITeamMemberRepository : IBaseRepository<TeamMember>
    {
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
        /// Delete value by primary key
        /// </summary>
        /// <param name="key">Primary key</param>
        /// <returns>Task for asynchronous operation</returns>
        Task DeleteAsync(string key);
    }
}
