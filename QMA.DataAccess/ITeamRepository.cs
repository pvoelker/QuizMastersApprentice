using QMA.Model.Season;
using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.DataAccess
{
    public interface ITeamRepository
    {
        /// <summary>
        /// Get value by primary key
        /// </summary>
        /// <param name="key">Primary key</param>
        /// <returns>A team value</returns>
        Team GetByKey(string key);

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

        /// <summary>
        /// Add a new value, cannot update an existing value
        /// </summary>
        /// <param name="value">The value to add</param>
        void Add(Team value);

        /// <summary>
        /// Update an existing value, cannot add a new value
        /// </summary>
        /// <param name="value">The value to update</param>
        void Update(Team value);
    }
}
