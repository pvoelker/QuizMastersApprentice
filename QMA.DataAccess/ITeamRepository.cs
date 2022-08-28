using QMA.Model.Season;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QMA.DataAccess
{
    public interface ITeamRepository : IUpdatableBaseRepository<Team>
    {
        /// <summary>
        /// Get all values
        /// </summary>
        /// <returns>Team values. Empty collection if no values</returns>
        IEnumerable<Team> GetAll();

        /// <summary>
        /// Get values by Season ID
        /// </summary>
        /// <param name="id">Season ID</param>
        /// <returns>Team values. Empty collection if no values</returns>
        IEnumerable<Team> GetBySeasonId(string id);
    }
}
