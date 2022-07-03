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
        public QuestionRepository()
        {
        }

        /// <inheritdoc/>
        public Question GetByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Primary key is required");
            }

            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<Question>();
            return coll.Find((x) => x.PrimaryKey == key).FirstOrDefault();
        }

        /// <inheritdoc/>
        public IEnumerable<Question> GetAll()
        {
            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<Question>();
            return coll.AsQueryable();
        }

        /// <inheritdoc/>
        public IEnumerable<Question> GetByQuestionNumber(string questionsSetId, int questionNumber, bool includeDeleted)
        {
            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<Question>();
            return coll.Find((x) => x.QuestionSetId == questionsSetId && x.Number == questionNumber && (includeDeleted || x.Deleted == null));
        }

        /// <inheritdoc/>
        public IEnumerable<Question> GetByQuestionSetId(string id, bool includeDeleted)
        {
            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<Question>();
            return coll.AsQueryable().Where(x => x.QuestionSetId == id && (includeDeleted || x.Deleted == null));
        }

        /// <inheritdoc/>
        public int CountByQuestionSetId(string id, int? maxQuestionPointValue, bool includeDeleted)
        {
            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<Question>();
            return coll.AsQueryable().Where(x => x.QuestionSetId == id
                && (includeDeleted || x.Deleted == null)
                && (!maxQuestionPointValue.HasValue || x.Points <= maxQuestionPointValue)).Count();
        }

        /// <inheritdoc/>
        public void Add(Question value)
        {
            if(value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<Question>();
            var success = coll.InsertOne(value);
            if (success == false)
            {
                throw new OperationFailedException("Add failed");
            }
        }

        /// <inheritdoc/>
        public void Update(Question value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<Question>();
            var success = coll.ReplaceOne(value.PrimaryKey, value);
            if (success == false)
            {
                throw new OperationFailedException("Update failed");
            }
        }
    }
}
