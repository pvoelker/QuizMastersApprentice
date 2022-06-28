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
    public class QuestionRepository : IQuestionRepository
    {
        private string _fileName;

        public QuestionRepository(string fileName)
        {
            _fileName = fileName;
        }

        /// <inheritdoc/>
        public Question GetByKey(string key)
        {
            using (var ds = new DataStore(_fileName, true, nameof(Question.PrimaryKey)))
            {
                var coll = ds.GetCollection<Question>();
                return coll.Find((x) => x.PrimaryKey == key).FirstOrDefault();
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Question> GetAll()
        {
            using (var ds = new DataStore(_fileName, true, nameof(Question.PrimaryKey)))
            {
                var coll = ds.GetCollection<Question>();
                return coll.AsQueryable();
            }
        }

        public IEnumerable<Question> GetByQuestionNumber(string questionsSetId, int questionNumber, bool includeDeleted)
        {
            using (var ds = new DataStore(_fileName, true, nameof(Question.PrimaryKey)))
            {
                var coll = ds.GetCollection<Question>();
                return coll.Find((x) => x.QuestionSetId == questionsSetId && x.Number == questionNumber && (includeDeleted || x.Deleted == null));
            }
        }

        public IEnumerable<Question> GetByQuestionSetId(string id, bool includeDeleted)
        {
            using (var ds = new DataStore(_fileName, true, nameof(Question.PrimaryKey)))
            {
                var coll = ds.GetCollection<Question>();
                return coll.AsQueryable().Where(x => x.QuestionSetId == id && (includeDeleted || x.Deleted == null));
            }
        }

        public int CountByQuestionSetId(string id, int? maxQuestionPointValue, bool includeDeleted)
        {
            using (var ds = new DataStore(_fileName, true, nameof(Question.PrimaryKey)))
            {
                var coll = ds.GetCollection<Question>();
                return coll.AsQueryable().Where(x => x.QuestionSetId == id
                && (includeDeleted || x.Deleted == null)
                && (!maxQuestionPointValue.HasValue || x.Points <= maxQuestionPointValue)).Count();
            }
        }

        public void Add(Question value)
        {
            if(value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            using (var ds = new DataStore(_fileName, true, nameof(Question.PrimaryKey)))
            {
                var coll = ds.GetCollection<Question>();
                var success = coll.InsertOne(value);
                if (success == false)
                {
                    throw new OperationFailedException("Add failed");
                }
            }
        }

        public void Update(Question value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            using (var ds = new DataStore(_fileName, true, nameof(Question.PrimaryKey)))
            {
                var coll = ds.GetCollection<Question>();
                var success = coll.ReplaceOne(value.PrimaryKey, value);
                if (success == false)
                {
                    throw new OperationFailedException("Update failed");
                }
            };
        }
    }
}
