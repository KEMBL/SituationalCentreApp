using System;
using SystemLogics;

namespace XMLDataStorageLib
{
    /// <summary>
    /// This interface allows use different types of data storages
    /// </summary>
    public interface IDataStorage : IDisposable
    {
        /// <summary>
        /// Work state of the current component
        /// </summary>
        ComponentState State { get; set; }        
    }
}
