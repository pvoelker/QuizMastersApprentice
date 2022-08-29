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
    public class SeasonRepository : ISeasonRepository
    {
        private IDocumentCollection<SeasonInfo> _coll;

        public SeasonRepository()
        {
            _coll = DataStoreSingleton.Instance.DataStore.GetCollection<SeasonInfo>();
        }

        /// <inheritdoc/>
        public SeasonInfo GetByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Primary key is required");
            }

            return _coll.Find((x) => x.PrimaryKey == key).FirstOrDefault();
        }

        /// <inheritdoc/>
        public IEnumerable<SeasonInfo> GetAll(bool includedDeleted)
        {
            return includedDeleted ?
                _coll.AsQueryable() :
                _coll.AsQueryable().Where(x => x.Deleted == null);
        }

        /// <inheritdoc/>
        public async Task AddAsync(SeasonInfo value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var success = await _coll.InsertOneAsync(value);
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
