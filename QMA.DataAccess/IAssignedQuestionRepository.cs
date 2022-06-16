using System;
using System.Collections.Generic;
using QMA.Model.Season;

namespace QMA.DataAccess
{
    public interface IAssignedQuestionRepository
    {
        AssignedQuestion GetByKey(string key);

        IEnumerable<AssignedQuestion> GetAll();

        IEnumerable<AssignedQuestion> GetByQuestionId(string Id);

        IEnumerable<AssignedQuestion> GetByTeamMemberId(string id);

        void Add(AssignedQuestion value);

        void Update(AssignedQuestion value);
    }
}
