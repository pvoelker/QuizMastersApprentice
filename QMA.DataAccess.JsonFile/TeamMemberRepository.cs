﻿using JsonFlatFileDataStore;
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
        private string _fileName;

        public TeamMemberRepository(string fileName)
        {
            _fileName = fileName;
        }

        public IEnumerable<TeamMember> GetAll()
        {
            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
            {
                var coll = ds.GetCollection<TeamMember>();
                return coll.AsQueryable();
            }
        }

        public IEnumerable<TeamMember> GetByTeamId(string id)
        {
            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
            {
                var coll = ds.GetCollection<TeamMember>();
                return coll.AsQueryable().Where(x => x.TeamId == id);
            }
        }

        public TeamMember GetByKey(string key)
        {
            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
            {
                return ds.GetItem<TeamMember>(key);
            }
        }

        public void Add(TeamMember value)
        {
            if(value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
            {
                var coll = ds.GetCollection<TeamMember>();
                var success = coll.InsertOne(value);
                if (success == false)
                {
                    throw new OperationFailedException("Add failed");
                }
            }
        }

        public void Delete(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            using (var ds = new DataStore(_fileName, true, "PrimaryKey"))
            {
                var coll = ds.GetCollection<TeamMember>();
                var success = coll.DeleteOne(x => x.PrimaryKey == id);
                if (success == false)
                {
                    throw new OperationFailedException("Delete failed");
                }
            };
        }
    }
}