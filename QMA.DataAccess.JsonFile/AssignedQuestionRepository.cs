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
    public class AssignedQuestionRepository : IAssignedQuestionRepository
    {
        public AssignedQuestionRepository()
        {
        }

        /// <inheritdoc/>
        public AssignedQuestion GetByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Primary key is required");
            }

            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<AssignedQuestion>();
            return coll.Find((x) => x.PrimaryKey == key).FirstOrDefault();
        }

        /// <inheritdoc/>
        public IEnumerable<AssignedQuestion> GetAll()
        {
            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<AssignedQuestion>();
            return coll.AsQueryable();
        }

        /// <inheritdoc/>
        public IEnumerable<AssignedQuestion> GetByQuestionId(string id)
        {
            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<AssignedQuestion>();
            return coll.Find((x) => x.QuestionId == id);
        }

        /// <inheritdoc/>
        public IEnumerable<AssignedQuestion> GetByTeamMemberId(string id)
        {
            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<AssignedQuestion>();
            return coll.AsQueryable().Where(x => x.TeamMemberId == id);
        }

        /// <inheritdoc/>
        public async Task AddAsync(AssignedQuestion value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<AssignedQuestion>();
            var success = await coll.InsertOneAsync(value);
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

            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<AssignedQuestion>();
            var success = await coll.DeleteOneAsync(x => x.PrimaryKey == key);
            if (success == false)
            {
                throw new OperationFailedException("Delete failed");
            }
        }

        /// <inheritdoc/>
        public async Task DeleteAllByTeamMemberIdAsync(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<AssignedQuestion>();
            var success = await coll.DeleteManyAsync(x => x.TeamMemberId == id);
            if (success == false)
            {
                throw new OperationFailedException("Delete failed");
            }
        }

        /// <inheritdoc/>
        public string GetNewPrimaryKey()
        {
            var coll = DataStoreSingleton.Instance.DataStore.GetCollection<AssignedQuestion>();
            return coll.GetNextIdValue().ToString();
        }        
    }
}
