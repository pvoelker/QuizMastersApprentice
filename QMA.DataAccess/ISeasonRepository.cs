using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QMA.Model;
using QMA.Model.Season;

namespace QMA.DataAccess
{
    public interface ISeasonRepository : IUpdatableBaseRepository<SeasonInfo>
    {
        /// <summary>
        /// Get all values
        /// </summary>
        /// <param name="includeDeleted">True to include deleted values</param>
        /// <returns>Quizzing season values. Empty collection if no values</returns>
        IEnumerable<SeasonInfo> GetAll(bool includeDeleted);
    }
}
