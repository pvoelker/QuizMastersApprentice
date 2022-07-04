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
        public SeasonRepository()
        {
        }

        /// <inheritdoc/>
        public SeasonInfo GetByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Primary key is required");
            }

            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<SeasonInfo>();
            return coll.Find((x) => x.PrimaryKey == key).FirstOrDefault();
        }

        /// <inheritdoc/>
        public IEnumerable<SeasonInfo> GetAll(bool includedDeleted)
        {
            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<SeasonInfo>();
            return includedDeleted ?
                coll.AsQueryable() :
                coll.AsQueryable().Where(x => x.Deleted == null);
        }

        /// <inheritdoc/>
        public async Task AddAsync(SeasonInfo value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<SeasonInfo>();
            var success = await coll.InsertOneAsync(value);
            if(success == false)
            {
                throw new OperationFailedException("Add failed");
            }
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(SeasonInfo value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<SeasonInfo>();
            var success = await coll.ReplaceOneAsync(value.PrimaryKey, value);
            if (success == false)
            {
                throw new OperationFailedException("Update failed");
            }
        }
    }
}
