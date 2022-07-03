using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.DataAccess.JsonFile
{
    public class RepositoryFactory : IRepositoryFactory
    {
        static private Guid _id = new Guid("cc284335-86e8-4bd2-b4fb-3dfbe503bb28");

        public RepositoryFactory()
        {
        }

        public RepositoryFactory(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            DataStoreSingleton.FileName = fileName;
        }

        public string Name { get { return "Local File"; } }

        public Guid Id { get { return _id;  } }

        public IQuestionRepository GetQuestionRepository()
        {
            return new QuestionRepository();
        }

        public IQuestionSetRepository GetQuestionSetRepository()
        {
            return new QuestionSetRepository();
        }

        public IQuizzerRepository GetQuizzerRepository()
        {
            return new QuizzerRepository();
        }

        public ISeasonRepository GetSeasonRepository()
        {
            return new SeasonRepository();
        }

        public ITeamRepository GetTeamRepository()
        {
            return new TeamRepository();
        }

        public ITeamMemberRepository GetTeamMemberRepository()
        {
            return new TeamMemberRepository();
        }

        public IAssignedQuestionRepository GetAssignedQuestionRepository()
        {
            return new AssignedQuestionRepository();
        }
    }
}
