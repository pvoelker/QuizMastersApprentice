using JsonFlatFileDataStore;
using QMA.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.DataAccess.JsonFile
{
    /// <summary>
    /// Thread-safe singleton for the JsonFlatFileDataStore.DataStore object. '<see cref="FileName"/>' must be set before an instance can be retrieved
    /// </summary>
    public sealed class DataStoreSingleton
    {
        private static readonly Lazy<DataStoreSingleton> lazy =
            new Lazy<DataStoreSingleton>(() => new DataStoreSingleton());

        /// <summary>
        /// Full path name of JSON data store file
        /// </summary>
        /// <remarks>Must be set before '<see cref="Instance"/>' is called</remarks>
        public static string FileName { get; set; }

        /// <summary>
        /// Get singleton instance
        /// </summary>
        public static DataStoreSingleton Instance { get { return lazy.Value; } }

        private DataStore _dataStore = null;
        /// <summary>
        /// '<see cref="JsonFlatFileDataStore"/>' data store
        /// </summary>
        public DataStore DataStore { get => _dataStore; }
        
        /// <exception cref="InvalidOperationException">'<see cref="FileName"/>' has not been set</exception>
        private DataStoreSingleton()
        {
            if(string.IsNullOrWhiteSpace(FileName))
            {
                throw new InvalidOperationException($"'{nameof(FileName)}' must be set first");
            }

            _dataStore = new DataStore(FileName, true, nameof(PrimaryKeyBase.PrimaryKey));
        }
    }
}
