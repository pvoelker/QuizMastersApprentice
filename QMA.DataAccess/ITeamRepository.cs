using QMA.Model.Season;
using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.DataAccess
{
    public interface ITeamRepository
    {
        /// <summary>
        /// Gets all teams
        /// </summary>
        /// <returns>A list of teams</returns>
        IEnumerable<Team> GetAll();

        IEnumerable<Team> GetBySeasonId(string id);

        Team GetByKey(string key);

        void Add(Team value);

        void Update(Team value);
    }
}
