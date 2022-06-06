using QMA.Model.Season;
using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.DataAccess
{
    public interface ITeamMemberRepository
    {
        /// <summary>
        /// Gets all teams
        /// </summary>
        /// <returns>A list of teams</returns>
        IEnumerable<TeamMember> GetAll();

        IEnumerable<TeamMember> GetByTeamId(string id);

        TeamMember GetByKey(string key);

        void Add(TeamMember value);

        void Delete(string id);
    }
}
