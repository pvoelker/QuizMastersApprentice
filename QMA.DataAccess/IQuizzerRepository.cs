using QMA.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QMA.DataAccess
{
    public interface IQuizzerRepository : IBaseRepository<Quizzer>
    {
        /// <summary>
        /// Get all values
        /// </summary>
        /// <param name="includeDeleted">True to include deleted values</param>
        /// <returns>Question set values. Empty collection if no values</returns>
        IEnumerable<Quizzer> GetAll(bool includeDeleted);
    }
}
