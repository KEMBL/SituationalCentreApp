using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemLogics;


namespace DataStorageProviderLib
{
    /// <summary>
    /// Interface storage provider which helps to store persistent data
    /// </summary>
    public interface IDataStorageProvider : IDisposable
    {
        /// <summary>
        /// Work state of the current component
        /// </summary>
        ComponentState State { get; set; }
    }
}
