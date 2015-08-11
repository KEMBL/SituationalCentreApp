using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLDataStorageLib;

namespace DataStorageProviderLib
{
    /// <summary>
    /// Storage provider which helps to store persistent data
    /// </summary>
    public class DataStorageProvider : IDataStorageProvider
    {
        /// <summary>
        /// Interface to data storage system
        /// </summary>
        private IDataStorage _xmlStorage;
        /// <summary>
        /// Initialize provider, check store and other factors
        /// </summary>
        public DataStorageProvider(IDataStorage xmlStorage)
        {            
            this._xmlStorage = xmlStorage;
        }
    }
}
