using JsonFlatFileDataStore;
using QMA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMA.DataAccess.JsonFile
{
    public class SeasonRepository : ISeasonRepository
    {
        private string _fileName;

        public SeasonRepository(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            _fileName = fileName;
        }

        /// <inheritdoc/>
        public SeasonInfo GetByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Primary key is required");
            }

            using (var ds = new DataStore(_fileName, true, nameof(SeasonInfo.PrimaryKey)))
            {
                var coll = ds.GetCollection<SeasonInfo>();
                return coll.Find((x) => x.PrimaryKey == key).FirstOrDefault();
            }
        }

        /// <inheritdoc/>
        public IEnumerable<SeasonInfo> GetAll(bool includedDeleted)
        {
            using (var ds = new DataStore(_fileName, true, nameof(SeasonInfo.PrimaryKey)))
            {
                var coll = ds.GetCollection<SeasonInfo>();
                return includedDeleted ?
                    coll.AsQueryable() :
                    coll.AsQueryable().Where(x => x.Deleted == null);
            }
        }

        /// <inheritdoc/>
        public void Add(SeasonInfo value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            using (var ds = new DataStore(_fileName, true, nameof(SeasonInfo.PrimaryKey)))
            {
                var coll = ds.GetCollection<SeasonInfo>();
                var success = coll.InsertOne(value);
                if(success == false)
                {
                    throw new OperationFailedException("Add failed");
                }
            }
        }

        /// <inheritdoc/>
        public void Update(SeasonInfo value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            using (var ds = new DataStore(_fileName, true, nameof(SeasonInfo.PrimaryKey)))
            {
                var coll = ds.GetCollection<SeasonInfo>();
                var success = coll.ReplaceOne(value.PrimaryKey, value);
                if (success == false)
                {
                    throw new OperationFailedException("Update failed");
                }
            };
        }
    }
}
