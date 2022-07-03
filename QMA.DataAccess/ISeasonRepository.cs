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

        /// <summary>
        /// Add a new value, cannot update an existing value
        /// </summary>
        /// <param name="value">The value to add</param>
        /// <returns>Task for asynchronous operation</returns>
        Task AddAsync(SeasonInfo value);

        /// <summary>
        /// Update an existing value, cannot add a new value
        /// </summary>
        /// <param name="value">The value to update</param>
        /// <returns>Task for asynchronous operation</returns>
        Task UpdateAsync(SeasonInfo value);
    }
}
