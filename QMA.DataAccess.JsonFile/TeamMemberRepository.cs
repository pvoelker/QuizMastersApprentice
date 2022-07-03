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
        public TeamMemberRepository()
        {
        }

        /// <inheritdoc/>
        public TeamMember GetByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Primary key is required");
            }

            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<TeamMember>();
            return coll.Find((x) => x.PrimaryKey == key).FirstOrDefault();
        }

        /// <inheritdoc/>
        public IEnumerable<TeamMember> GetAll()
        {
            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<TeamMember>();
            return coll.AsQueryable();
        }

        /// <inheritdoc/>
        public IEnumerable<TeamMember> GetByTeamId(string id)
        {
            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<TeamMember>();
            return coll.AsQueryable().Where(x => x.TeamId == id);
        }

        /// <inheritdoc/>
        public void Add(TeamMember value)
        {
            if(value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<TeamMember>();
            var success = coll.InsertOne(value);
            if (success == false)
            {
                throw new OperationFailedException("Add failed");
            }
        }

        /// <inheritdoc/>
        public void Delete(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Primary key is required");
            }

            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<TeamMember>();
            var success = coll.DeleteOne(x => x.PrimaryKey == key);
            if (success == false)
            {
                throw new OperationFailedException("Delete failed");
            }
        }
    }
}
