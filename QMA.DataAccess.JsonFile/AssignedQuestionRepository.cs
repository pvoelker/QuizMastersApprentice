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

        public IEnumerable<AssignedQuestion> GetAll()
        {
            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
            {
                var coll = ds.GetCollection<AssignedQuestion>();
                return coll.AsQueryable();
            }
        }

        public AssignedQuestion GetByKey(string key)
        {
            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
            {
                var coll = ds.GetCollection<AssignedQuestion>();
                return coll.Find((x) => x.PrimaryKey == key).FirstOrDefault();
            }
        }

        public IEnumerable<AssignedQuestion> GetByQuestionId(string id)
        {
            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
            {
                var coll = ds.GetCollection<AssignedQuestion>();
                return coll.Find((x) => x.QuestionId == id);
            }
        }

        public IEnumerable<AssignedQuestion> GetByTeamMemberId(string id)
        {
            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
            {
                var coll = ds.GetCollection<AssignedQuestion>();
                return coll.AsQueryable().Where(x => x.TeamMemberId == id);
            }
        }

        public void Add(AssignedQuestion value)
        {
            if(value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
            {
                var coll = ds.GetCollection<AssignedQuestion>();
                var success = coll.InsertOne(value);
                if (success == false)
                {
                    throw new OperationFailedException("Add failed");
                }
            }
        }

        public void Update(AssignedQuestion value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
            {
                var coll = ds.GetCollection<AssignedQuestion>();
                var success = coll.ReplaceOne(value.PrimaryKey, value);
                if (success == false)
                {
                    throw new OperationFailedException("Update failed");
                }
            };
        }
    }
}
