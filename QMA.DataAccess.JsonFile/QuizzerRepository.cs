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
    public class QuizzerRepository : IQuizzerRepository
    {
        public QuizzerRepository()
        {
        }

        /// <inheritdoc/>
        public Quizzer GetByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Primary key is required");
            }

            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<Quizzer>();
            return coll.Find((x) => x.PrimaryKey == key).FirstOrDefault();
        }

        /// <inheritdoc/>
        public IEnumerable<Quizzer> GetAll(bool includedDeleted)
        {
            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<Quizzer>();
            return includedDeleted ?
                coll.AsQueryable() :
                coll.AsQueryable().Where(x => x.Deleted == null);
        }

        /// <inheritdoc/>
        public async Task AddAsync(Quizzer value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<Quizzer>();
            var success = await coll.InsertOneAsync(value);
            if (success == false)
            {
                throw new OperationFailedException("Add failed");
            }
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Quizzer value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<Quizzer>();
            var success = await coll.ReplaceOneAsync(value.PrimaryKey, value);
            if (success == false)
            {
                throw new OperationFailedException("Update failed");
            }
        }

        /// <inheritdoc/>
        public string GetNewPrimaryKey()
        {
            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<Quizzer>();
            return coll.GetNextIdValue().ToString();
        }
    }
}
