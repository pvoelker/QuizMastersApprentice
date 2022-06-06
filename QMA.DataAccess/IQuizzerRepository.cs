using QMA.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QMA.DataAccess
{
    public interface IQuizzerRepository
    {
        /// <summary>
        /// Get a quizzer by ID
        /// </summary>
        /// <param name="key">The ID of the quizzer to retrieve</param>
        /// <returns>A quizzer</returns>
        Quizzer GetByKey(string key);

        /// <summary>
        /// Gets all quizzers
        /// </summary>
        /// <returns>A list of quizzers</returns>
        IEnumerable<Quizzer> GetAll(bool includeDeleted);

        void Add(Quizzer value);

        void Update(Quizzer value);

        //void Delete(string key);
    }
}
