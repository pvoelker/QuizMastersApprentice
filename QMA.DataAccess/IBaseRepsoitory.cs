using QMA.Model.Season;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QMA.DataAccess
{
    /// <summary>
    /// Base repository interface for add and update functionality
    /// </summary>
    /// <typeparam name="T">The data model to use</typeparam>
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// Get value by primary key
        /// </summary>
        /// <param name="key">Primary key</param>
        /// <returns>A value</returns>
        T GetByKey(string key);

        /// <summary>
        /// Add a new value, cannot update an existing value
        /// </summary>
        /// <param name="value">The value to add</param>
        /// <returns>Task for asynchronous operation</returns>
        Task AddAsync(T value);

        /// <summary>
        /// Update an existing value, cannot add a new value
        /// </summary>
        /// <param name="value">The value to update</param>
        /// <returns>Task for asynchronous operation</returns>
        Task UpdateAsync(T value);
    }
}
