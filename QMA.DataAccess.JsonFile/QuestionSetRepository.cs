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
        private string _fileName;

        public QuestionSetRepository(string fileName)
        {
            _fileName = fileName;
        }

        /// <inheritdoc/>
        public QuestionSet GetByKey(string key)
        {
            using (var ds = new DataStore(_fileName, true, nameof(QuestionSet.PrimaryKey)))
            {
                var coll = ds.GetCollection<QuestionSet>();
                return coll.Find((x) => x.PrimaryKey == key).FirstOrDefault();
            }
        }

        /// <inheritdoc/>
        public IEnumerable<QuestionSet> GetAll()
        {
            using (var ds = new DataStore(_fileName, true, nameof(QuestionSet.PrimaryKey)))
            {
                var coll = ds.GetCollection<QuestionSet>();
                return coll.AsQueryable();
            }
        }

        /// <inheritdoc/>
        public void Add(QuestionSet value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            using (var ds = new DataStore(_fileName, true, nameof(QuestionSet.PrimaryKey)))
            {
                var coll = ds.GetCollection<QuestionSet>();
                var success = coll.InsertOne(value);
                if (success == false)
                {
                    throw new OperationFailedException("Add failed");
                }
            }
        }

        /// <inheritdoc/>
        public void Update(QuestionSet value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            using (var ds = new DataStore(_fileName, true, nameof(QuestionSet.PrimaryKey)))
            {
                var coll = ds.GetCollection<QuestionSet>();
                var success = coll.ReplaceOne(value.PrimaryKey, value);
                if (success == false)
                {
                    throw new OperationFailedException("Update failed");
                }
            }
        }
    }
}
