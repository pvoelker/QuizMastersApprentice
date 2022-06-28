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

        IEnumerable<Team> GetBySeasonId(string id);

        void Add(Team value);

        void Update(Team value);
    }
}
