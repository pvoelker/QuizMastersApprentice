using JsonFlatFileDataStore;
using QMA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMA.DataAccess.JsonFile
{
    public class QuizzerRepository : IQuizzerRepository
    {
        private string _fileName;

        public QuizzerRepository(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            _fileName = fileName;
        }

        /// <inheritdoc/>
        public Quizzer GetByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Primary key is required");
            }

            using (var ds = new DataStore(_fileName, true, nameof(Quizzer.PrimaryKey)))
            {
                var coll = ds.GetCollection<Quizzer>();
                return coll.Find((x) => x.PrimaryKey == key).FirstOrDefault();
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Quizzer> GetAll(bool includedDeleted)
        {
            using (var ds = new DataStore(_fileName, true, nameof(Quizzer.PrimaryKey)))
            {
                var coll = ds.GetCollection<Quizzer>();
                return includedDeleted ?
                    coll.AsQueryable() :
                    coll.AsQueryable().Where(x => x.Deleted == null);
            }
        }

        /// <inheritdoc/>
        public void Add(Quizzer value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            using (var ds = new DataStore(_fileName, true, nameof(Quizzer.PrimaryKey)))
            {
                var coll = ds.GetCollection<Quizzer>();
                var success = coll.InsertOne(value);
                if (success == false)
                {
                    throw new OperationFailedException("Add failed");
                }
            }
        }

        /// <inheritdoc/>
        public void Update(Quizzer value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            using (var ds = new DataStore(_fileName, true, nameof(Quizzer.PrimaryKey)))
            {
                var coll = ds.GetCollection<Quizzer>();
                var success = coll.ReplaceOne(value.PrimaryKey, value);
                if (success == false)
                {
                    throw new OperationFailedException("Update failed");
                }
            }
        }
    }
}
