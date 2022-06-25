using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.DataAccess.JsonFile
{
    public class RepositoryFactory : IRepositoryFactory
    {
        static private Guid _id = new Guid("cc284335-86e8-4bd2-b4fb-3dfbe503bb28");

        private string _fileName = null;

        public RepositoryFactory()
        {
        }

        public RepositoryFactory(string filename)
        {
            _fileName = filename;
        }

        public string Name { get { return "Local File"; } }

        public Guid Id { get { return _id;  } }

        public IQuestionRepository GetQuestionRepository()
        {
            if (_fileName == null)
            {
                throw new NullReferenceException("File name is not set");
            }

            return new QuestionRepository(_fileName);
        }

        public IQuestionSetRepository GetQuestionSetRepository()
        {
            if (_fileName == null)
            {
                throw new NullReferenceException("File name is not set");
            }

            return new QuestionSetRepository(_fileName);
        }

        public IQuizzerRepository GetQuizzerRepository()
        {
            if (_fileName == null)
            {
                throw new NullReferenceException("File name is not set");
            }

            return new QuizzerRepository(_fileName);
        }

        public ISeasonRepository GetSeasonRepository()
        {
            if (_fileName == null)
            {
                throw new NullReferenceException("File name is not set");
            }

            return new SeasonRepository(_fileName);
        }

        public ITeamRepository GetTeamRepository()
        {
            if (_fileName == null)
            {
                throw new NullReferenceException("File name is not set");
            }

            return new TeamRepository(_fileName);
        }

        public ITeamMemberRepository GetTeamMemberRepository()
        {
            if (_fileName == null)
            {
                throw new NullReferenceException("File name is not set");
            }

            return new TeamMemberRepository(_fileName);
        }

        public IAssignedQuestionRepository GetAssignedQuestionRepository()
        {
            if (_fileName == null)
            {
                throw new NullReferenceException("File name is not set");
            }

            return new AssignedQuestionRepository(_fileName);
        }
    }
}
