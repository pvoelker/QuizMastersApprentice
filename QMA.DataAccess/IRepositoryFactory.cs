using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.DataAccess
{
    public interface IRepositoryFactory
    {
        string Name { get; }

        Guid Id { get; }

        IQuestionRepository GetQuestionRepository();

        IQuestionSetRepository GetQuestionSetRepository();

        IQuizzerRepository GetQuizzerRepository();

        ISeasonRepository GetSeasonRepository();

        ITeamRepository GetTeamRepository();

        ITeamMemberRepository GetTeamMemberRepository();

        IAssignedQuestionRepository GetAssignedQuestionRepository();
    }
}
