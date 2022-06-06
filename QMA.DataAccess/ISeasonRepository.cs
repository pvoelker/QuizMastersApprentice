using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QMA.Model;

namespace QMA.DataAccess
{
    public interface ISeasonRepository
    {
        /// <summary>
        /// Gets all quizzing season information
        /// </summary>
        /// <returns>A list of seasons</returns>
        IEnumerable<SeasonInfo> GetAll(bool includeDeleted);

        SeasonInfo GetByKey(string key);

        void Add(SeasonInfo value);

        void Update(SeasonInfo value);
    }
}
