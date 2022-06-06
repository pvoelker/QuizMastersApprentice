using QMA.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QMA.DataAccess
{
    public interface IQuestionSetRepository
    {
        QuestionSet GetByKey(string key);

        IEnumerable<QuestionSet> GetAll();

        void Add(QuestionSet value);

        void Update(QuestionSet value);
    }
}
