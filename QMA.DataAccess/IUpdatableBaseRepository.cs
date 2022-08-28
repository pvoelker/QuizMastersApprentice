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
    public interface IUpdatableBaseRepository<T> : IBaseRepository<T> where T : class
    {
        /// <summary>
        /// Update an existing value, cannot add a new value
        /// </summary>
        /// <param name="value">The value to update</param>
        /// <returns>Task for asynchronous operation</returns>
        Task UpdateAsync(T value);
    }
}
