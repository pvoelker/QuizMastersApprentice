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
        private IDocumentCollection<Question> _coll;

        public QuestionRepository()
        {
            _coll = DataStoreSingleton.Instance.DataStore.GetCollection<Question>();
        }

        /// <inheritdoc/>
        public Question GetByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Primary key is required");
            }

            return _coll.Find((x) => x.PrimaryKey == key).FirstOrDefault();
        }

        /// <inheritdoc/>
        public IEnumerable<Question> GetAll()
        {
            return _coll.AsQueryable();
        }

        /// <inheritdoc/>
        public IEnumerable<Question> GetByQuestionNumber(string questionsSetId, int questionNumber, bool includeDeleted)
        {
            return _coll.Find((x) => x.QuestionSetId == questionsSetId && x.Number == questionNumber && (includeDeleted || x.Deleted == null));
        }

        /// <inheritdoc/>
        public IEnumerable<Question> GetByQuestionSetId(string id, bool includeDeleted)
        {
            return _coll.AsQueryable().Where(x => x.QuestionSetId == id && (includeDeleted || x.Deleted == null));
        }

        /// <inheritdoc/>
        public int CountByQuestionSetId(string id, int? maxQuestionPointValue, bool includeDeleted)
        {
            return _coll.AsQueryable().Where(x => x.QuestionSetId == id
                && (includeDeleted || x.Deleted == null)
                && (!maxQuestionPointValue.HasValue || x.Points <= maxQuestionPointValue)).Count();
        }

        /// <inheritdoc/>
        public async Task AddAsync(Question value)
        {
            if(value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var success = await _coll.InsertOneAsync(value);
            if (success == false)
            {
                throw new OperationFailedException("Add failed");
            }
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Question value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var success = await _coll.ReplaceOneAsync(value.PrimaryKey, value);
            if (success == false)
            {
                throw new OperationFailedException("Update failed");
            }
        }

        /// <inheritdoc/>
        public string GetNewPrimaryKey()
        {
            return _coll.GetNextIdValue().ToString();
        }
    }
}
