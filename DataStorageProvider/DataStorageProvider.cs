using SystemLogics;
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
        /// State of the current component
        /// </summary>
        public ComponentState State { get; set; }
        /// <summary>
        /// Initialize provider, check store and other factors
        /// </summary>
        public DataStorageProvider(IDataStorage xmlStorage)
        {            
            this._xmlStorage = xmlStorage;

            State = ComponentState.Ready;
        }
        /// <summary>
        /// Dispose object
        /// </summary>
        public void Dispose()
        {
            State = ComponentState.NoReady;
            if (_xmlStorage != null)
            {                
                _xmlStorage.Dispose();
                _xmlStorage = null;
            }            
        }
    }
}
