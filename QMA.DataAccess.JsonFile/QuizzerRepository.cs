using JsonFlatFileDataStore;
using QMA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMA.DataAccess.JsonFile
{
    public class QuizzerRepository : IQuizzerRepository
    {
        private string _fileName;

        public QuizzerRepository(string fileName)
        {
            _fileName = fileName;
        }

        public IEnumerable<Quizzer> GetAll(bool includedDeleted)
        {
            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
            {
                var coll = ds.GetCollection<Quizzer>();
                return includedDeleted ?
                    coll.AsQueryable() :
                    coll.AsQueryable().Where(x => x.Deleted == null);
            }
       }

        public Quizzer GetByKey(string key)
        {
            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
            {
                var coll = ds.GetCollection<Quizzer>();
                return coll.Find((x) => x.PrimaryKey == key).FirstOrDefault();
            }
        }

        public void Add(Quizzer value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
            {
                var coll = ds.GetCollection<Quizzer>();
                var success = coll.InsertOne(value);
                if (success == false)
                {
                    throw new OperationFailedException("Add failed");
                }
            }
        }

        public void Update(Quizzer value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
            {
                var coll = ds.GetCollection<Quizzer>();
                var success = coll.ReplaceOne(value.PrimaryKey, value);
                if (success == false)
                {
                    throw new OperationFailedException("Update failed");
                }
            }
        }

        //public void Delete(string key)
        //{
        //    if (key == null)
        //    {
        //        throw new ArgumentNullException(nameof(key));
        //    }

        //    using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
        //    {
        //        var coll = ds.GetCollection<Quizzer>();
        //        var success = coll.DeleteOne(key);
        //        if (success == false)
        //        {
        //            throw new OperationFailedException("Delete failed");
        //        }
        //    }
        //}
    }
}
