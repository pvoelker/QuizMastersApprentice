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
            _fileName = fileName;
        }

        public IEnumerable<SeasonInfo> GetAll(bool includedDeleted)
        {
            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
            {
                var coll = ds.GetCollection<SeasonInfo>();
                return includedDeleted ?
                    coll.AsQueryable() :
                    coll.AsQueryable().Where(x => x.Deleted == null);
            }
       }

        public SeasonInfo GetByKey(string key)
        {
            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
            {
                var coll = ds.GetCollection<SeasonInfo>();
                return coll.Find((x) => x.PrimaryKey == key).FirstOrDefault();
            }
        }

        public void Add(SeasonInfo value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
            {
                var coll = ds.GetCollection<SeasonInfo>();
                var success = coll.InsertOne(value);
                if(success == false)
                {
                    throw new OperationFailedException("Add failed");
                }
            }
        }

        public void Update(SeasonInfo value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
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
