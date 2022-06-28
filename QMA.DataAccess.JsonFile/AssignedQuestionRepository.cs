using JsonFlatFileDataStore;
using QMA.Model;
using QMA.Model.Season;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMA.DataAccess.JsonFile
{
    public class AssignedQuestionRepository : IAssignedQuestionRepository
    {
        private string _fileName;

        public AssignedQuestionRepository(string fileName)
        {
            _fileName = fileName;
        }

        /// <inheritdoc/>
        public AssignedQuestion GetByKey(string key)
        {
            using (var ds = new DataStore(_fileName, true, nameof(AssignedQuestion.PrimaryKey)))
            {
                var coll = ds.GetCollection<AssignedQuestion>();
                return coll.Find((x) => x.PrimaryKey == key).FirstOrDefault();
            }
        }

        /// <inheritdoc/>
        public IEnumerable<AssignedQuestion> GetAll()
        {
            using (var ds = new DataStore(_fileName, true, nameof(AssignedQuestion.PrimaryKey)))
            {
                var coll = ds.GetCollection<AssignedQuestion>();
                return coll.AsQueryable();
            }
        }

        public IEnumerable<AssignedQuestion> GetByQuestionId(string id)
        {
            using (var ds = new DataStore(_fileName, true, nameof(AssignedQuestion.PrimaryKey)))
            {
                var coll = ds.GetCollection<AssignedQuestion>();
                return coll.Find((x) => x.QuestionId == id);
            }
        }

        public IEnumerable<AssignedQuestion> GetByTeamMemberId(string id)
        {
            using (var ds = new DataStore(_fileName, true, nameof(AssignedQuestion.PrimaryKey)))
            {
                var coll = ds.GetCollection<AssignedQuestion>();
                return coll.AsQueryable().Where(x => x.TeamMemberId == id);
            }
        }

        public void Add(AssignedQuestion value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            using (var ds = new DataStore(_fileName, true, nameof(AssignedQuestion.PrimaryKey)))
            {
                var coll = ds.GetCollection<AssignedQuestion>();
                var success = coll.InsertOne(value);
                if (success == false)
                {
                    throw new OperationFailedException("Add failed");
                }
            }
        }

        public void Delete(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            using (var ds = new DataStore(_fileName, true, nameof(AssignedQuestion.PrimaryKey)))
            {
                var coll = ds.GetCollection<AssignedQuestion>();
                var success = coll.DeleteOne(x => x.PrimaryKey == id);
                if (success == false)
                {
                    throw new OperationFailedException("Delete failed");
                }
            };
        }

        public void DeleteAllByTeamMemberId(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            using (var ds = new DataStore(_fileName, true, nameof(AssignedQuestion.PrimaryKey)))
            {
                var coll = ds.GetCollection<AssignedQuestion>();
                var success = coll.DeleteMany(x => x.TeamMemberId == id);
                if (success == false)
                {
                    throw new OperationFailedException("Delete failed");
                }
            };
        }
    }
}
