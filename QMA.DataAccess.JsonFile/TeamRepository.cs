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
        private string _fileName;

        public TeamRepository(string fileName)
        {
            _fileName = fileName;
        }

        public IEnumerable<Team> GetAll()
        {
            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
            {
                var coll = ds.GetCollection<Team>();
                return coll.AsQueryable();
            }
        }

        public IEnumerable<Team> GetBySeasonId(string id)
        {
            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
            {
                var coll = ds.GetCollection<Team>();
                return coll.AsQueryable().Where(x => x.SeasonId == id);
            }
        }

        public Team GetByKey(string key)
        {
            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
            {
                return ds.GetItem<Team>(key);
            }
        }

        public void Add(Team value)
        {
            if(value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
            {
                var coll = ds.GetCollection<Team>();
                var success = coll.InsertOne(value);
                if (success == false)
                {
                    throw new OperationFailedException("Add failed");
                }
            }
        }

        public void Update(Team value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
            {
                var coll = ds.GetCollection<Team>();
                var success = coll.ReplaceOne(value.PrimaryKey, value);
                if (success == false)
                {
                    throw new OperationFailedException("Update failed");
                }
            };
        }
    }
}
