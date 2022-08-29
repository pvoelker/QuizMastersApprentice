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
    public class TeamMemberRepository : ITeamMemberRepository
    {
        private IDocumentCollection<TeamMember> _coll;

        public TeamMemberRepository()
        {
            _coll = DataStoreSingleton.Instance.DataStore.GetCollection<TeamMember>();
        }

        /// <inheritdoc/>
        public TeamMember GetByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Primary key is required");
            }

            return _coll.Find((x) => x.PrimaryKey == key).FirstOrDefault();
        }

        /// <inheritdoc/>
        public IEnumerable<TeamMember> GetAll()
        {
            return _coll.AsQueryable();
        }

        /// <inheritdoc/>
        public IEnumerable<TeamMember> GetByTeamId(string id)
        {
            return _coll.AsQueryable().Where(x => x.TeamId == id);
        }

        /// <inheritdoc/>
        public async Task AddAsync(TeamMember value)
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
        public async Task DeleteAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Primary key is required");
            }

            var success = await _coll.DeleteOneAsync(x => x.PrimaryKey == key);
            if (success == false)
            {
                throw new OperationFailedException("Delete failed");
            }
        }

        /// <inheritdoc/>
        public string GetNewPrimaryKey()
        {
            return _coll.GetNextIdValue().ToString();
        }
    }
}
