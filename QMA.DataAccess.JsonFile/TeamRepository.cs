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
    public class TeamRepository : ITeamRepository
    {
        private IDocumentCollection<Team> _coll;

        public TeamRepository()
        {
            _coll = DataStoreSingleton.Instance.DataStore.GetCollection<Team>();
        }

        /// <inheritdoc/>
        public Team GetByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Primary key is required");
            }

            return _coll.Find((x) => x.PrimaryKey == key).FirstOrDefault();
        }

        /// <inheritdoc/>
        public IEnumerable<Team> GetAll()
        {
            return _coll.AsQueryable();
        }

        /// <inheritdoc/>
        public IEnumerable<Team> GetBySeasonId(string id)
        {
            return _coll.AsQueryable().Where(x => x.SeasonId == id);
        }

        /// <inheritdoc/>
        public async Task AddAsync(Team value)
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
        public async Task UpdateAsync(Team value)
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
