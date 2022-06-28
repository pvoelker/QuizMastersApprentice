using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QMA.Model;

namespace QMA.DataAccess
{
    public interface ISeasonRepository
    {
        /// <summary>
        /// Get value by primary key
        /// </summary>
        /// <param name="key">Primary key</param>
        /// <returns>A quizzing season value</returns>
        SeasonInfo GetByKey(string key);

        /// <summary>
        /// Get all values
        /// </summary>
        /// <param name="includeDeleted">True to include deleted values</param>
        /// <returns>Quizzing season values. Empty collection if no values</returns>
        IEnumerable<SeasonInfo> GetAll(bool includeDeleted);

        void Add(SeasonInfo value);

        void Update(SeasonInfo value);
    }
}
