using System;
using System.Collections.Generic;
using QMA.Model;

namespace QMA.DataAccess
{
    public interface IQuestionRepository
    {
        Question GetByKey(string key);

        IEnumerable<Question> GetAll();

        IEnumerable<Question> GetByQuestionNumber(string questionsSetId, int questionNumber, bool includeDeleted);

        IEnumerable<Question> GetByQuestionSetId(string id, bool includeDeleted);

        int CountByQuestionSetId(string id, int? maxQuestionPointValue, bool includeDeleted);

        void Add(Question value);

        void Update(Question value);
    }
}
