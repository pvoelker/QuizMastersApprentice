using JsonFlatFileDataStore;
using QMA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMA.DataAccess.JsonFile
{
    public class QuestionSetRepository : IQuestionSetRepository
    {
        public QuestionSetRepository()
        {
        }

        /// <inheritdoc/>
        public QuestionSet GetByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Primary key is required");
            }

            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<QuestionSet>();
            return coll.Find((x) => x.PrimaryKey == key).FirstOrDefault();
        }

        /// <inheritdoc/>
        public IEnumerable<QuestionSet> GetAll()
        {
            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<QuestionSet>();
            return coll.AsQueryable();
        }

        /// <inheritdoc/>
        public void Add(QuestionSet value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<QuestionSet>();
            var success = coll.InsertOne(value);
            if (success == false)
            {
                throw new OperationFailedException("Add failed");
            }
        }

        /// <inheritdoc/>
        public void Update(QuestionSet value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<QuestionSet>();
            var success = coll.ReplaceOne(value.PrimaryKey, value);
            if (success == false)
            {
                throw new OperationFailedException("Update failed");
            }
        }
    }
}
